using System;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace ksc.Service.ToolGen
{
    public static class GenRes
    {
        private static readonly string SourceSubPath = "配置/develop";
        private const string ArtResPath = "ArtResPath";
        private static readonly string TargetPath = Path.Combine(Application.dataPath.Replace("Assets", ""), "Tools/res/excel");
        
        public static void Export(bool genCs)
        {
            ClearOldGen();

            if (genCs && !GenDataCS())
            {
                Debug.LogError("生成cs文件错误");
                return;
            }
            if (!UpdateDir())
            {
                Debug.LogError("更新表错误");
                return;
            }
            if (!GenClientExcelDataBytes())
            {
                Debug.LogError("生成bytes文件错误");
                return;
            }

            Copy2Dest(genCs);
            ClearOldGen();

            if (genCs)
            {
                try
                {
                    // 导出常量类
                    // PBMenu.MakeAll();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }

            Debug.Log("生成成功");
        }

        private static bool GenDataCS()
        {
            var workDirectory = SyncTools.GetToolGenPath();
            var protocPath = Path.Combine(workDirectory, ExternalProcessInvoke.GetProtocFileName());
            var protoPath = "./res/proto/client";
            var importPath = "./res/raw_proto;./res/proto/client";
            var csPath = "./res/gen/cs";
            var binFile = "./res/gen/bin/proto_res.bytes";

            IOUtils.CreateAllSubPath(workDirectory, csPath.Replace(".", string.Empty) + "/");
            IOUtils.CreateAllSubPath(workDirectory, binFile);

            return ExternalProcessInvoke.Proto2csAllRelativeProtoPath(protocPath, protoPath, importPath, csPath,
                workDirectory, binFile);
        }

        private static bool GenClientExcelDataBytes()
        {
            var workDirectory = SyncTools.GetTablePath();
            var tsv2pbPath = Path.Combine(workDirectory, GetTsv2pbFileName());
            var inputPath = "./excel";
            var tsvPath = "./tsv/client";
            var protoPath = "./proto/client";
            var storePath = "./store/client";
            var storeSuffix = "bytes";
            return ExternalProcessInvoke.xls2pb(tsv2pbPath, inputPath, tsvPath, protoPath, storePath, storeSuffix, workDirectory);
        }

        /// <summary>
        /// 更新SVN表 + 拷贝表
        /// </summary>
        private static bool UpdateDir()
        {
            var artResPath = string.Empty;
            if (SelectPath(ArtResPath, ref artResPath))
            {
                // 1更新
                ExternalProcessInvoke.InvokeProcess("svn", "up", artResPath);
                // 2拷贝
                var sourcePath = Path.Combine(artResPath, SourceSubPath);
                var targetPath = TargetPath;
                if (!Directory.Exists(targetPath)) {
                    Directory.CreateDirectory(targetPath);
                }
                IOUtils.CopyFilesRecursively(new DirectoryInfo(sourcePath), new DirectoryInfo(targetPath), true, new []{"*.xlsx"}, 1);
                return true;
            }

            return false;
        }
        
        private static bool SelectPath(string key, ref string path)
        {
            var rootPath = PlayerPrefs.GetString(key);

            if (string.IsNullOrEmpty(rootPath)) {
                if (!EditorUtility.DisplayDialog("提示", "请选择美术资源目录", "确定")) {
                    return false;
                }
                rootPath = SyncTools.FolderChoose(key);
                if (string.IsNullOrEmpty(rootPath)) {
                    EditorUtility.DisplayDialog("提示", "未选择美术资源目录", "确定");
                    return false;
                }
            }

            if (!Directory.Exists(rootPath)) {
                PlayerPrefs.SetString(key, null);
                if (!EditorUtility.DisplayDialog("提示", "该路径不存在:" + rootPath + ",请重新选择目录", "确定")) {
                    Debug.LogErrorFormat("路径指定错误：{0}", rootPath);
                    return SelectPath(key, ref path);
                }
            }

            path = rootPath;
            PlayerPrefs.SetString(key, rootPath);
            return true;
        }
        
        private static void GenProto2Lua()
        {
            // todo
            /*var param = new[]
            {
                "20", // keywords
                "./res/proto/client/keywords.proto",
                "../../Assets/res/lua/data/res/keywords.lua.txt",
            };
            if (!ExternalProcessInvoke.GenMakeLuaProtoConfig(makeLuaProtoConfigPath, workDirectory, param))
            {
                return false;
            }

            var param1 = new[]
            {
                "20", // keywords_client
                "./res/proto/client/keywords_client.proto",
                "../../Assets/res/lua/data/res/keywords_client.lua.txt",
            };
            if (!ExternalProcessInvoke.GenMakeLuaProtoConfig(makeLuaProtoConfigPath, workDirectory, param1))
            {
                return false;
            }*/
        }

        private static void ClearOldGen()
        {
            string toolGenPath = SyncTools.GetToolGenPath();
            string sourceCsPath = Path.Combine(toolGenPath, "res/gen");
            if (!Directory.Exists(sourceCsPath))
            {
                return;
            }

            Directory.Delete(sourceCsPath, true);
        }

        private static void Copy2Dest(bool copyCs)
        {
            string toolGenPath = SyncTools.GetToolGenPath();
            DirectoryInfo sourceDir;
            DirectoryInfo destDir;
            if (copyCs)
            {
                // copy cs
                string sourceCsPath = Path.Combine(toolGenPath, "res/gen/cs");
                string destCsPath = Path.Combine(Application.dataPath, "Scripts/Proto/Table");
                sourceDir = new DirectoryInfo(sourceCsPath);
                destDir = new DirectoryInfo(destCsPath);
                IOUtils.CopyFilesRecursively(sourceDir, destDir, true, ".cs");

                // copy bin
                var sourceBinPath = Path.Combine(toolGenPath, "res/gen/bin");
                var destBinPath = Path.Combine(Application.dataPath, "Resource/ABResources/data/proto");
                sourceDir = new DirectoryInfo(sourceBinPath);
                destDir = new DirectoryInfo(destBinPath);
                IOUtils.CopyFilesRecursively(sourceDir, destDir, true, ".bytes");
            }

            string sourceBytesPath = Path.Combine(toolGenPath, "res/store/client");
            string destBytesPath = Path.Combine(Application.dataPath, "Resource/ABResources/data/excel_data");
            sourceDir = new DirectoryInfo(sourceBytesPath);
            destDir = new DirectoryInfo(destBytesPath);
            CopyFilesDataBytes(sourceDir, destDir, true, ".bytes");
            
            AssetDatabase.Refresh();
        }

        private static void CopyFilesDataBytes(DirectoryInfo source, DirectoryInfo target, bool overwrite,
            string extension = null, string include = null, string exclude = null)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                if ((dir.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden &&
                    (dir.Attributes & FileAttributes.System) != FileAttributes.System)
                    CopyFilesDataBytes(dir, target.CreateSubdirectory(dir.Name), overwrite, extension);
            }

            foreach (FileInfo file in source.GetFiles())
            {
                if (!string.IsNullOrEmpty(extension))
                {
                    if (!string.Equals(file.Extension, extension, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(exclude) && file.Name.IndexOf(exclude) != -1)
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(include) && file.Name.IndexOf(include) == -1)
                    {
                        continue;
                    }

                    file.CopyTo(Path.Combine(target.FullName, file.Name), overwrite);
                }
                else
                {
                    file.CopyTo(Path.Combine(target.FullName, file.Name), overwrite);
                }
            }
        }
        
        private static string GetTsv2pbFileName()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    return "xls2pb_cpp.exe";
                case RuntimePlatform.OSXEditor:
                    return "xls2pb_cpp";
                default:
                    return string.Empty;
            }
        }
    }
}
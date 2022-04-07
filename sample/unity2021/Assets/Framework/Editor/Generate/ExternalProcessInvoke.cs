using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using UnityEngine;

namespace ksc.Service.ToolGen
{
    static class ExternalProcessInvoke
    {
        public static string GetProtocFileName()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    return "res_client/protoc.exe";
                case RuntimePlatform.OSXEditor:
                    return "res_client/protoc";
                default:
                    return string.Empty;
            }
        }
        
        public static bool Proto2csAllRelativeProtoPath(string protocPath, string relativeProtoPath, string importPath,
            string csPath, string workDirectory, string binFile = null)
        {
            string protoFullPath = workDirectory + relativeProtoPath.Replace(".", string.Empty);
            return Proto2csAll(protocPath, protoFullPath, importPath, csPath, workDirectory, null, binFile);
        }
        
        private static string OSSep
        {
            get
            {
#if UNITY_EDITOR_WIN
                return "\\";
#else
                return "/";
#endif
            }
        }

        public static bool Proto2csAll(string protocPath, string protoFullPath, string importPath, string csPath,
            string workDirectory, string[] filters = null, string binFile = null)
        {
            if (!Directory.Exists(protoFullPath))
            {
                UnityEngine.Debug.LogErrorFormat("Directory not exist. [{0}]", protoFullPath);
                return false;
            }

            string[] files = Directory.GetFiles(protoFullPath, "*", SearchOption.AllDirectories);
            StringBuilder fileName = new StringBuilder();
            foreach (var file in files)
            {
                if (!file.EndsWith(".proto", StringComparison.OrdinalIgnoreCase))
                    continue;
                if (filters != null && !CheckFiltersPass(file, filters))
                    continue;
                fileName.Append(file.Replace(protoFullPath + OSSep, "").Replace("\\", "/")).Append(" ");
            }

            return Proto2cs(protocPath, importPath, csPath, binFile, fileName.ToString(), workDirectory);
        }

        private static bool CheckFiltersPass(string file, string[] filters)
        {
            foreach (var filter in filters)
            {
                if (file.ToLower().Contains(filter.ToLower()))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Proto2cs(string protocPath, string protopath, string csPath, string binFile, string proto,
            string workDirectory)
        {
            string[] protoPaths = protopath.Split(';');
            StringBuilder sb = new StringBuilder();
            foreach (var path in protoPaths)
            {
                sb.AppendFormat("--proto_path={0} ", path);
            }

            protopath = sb.ToString();

            string args;
            if (string.IsNullOrEmpty(binFile))
            {
                args = string.Format("{0} --csharp_out={1} {2}", protopath, csPath, proto);
            }
            else
            {
                args = string.Format("{0} --csharp_out={1} {2} -o {3}", protopath, csPath, proto, binFile);
            }
            return InvokeProcess(protocPath, args, workDirectory);
        }
        
        public static bool xls2pb(string tsv2pbPath, string xlsxInput, string tsvPath, string protoPath,
            string storePath, string storeSuffix, string workDirectory)
        {
            string args = (string.Format(
                "--xlsx_input={0} --tsv_path={1} --proto_path={2}  --store_path={3} --store_suffix={4}",
                xlsxInput, tsvPath, protoPath, storePath, storeSuffix));
            return InvokeProcess(tsv2pbPath, args, workDirectory);
        }
        
        public static bool InvokeProcess(string processPath, string args, string workDirectory)
        {
            ProcessStartInfo info = new ProcessStartInfo(processPath, args);
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            info.ErrorDialog = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.WorkingDirectory = workDirectory;
            info.StandardOutputEncoding = Encoding.UTF8;
            info.StandardErrorEncoding = Encoding.UTF8;
            
            Process p = Process.Start(info);
            var output = p.StandardOutput.ReadToEnd();
            var error = p.StandardError.ReadToEnd();
            p.WaitForExit();
            var exitCode = p.ExitCode;
            p.Close();
            if (!string.IsNullOrEmpty(output))
            {
                if (exitCode == 0)
                {
                    UnityEngine.Debug.Log(output);
                }
                else
                {
                    UnityEngine.Debug.LogError(output);
                }
            }
            if (!string.IsNullOrEmpty(error))
            {
                UnityEngine.Debug.Log(args);
                
                if (exitCode == 0)
                {
                    UnityEngine.Debug.LogWarning(error);
                }
                else
                {
                    UnityEngine.Debug.LogError(error);
                }
            }
            
            return exitCode == 0;
        }
    }
}
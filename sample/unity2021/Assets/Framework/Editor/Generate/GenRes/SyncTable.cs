using System.IO;
using ksc.Service.ToolGen;
using UnityEditor;
using UnityEngine;


public class SyncTable
{
    private static readonly string SourceSubPath = "配置/develop";
    private const string ArtResPath = "ArtResPath";
    private static readonly string TargetPath = Path.Combine(Application.dataPath.Replace("Assets", ""), "Tools/ConfigExport");
    
    [MenuItem("Tools/同步表 &A")]
    private static void _SyncTable()
    {
        new SyncTable().OnSyncTable();
    }

    private void OnSyncTable()
    {
        string sourcePath = string.Empty;
        if (SelectPath(ArtResPath, ref sourcePath)) {
            
            // 1.更新svn
            ExternalProcessInvoke.InvokeProcess("svn", "update", sourcePath);
            
            // 2.拷贝表
            UpdateDir(sourcePath);

#if UNITY_EDITOR_WIN
            // 4.导出表
            ExternalProcessInvoke.InvokeProcess("python",  "export.py", TargetPath);
#else
            // 4.导出表
            ExternalProcessInvoke.InvokeProcess("/usr/local/bin/python3",  "export.py", TargetPath); 
#endif
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("提示", "同步表成功", "确定");
        }
    }
    
    private bool SelectPath(string key, ref string path)
    {
        string rootPath = PlayerPrefs.GetString(key);

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
    
    private void UpdateDir(string SourcePath)
    {
        var sourcePath = Path.Combine(SourcePath, SourceSubPath);
        var targetPath = TargetPath;
        if (!Directory.Exists(targetPath)) {
            Directory.CreateDirectory(targetPath);
        }
        IOUtils.CopyFilesRecursively(new DirectoryInfo(sourcePath), new DirectoryInfo(targetPath), true, new []{"*.xls"}, 1);
 }
}

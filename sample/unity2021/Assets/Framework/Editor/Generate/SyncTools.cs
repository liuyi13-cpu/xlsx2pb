using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public static class SyncTools
{
    public static string FolderChoose(string pathKey)
    {
        string folderPath = EditorUtility.OpenFolderPanel("Find File", Application.dataPath, "");
        Debug.Log(folderPath);
        if (string.IsNullOrEmpty(folderPath)) { return null; }
        PlayerPrefs.SetString(pathKey, folderPath);
        PlayerPrefs.Save();
        return folderPath;
    }
    
    public static string GetToolGenPath()
    {
        return Application.dataPath.Replace("/Assets", "/Tools");
    }
    
    public static string GetTablePath()
    {
        return Application.dataPath.Replace("/Assets", "/Tools/res");
    }
}
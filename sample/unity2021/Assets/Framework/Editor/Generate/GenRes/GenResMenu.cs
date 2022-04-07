using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace ksc.Service.ToolGen
{
    public static class GenResMenu
    {
        [MenuItem("KSC.功能/导出表 &z", false, 200)]
        public static void ExportData()
        {
            GenRes.Export(true);
        }
        
        [MenuItem("KSC.功能/生成表加载代码 &x", false, 201)]
        private static void GenerateDBLoadCode()
        {
            GenResCode.GenerateDBLoadCode();
        }
        
        [MenuItem("KSC.功能/一键导出+生成 &c", false, 202)]
        private static void BuildAll()
        {
            GenRes.Export(true);
            if (EditorApplication.isCompiling)
            {
                PlayerPrefs.SetInt("s_BuildAll", 1);
            }
            else
            {
                GenResCode.GenerateDBLoadCode();
            }
        }

        // lua枚举常量需要等C#代码编译完成之后才能正常更新
        [DidReloadScripts]
        private static void OnScriptReloaded()
        {
            if (PlayerPrefs.HasKey("s_BuildAll"))
            {
                GenResCode.GenerateDBLoadCode();
                PlayerPrefs.DeleteKey("s_BuildAll");
            }
        }
    }
}
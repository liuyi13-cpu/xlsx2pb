using System.IO;
using ksc.Service.ToolGen;
using UnityEditor;
using UnityEngine;

public class SyncMsg
{
    private static readonly string TargetPath = Path.Combine(Application.dataPath.Replace("Assets", ""), "Tools/Protobuf");
    
    [MenuItem("Tools/同步消息协议 &S")]
    private static void _SyncMsg()
    {
        new SyncMsg().OnSyncMsg();

        if (GenMsg.GenDataCS())
        {
            GenMsg.Copy2Dest();
        }
        GenMsg.GenLuaConstCodeEnum(Path.Combine(Application.dataPath, "Lua/Data/cs_msg/msg_enum.lua"));
        GenMsg.GenLuaConstCodeIPC(Path.Combine(Application.dataPath, "Lua/Data/cs_msg/InfoPlayerConfig.lua"));
         
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("提示", "同步消息协议成功", "确定");
    }

    private void OnSyncMsg()
    {
#if UNITY_EDITOR_WIN
        ExternalProcessInvoke.InvokeProcess("python",  "generate_msgid.py", TargetPath);
        ExternalProcessInvoke.InvokeProcess("python",  "convert_msgid.py ./messageId.js", TargetPath);
        ExternalProcessInvoke.InvokeProcess("python",  "generate_msghandler.py", TargetPath);
#else
        ExternalProcessInvoke.InvokeProcess("/usr/local/bin/python3",  "generate_msgid.py", TargetPath);
        ExternalProcessInvoke.InvokeProcess("/usr/local/bin/python3",  "convert_msgid.py ./messageId.js", TargetPath);
        ExternalProcessInvoke.InvokeProcess("/usr/local/bin/python3",  "generate_msghandler.py", TargetPath);
#endif
    }
}

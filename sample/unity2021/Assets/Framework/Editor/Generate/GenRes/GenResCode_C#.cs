﻿using System.Text;
using System.IO;

namespace ksc.Service.ToolGen
{
    partial class GenResCode
    {
        StringBuilder mSb = new StringBuilder();
        
        private void GenCS(string saveName, string dbName)
        {
            mSb.Length = 0;
            GenHead(mSb, dbName);
            GenField(mSb);
            GenReleaseFiles(mSb);
            GenRelease(mSb);
            GenReleaseResource(mSb);
            GenLoadCoroutine(mSb);
            GenLoad(mSb);
            for (int i = 0; i < mItems.Count; ++i)
            {
                if (mItems[i].genType == 2)
                {
                    // 仅导出lua
                    continue;
                }
                GenItem(mSb, mItems[i], dbName);
            }

            GenGet(mSb);
            GenTail(mSb);
            File.WriteAllText(saveName, mSb.ToString(), Encoding.UTF8);
        }

        private void GenHead(StringBuilder sb, string dbName)
        {
            sb.AppendFormat(
                @"// This code was generated by a tool.
using RES;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Timing.Data
{{
    public partial class {0}
    {{
", dbName);
        }

        private void GenTail(StringBuilder sb)
        {
            sb.Append(
                @"    }
}");
        }

        private string GetItemKey(string itemType)
        {
            var key = itemType;
            if (key.Contains("RES."))
            {
                key = "int";
            }

            return key;
        }

        private bool IsItemKeyEnum(string itemType)
        {
            return itemType.Contains("RES.");
        }

        private void GenField(StringBuilder sb)
        {
            sb.AppendFormat("\t\tpublic const int TableCount = {0};\n", mItems.Count);
            sb.Append("\t\tpublic int CurCount = 0;\n");
            for (int i = 0; i < mItems.Count; ++i)
            {
                if (mItems[i].genType == 2)
                {
                    // 仅导出lua
                    continue;
                }
                if (mItems[i].types.Count == 1)
                {
                    var key = GetItemKey(mItems[i].types[0].type);
                    sb.AppendFormat("\t\tpublic Dictionary<{0}, {1}> m_{1} = new Dictionary<{0}, {1}>();\n", key,
                        mItems[i]._class);
                }
                else
                {
                    var key1 = GetItemKey(mItems[i].types[0].type);
                    var key2 = GetItemKey(mItems[i].types[1].type);
                    sb.AppendFormat(
                        "\t\tpublic Dictionary<{0}, Dictionary<{1}, {2}>> m_{2} = new Dictionary<{0}, Dictionary<{1}, {2}>>();\n",
                        key1, key2, mItems[i]._class);
                }
            }
        }

        private void GenReleaseResource(StringBuilder sb)
        {
            sb.AppendFormat("\t\tpublic void ReleaseResource(string fullPath)\n");
            sb.AppendFormat("\t\t{{\n");
            sb.AppendFormat("\t\t\tmUnloader(fullPath);\n");
            sb.AppendFormat("\t\t}}\n");
        }

        private void GenGet(StringBuilder sb)
        {
            sb.Append("#region getter\n");
            for (int i = 0; i < mItems.Count; ++i)
            {
                if (mItems[i].genType == 2)
                {
                    // 仅导出lua
                    continue;
                }
                sb.AppendFormat("\t\t// {0}, file:{1}.bytes\n", mItems[i].desc, mItems[i].fullName);
                if (mItems[i].types.Count == 1)
                {
                    sb.AppendFormat("\t\tpublic {0} Get{0}({1} {2}, bool logError = true)\n",
                        mItems[i]._class, mItems[i].types[0].type, mItems[i].types[0].name);
                }
                else
                {
                    sb.AppendFormat("\t\tpublic {0} Get{0}({1} {2}, {3} {4}, bool logError = true)\n",
                        mItems[i]._class, mItems[i].types[0].type, mItems[i].types[0].name, mItems[i].types[1].type,
                        mItems[i].types[1].name);
                }

                sb.Append("\t\t{\n");
                if (mItems[i].types.Count != 1)
                {
                    var key1 = GetItemKey(mItems[i].types[0].type);
                    var key2 = GetItemKey(mItems[i].types[1].type);
                    string typeName1 = mItems[i].types[0].name;
                    if (IsItemKeyEnum(mItems[i].types[0].type))
                    {
                        typeName1 = "(int)" + typeName1;
                    }

                    string typeName2 = mItems[i].types[1].name;
                    if (IsItemKeyEnum(mItems[i].types[1].type))
                    {
                        typeName2 = "(int)" + typeName2;
                    }

                    sb.AppendFormat("\t\t\tm_{0}.TryGetValue({1}, out var data1);\n", mItems[i]._class, typeName1);
                    sb.AppendFormat(@"            if (data1 == null)
            {{
                #if UNITY_EDITOR
                if (logError) UnityEngine.Debug.LogErrorFormat(""{0} MISSING : {{0}}"", {1});
                #endif
                return null; 
            }}
", mItems[i]._class, typeName1);
                    sb.AppendFormat("\t\t\tdata1.TryGetValue({0}, out var data);\n", typeName2);
                    sb.Append("\t\t\t#if UNITY_EDITOR\n");
                    sb.AppendFormat(@"            if (data == null && logError) {{ UnityEngine.Debug.LogErrorFormat(""{0} MISSING : {{0}} {{1}}"", {1}, {2}); }}
", mItems[i]._class, typeName1, typeName2);
                    sb.Append("\t\t\t#endif\n");
                }
                else
                {
                    string typeName = mItems[i].types[0].name;
                    if (IsItemKeyEnum(mItems[i].types[0].type))
                    {
                        typeName = "(int)" + typeName;
                    }

                    sb.AppendFormat("\t\t\tm_{0}.TryGetValue({1}, out var data);\n", mItems[i]._class, typeName);
                    sb.Append("\t\t\t#if UNITY_EDITOR\n");
                    sb.AppendFormat(@"            if (data == null && logError) {{ UnityEngine.Debug.LogErrorFormat(""{0} MISSING : {{0}}"", {1}); }}
", mItems[i]._class, typeName);
                    sb.Append("\t\t\t#endif\n");
                }

                sb.Append("\t\t\treturn data;\n");
                sb.Append("\t\t}\n");
            }

            sb.Append("#endregion\n");
        }

        private void GenReleaseFiles(StringBuilder sb)
        {
            sb.Append("\t\tpublic void ReleaseFiles()\n");
            sb.Append("\t\t{\n");

            sb.Append("\t\t\tint count = mFilePaths.Count;\n");
            sb.Append("\t\t\tfor (int i = 0; i < count; ++i) {\n");
            sb.Append("\t\t\t\tReleaseResource(mFilePaths[i]);\n");
            sb.Append("\t\t\t}\n");
            sb.Append("\t\t\tmFilePaths.Clear();\n");

            sb.Append("\t\t}\n");
        }

        private void GenRelease(StringBuilder sb)
        {
            sb.Append("\t\tpublic void ClearLoadDB()\n");
            sb.Append("\t\t{\n");

            sb.Append("\t\t\tCurCount = 0;\n");
            sb.Append("\t\t\tmFilePaths.Clear();\n");
            for (int i = 0; i < mItems.Count; ++i)
            {
                if (mItems[i].genType == 2)
                {
                    // 仅导出lua
                    continue;
                }
                sb.AppendFormat("\t\t\tm_{0}.Clear();\n", mItems[i]._class);
            }

            sb.Append("\t\t}\n");
        }

        private void GenLoadCoroutine(StringBuilder sb)
        {
            sb.Append("\t\tpublic IEnumerator LoadData_External(Action intervalCallback, Action callback)\n");
            sb.Append("\t\t{\n");
            for (int i = 0; i < mItems.Count; ++i)
            {
                if (mItems[i].genType == 2)
                {
                    // 仅导出lua
                    continue;
                }
                sb.AppendFormat("\t\t\t++CurCount;Load{0}();\n", mItems[i]._class);
                if (i % 5 == 0)
                {
                    sb.Append("\t\t\tintervalCallback?.Invoke();\n");
                    sb.Append("\t\t\tyield return null;\n");
                }
            }

            sb.Append("\t\t\tintervalCallback?.Invoke();\n");
            sb.Append("\t\t\tyield return null;\n");
            sb.Append("\t\t\tcallback?.Invoke();\n");
            sb.Append("\t\t\tReleaseFiles();\n");
            sb.Append("\t\t}\n");
        }

        private void GenLoad(StringBuilder sb)
        {
            sb.Append("\t\tpublic void LoadData_External()\n");
            sb.Append("\t\t{\n");
            for (int i = 0; i < mItems.Count; ++i)
            {
                if (mItems[i].genType == 2)
                {
                    // 仅导出lua
                    continue;
                }
                sb.AppendFormat("\t\t\tLoad{0}();\n", mItems[i]._class);
            }

            sb.Append("\t\t\tReleaseFiles();\n");
            sb.Append("\t\t}\n");
        }

        private void GenItem(StringBuilder sb, DataItem item, string dbName)
        {
            sb.AppendFormat(
                @"        public void Load{1}()
        {{
            string path = $""{{mRelativePath}}{0}"";
            mFilePaths.Add(path);
            byte[] bytes = mLoader(path);
            if (bytes == null)
            {{
                UnityEngine.Debug.LogErrorFormat(""bin data is empty. path :{{0}}"", path);
                return;
            }} 

            {1}Array table;
            table = {1}Array.Parser.ParseFrom(bytes);
            if (table == null) return;
            for (int i = 0; i < table.items.Count; ++i)
            {{
{3}
            }}
        }}
", item.fullName + ".bytes", item._class, item.types[0], GetAddDicString(item));
        }

        private string GetAddDicString(DataItem item)
        {
            if (item.types.Count == 1)
            {
                string typeName = item.types[0].name;
                if (IsItemKeyEnum(item.types[0].type))
                {
                    typeName = "(int)table.items[i]." + typeName;
                }
                else
                {
                    typeName = "table.items[i]." + typeName;
                }

                return string.Format(
                    @"                if (!m_{0}.ContainsKey({1}))
                {{
                    m_{0}.Add({1}, table.items[i]);
                }}
                else
                {{
                    UnityEngine.Debug.LogErrorFormat(""TABLE_{0} already have the same key {{0}}, path {{1}}"", {1}, path);
                }}", item._class, typeName);
            }
            else
            {
                var key1 = GetItemKey(item.types[0].type);
                var key2 = GetItemKey(item.types[1].type);
                string typeName1 = item.types[0].name;
                if (IsItemKeyEnum(item.types[0].type))
                {
                    typeName1 = "(int)table.items[i]." + typeName1;
                }
                else
                {
                    typeName1 = "table.items[i]." + typeName1;
                }

                string typeName2 = item.types[1].name;
                if (IsItemKeyEnum(item.types[1].type))
                {
                    typeName2 = "(int)table.items[i]." + typeName2;
                }
                else
                {
                    typeName2 = "table.items[i]." + typeName2;
                }

                return string.Format(
                    @"          if (!m_{0}.TryGetValue({3}, out var subDic)) {{
                subDic = new Dictionary<{1}, {0}>();
                m_{0}.Add({3}, subDic);
            }}
            if (!subDic.ContainsKey({4})) {{
                subDic.Add({4}, table.items[i]);
            }} else {{
                UnityEngine.Debug.LogErrorFormat(""TABLE_{0} already have the same key {{0}} + {{1}}, path {{2}}"", {3}, {4}, path);
            }}", item._class, key1, key2, typeName1, typeName2);
            }
        }
    }
}
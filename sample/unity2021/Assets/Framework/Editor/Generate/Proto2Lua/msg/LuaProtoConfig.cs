﻿using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MakeLuaProtoConfig
{
    class LuaProtoConfig : Base
    {
        public LuaProtoConfig(string protoFile)
        {
            var dir = Directory.GetCurrentDirectory();
            protoFile = Path.Combine(dir, protoFile);
            var arr = File.ReadAllLines(protoFile);

            foreach (var item in arr)
            {
                if ((item.Contains("=") || item.Contains("//")) && !item.Contains("int32") && !item.Contains("syntax = \"proto3\";"))
                {
                    ParseProto(item);
                }
            }
        }

        protected override void ParseProto(string item)
        {
            item = item.Trim().Replace(";", "");
            item = Regex.Replace(item.Trim(), "\\s+", " ");
            var data = new ProtoData();
            if (item.Contains("//"))
            {
                // 注释
                data.comment = item.Replace("//", "--");
            }
            else if (item.Contains("="))
            {
                // 消息
                var arr = item.Split(' ');
                data.className = arr[0].Replace(".", "_");
                data.memberName = arr[1];
                data.cmdId = arr[3];
            }
            protoList.Add(data);
        }

        /// <summary>
        /// 导出PB_CONFIG
        /// </summary>
        public void MakeConfig(string outFilePath)
        {
            var dir = Directory.GetCurrentDirectory();
            outFilePath = Path.Combine(dir, outFilePath);
            StringBuilder sb = new StringBuilder();
            sb.Append("-- THIS SOURCE CODE WAS AUTO-GENERATED BY TOOL, DO NOT MODIFY IT!!!\n");
            sb.Append("\n");
            
            // 1
            sb.Append("local STR2ID = {\n");
            foreach (var item in protoList)
            {
                if (!string.IsNullOrEmpty(item.comment))
                {
                    sb.Append("\n");
                    sb.Append("    ").Append(item.comment + "\n");
                }
                else if (item.className.Contains("Req") || item.className.Contains("Ntf") || item.className.Contains("Rsp"))
                {
                    sb.Append("    ").Append("['").Append(item.memberName).Append("'] = ").Append(item.cmdId).Append(",\n");
                }
            }
            sb.Append("}\n");

            // 2
            sb.Append("local ID2STR = {\n");
            foreach (var item in protoList)
            {
                if (!string.IsNullOrEmpty(item.comment))
                {
                    sb.Append("\r\n");
                    sb.Append("    ").Append(item.comment + "\r\n");
                }
                else if (item.className.Contains("Rsp") || item.className.Contains("Ntf") || item.className.Contains("Req"))
                {
                    sb.Append("    ").Append("[").Append(item.cmdId).Append("] = '").Append(item.memberName).Append("',\r\n");
                }
            }
            sb.Append("}\r\n");
            sb.Append("return {\r\n");
            sb.Append("    STR2ID = STR2ID,\r\n");
            sb.Append("    ID2STR = ID2STR,\r\n");
            sb.Append("}\r\n");

            File.WriteAllText(outFilePath, sb.ToString());
        }

        /// <summary>
        /// 导出PB_MSG
        /// </summary>
        public void MakeMsg(string outFilePath)
        {
            var dir = Directory.GetCurrentDirectory();
            outFilePath = Path.Combine(dir, outFilePath);
            StringBuilder sb = new StringBuilder();
            sb.Append("-- THIS SOURCE CODE WAS AUTO-GENERATED BY TOOL, DO NOT MODIFY IT!!!\r\n");
            sb.Append("\r\n");
            
            sb.Append("---@class PB_MSG\r\n");
            sb.Append("PB_MSG = {\r\n");
            foreach (var item in protoList)
            {
                if (!string.IsNullOrEmpty(item.comment))
                {
                    sb.Append("\r\n");
                    sb.Append("    ").Append(item.comment + "\r\n");
                }
                else if (item.className.Contains("Req") || item.className.Contains("Ntf") || item.className.EndsWith("Rsp"))
                {
                    sb.Append("    ").Append(item.className).Append(" = '").Append(item.memberName).Append("',\r\n");
                }
            }
            sb.Append("}\r\n");

            File.WriteAllText(outFilePath, sb.ToString());
        }
    }
}
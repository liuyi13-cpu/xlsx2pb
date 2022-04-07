﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MakeLuaProtoConfig
{
    class LuaPlayerInfo : Base
    {
        private Dictionary<string, List<ProtoData>> msg_dic = new Dictionary<string, List<ProtoData>>();
        private List<ProtoData> cur_proto = new List<ProtoData>();
        private bool is_aggregation = false;

        public LuaPlayerInfo(string protoFile)
        {
            var dir = Directory.GetCurrentDirectory();
            protoFile = Path.Combine(dir, protoFile);
            var arr = File.ReadAllLines(protoFile);

            foreach (var item in arr)
            {
                if ((item.Contains("message") || item.Contains("}")) && !item.Contains("//"))
                {
                    AddDicKey(item);
                }
                if ((item.Contains("=") || item.Contains("//")) && !item.Contains("syntax = \"proto3\";") && is_aggregation)
                {
                    ParseProto(item);
                }
            }
        }
        private void AddDicKey(string item)
        {
            item = item.Trim();
            item = Regex.Replace(item.Trim(), "\\s+", " ");
            // 消息名称
            if (item.Contains("message"))
            {
                is_aggregation = true;
                var arr = item.Split(' ');
                msg_dic[arr[1]] = new List<ProtoData>();
                cur_proto = msg_dic[arr[1]];
            }
            else if (item.Contains("}"))
            {
                is_aggregation = false;
            }
        }
        protected override void ParseProto(string item)
        {
            if (item.Contains("map<"))
            {
                string a = ",";
                string b = ";";
                Regex rg = new Regex("(?<=(" + a + "))[.\\s\\S]*?(?=(" + b + "))", RegexOptions.Multiline | RegexOptions.Singleline);
                item = rg.Match(item).Value;
                item = item.Replace(">", "");
            }
            else if (item.Contains("repeated"))
            {
                item = item.Replace("repeated", "");

            }
            NormalProto(item);
        }
        private void NormalProto(string item)
        {
            if (item.Contains("int32") || item.Contains("int64") || item.Contains("bool") || item.Contains("uint64") || item.Contains("oneof") || item.Contains("string"))
            {
                return;
            }
            item = item.Trim().Replace(";", "");
            item = Regex.Replace(item.Trim(), "\\s+", " ");
            var data = new ProtoData();
            if (item.Contains("="))
            {
                // 消息
                var arr = item.Split(' ');
                data.className = arr[0];
                data.memberName = arr[1];
                data.cmdId = arr[3];

                if (arr.Length > 4)
                {
                    data.comment = arr[4].Replace("//", "--");
                    if (arr.Length > 5)
                    {
                        for (int i = 5; i < arr.Length; i++)
                        {
                            data.comment += arr[i];
                        }
                    }
                }
                cur_proto.Add(data);
            }
            //else if (item.Contains("//"))
            //{
            //    // 注释
            //    data.comment = item.Replace("//", "--");
            //}
            //cur_proto.Add(data);
        }
        public void Make(string outFilePath)
        {
            var dir = Directory.GetCurrentDirectory();
            outFilePath = Path.Combine(dir, outFilePath);
            StringBuilder sb = new StringBuilder();
            sb.Append("-- THIS SOURCE CODE WAS AUTO-GENERATED BY TOOL, DO NOT MODIFY IT!!!\n");
            sb.Append("\n");
            
            var player_info = msg_dic["PlayerInfo"];
            // PlayerInfoTag
            PlayerInfoTag(sb, player_info);
            // EventTag
            EventTag(sb, player_info);
            // Tag1
            Tag1(sb, player_info);
            // Tag2
            Tag2(sb, player_info);

            File.WriteAllText(outFilePath, sb.ToString());
        }
        private void PlayerInfoTag(StringBuilder sb, List<ProtoData> player_info)
        {
            sb.Append("---@class PLAYER_INFO_TAG\n");
            sb.Append("PLAYER_INFO_TAG = {\n");
            foreach (var item in player_info)
            {
                if (string.IsNullOrEmpty(item.className))
                {
                    sb.Append("\n");
                    sb.Append("    ").Append(item.comment + "\n");
                }
                else
                {
                    sb.Append("    ").Append("Refresh_").Append(item.memberName).Append(" = ").Append(item.cmdId).Append(" ,");
                    if (!string.IsNullOrEmpty(item.comment))
                    {
                        sb.Append(item.comment + "\n");
                    }
                    else
                    {
                        sb.Append("\n");
                    }
                }
            }
            sb.Append("}\n");
        }
        private void EventTag(StringBuilder sb, List<ProtoData> player_info)
        {
            sb.Append("---@class EVENT_NAME_TAG\n");
            sb.Append("EVENT_NAME_TAG = {\n");
            sb.Append("    ").Append("---TAG1\n");
            foreach (var item in player_info)
            {
                if (string.IsNullOrEmpty(item.className))
                {
                    sb.Append("\n");
                    sb.Append("    ").Append(item.comment + "\n");
                }
                else
                {
                    sb.Append("    ").Append("Refresh_").Append(item.memberName).Append(" = ").Append(" '").Append("Refresh_").Append(item.memberName).Append("',");
                    if (!string.IsNullOrEmpty(item.comment))
                    {
                        sb.Append(item.comment + "\n");
                    }
                    else
                    {
                        sb.Append("\n");
                    }
                }
            }
            sb.Append("\n");
            sb.Append("    ").Append("---TAG2\n");
            foreach (var item in player_info)
            {
                if (!string.IsNullOrEmpty(item.className))
                {
                    if (msg_dic.ContainsKey(item.className))
                    {
                        var info = msg_dic[item.className];
                        foreach (var _item in info)
                        {
                            sb.Append("    ").Append("Refresh_").Append(_item.memberName).Append(" = ").Append(" '").Append("Refresh_").Append(_item.memberName).Append("',");
                            if (!string.IsNullOrEmpty(_item.comment))
                            {
                                sb.Append(_item.comment + "\n");
                            }
                            else
                            {
                                sb.Append("\n");
                            }
                        }
                    }
                }
            }
            sb.Append("}\n");
        }
        private void Tag1(StringBuilder sb, List<ProtoData> player_info)
        {
            sb.Append("---@class TAG1\n");
            sb.Append("TAG1 = {\n");
            foreach (var item in player_info)
            {
                if (string.IsNullOrEmpty(item.className))
                {
                    sb.Append("\n");
                    sb.Append("    ").Append(item.comment + "\n");
                }
                else
                {
                    sb.Append("    ").Append("[").Append(item.cmdId).Append("] = ").Append("{'").Append(item.className).Append("', '").Append(item.memberName).Append("'},");
                    if (!string.IsNullOrEmpty(item.comment))
                    {
                        sb.Append(item.comment + "\n");
                    }
                    else
                    {
                        sb.Append("\n");
                    }
                }
            }
            sb.Append("}\n");
        }
        private void Tag2(StringBuilder sb, List<ProtoData> player_info)
        {
            sb.Append("---@class TAG2\n");
            sb.Append("TAG2 = {\n");
            foreach (var item in player_info)
            {
                if (!string.IsNullOrEmpty(item.className))
                {
                    if (msg_dic.ContainsKey(item.className))
                    {
                        var info = msg_dic[item.className];
                        if (info.Count > 0)
                        {
                            sb.Append("    ").Append("[").Append(item.cmdId).Append("] = ").Append("{\n");
                            foreach (var _item in info)
                            {
                                sb.Append("        ").Append("[").Append(_item.cmdId).Append("] = ").Append("{'").Append(_item.className).Append("', '").Append(_item.memberName).Append("'},");
                                if (!string.IsNullOrEmpty(_item.comment))
                                {
                                    sb.Append(_item.comment + "\n");
                                }
                                else
                                {
                                    sb.Append("\n");
                                }
                            }
                            sb.Append("    },\n");
                        }
                    }
                }
            }
            sb.Append("}\n");
        }
    }
}


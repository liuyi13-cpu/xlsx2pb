using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ksc.Service.ToolGen
{
    partial class GenResCode
    {
        public static void GenerateDBLoadCode()
        {
            var xmlPath = Path.Combine(Application.dataPath, "Framework/Utils/Editor/Generate/config/res_table.xml");
            var codeExportPath = Path.Combine(Application.dataPath, "Scripts/Table/");
            
            var genetor = new GenResCode();
            genetor.LoadXml(xmlPath);
            
            genetor.GenCS(codeExportPath + "Table.Load.cs", "Table");
            Debug.Log("gen table code finish.");

            genetor.GenLua();
            Debug.Log("gen table lua code finish.");
            
            AssetDatabase.Refresh();
        }

        #region XML
        public struct TypeItem
        {
            public string name;
            public string type;
        }

        public struct DataItem
        {
            public string desc;
            public string _class;
            public List<TypeItem> types;
            public string fileName;
            public string fullName;
            public int genType;
        }

        private List<DataItem> mItems = new List<DataItem>();
        
        private void LoadXml(string xmlPath)
        {
            mItems.Clear();
            using (var reader = XmlReader.Create(xmlPath))
            {
                reader.ReadToFollowing("Module");
                do
                {
                    var path = reader.GetAttribute("path");
                    ReadXmlItem(path, reader);
                } while (reader.ReadToNextSibling("Module"));
            }
        }

        private void ReadXmlItem(string path, XmlReader reader)
        {
            reader.ReadToFollowing("item");
            do
            {
                DataItem item = new DataItem();
                item.desc = reader.GetAttribute("desc");
                item._class = reader.GetAttribute("class");
                item.types = new List<TypeItem>();
                string key = reader.GetAttribute("key");
                if (string.IsNullOrEmpty(key))
                {
                    return;
                }

                string[] keys = key.Split(';');
                for (int i = 0; i < keys.Length; ++i)
                {
                    string[] typeItemStr = keys[i].Split(':');
                    if (typeItemStr.Length != 2)
                    {
                        Console.WriteLine(string.Format("invalid key in item :{0}", item._class));
                        continue;
                    }

                    TypeItem typeItem = new TypeItem()
                    {
                        name = typeItemStr[0],
                        type = typeItemStr[1]
                    };
                    item.types.Add(typeItem);
                }

                item.fileName = reader.GetAttribute("filename");
                item.fullName = $"{path}/{item.fileName}";
                item.genType = int.Parse(reader.GetAttribute("type"));
                mItems.Add(item);
            } while (reader.ReadToNextSibling("item"));
        }
        #endregion XML
    }
}
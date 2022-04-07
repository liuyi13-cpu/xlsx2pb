using System;
using System.Collections;
using System.Collections.Generic;
using Timing.Common;

namespace Timing.Data
{
    public partial class Table : Singleton<Table>
    {
        private Func<string, byte[]> mLoader = default;
        private Action<string> mUnloader = default;
        private string mRelativePath = default;
        private List<string> mFilePaths = new List<string>();
        
        public bool Init(Func<string, byte[]> loader, Action<string> unLoader, string relativePath)
        {
            mLoader = loader;
            mUnloader = unLoader;
            mRelativePath = relativePath;
            return true;
        }
        public void Release()
        {
            ReleasePostData();
            ClearLoadDB();
        }
        public IEnumerator LoadData(Action singleLoadedCallback)
        {
            yield return LoadData_External(singleLoadedCallback, null);
            InitPostData();
        }
    }
}

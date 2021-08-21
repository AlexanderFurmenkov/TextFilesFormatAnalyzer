using System.Collections.Generic;

namespace TextFilesFormatAnalizer
{
    interface IFileTask
    {
        public bool Initialize(Options options);
        public string Name();
        public Dictionary<string, string> Execute(string path);
    }
}

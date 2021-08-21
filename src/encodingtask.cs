using System;
using System.Collections.Generic;
using System.IO;

namespace TextFilesFormatAnalizer
{
    class EncodingTask : IFileTask
    {
        public bool Initialize(Options options)
        {
            m_is_initialized = options.Encodings;
            return m_is_initialized;
        }
        public string Name()
        {
            return "Encoding";
        }
        public Dictionary<string, string> Execute(string path)
        {
            Dictionary<string, string> result = new();
            if (!m_is_initialized)
                return result;

            var encoding_with_confidence = GetEncoding(path);
            var encoding = encoding_with_confidence.Item1;
            var confidence = encoding_with_confidence.Item2;

            result.Add("Value", encoding);
            result.Add("Confidence", confidence.ToString().Replace(",", "."));
            return result;
        }

        static Tuple<string, float> GetEncoding(string path_to_file)
        {
            using (FileStream fs = File.OpenRead(path_to_file))
            {
                Ude.CharsetDetector cdet = new();
                cdet.Feed(fs);
                cdet.DataEnd();
                if (cdet.Charset != null)
                {
                    return Tuple.Create(cdet.Charset, cdet.Confidence);
                }
            }
            return Tuple.Create("", (float)0);
        }

        private bool m_is_initialized = false;
    }
}
using System.Collections.Generic;
using System.IO;

namespace TextFilesFormatAnalizer
{
    class LineEndingTask : IFileTask
    {
        public bool Initialize(Options options)
        {
            m_is_initialized = options.LineEndings;
            return m_is_initialized;
        }
        public string Name()
        {
            return "LineEnding";
        }
        public Dictionary<string, string> Execute(string path)
        {
            Dictionary<string, string> result = new();
            if (!m_is_initialized)
                return result;

            var line_ending = GetLineEnding(path);

            result.Add("Value", line_ending);
            return result;
        }

        private static string GetLineEnding(string path_to_file)
        {
            int line = 0;
            int cr = 0;
            int crlf = 0;
            int lf = 0;

            var stream = new FileStream(path_to_file, FileMode.Open, FileAccess.Read);
            using (var br = new BinaryReader(stream))
            {
                var bytes = br.ReadBytes((int)stream.Length);
                for (int i = 0; i < bytes.Length; i++)
                {
                    if (bytes[i] == '\r')
                    {
                        if (i + 1 < bytes.Length &&
                            bytes[i + 1] == '\n')
                        {
                            line++;
                            crlf++;
                            i++;
                        }
                        else
                        {
                            line++;
                            cr++;
                        }
                    }
                    else if (bytes[i] == '\n')
                    {
                        lf++;
                        line++;
                    }
                }
            }

            if (cr == line)
            {
                return "CR";
            }
            else if (crlf == line)
            {
                return "CRLF";
            }
            else if (lf == line)
            {
                return "LF";
            }

            return "MIXED";
        }

        private bool m_is_initialized = false;
    }
}
using CommandLine;

namespace TextFilesFormatAnalizer
{
    public class Options
    {
        [Value(0, MetaName = "folder to scan", Required = true, HelpText = "Folder to scan.")]
        public string Folder { get; set; }

        [Value(1, MetaName = "extensions", Required = true, HelpText = "File extensions in the format \"*.file_extension1|*.file_extension2\".")]
        public string Extensions { get; set; }

        [Value(2, MetaName = "report", Required = true, HelpText = "Report name.")]
        public string Report { get; set; }

        [Option('e', "encodings", Required = false, Default = true, HelpText = "Add encodings to the result report.")]
        public bool Encodings { get; set; }

        [Option('l', "line_endings", Required = false, HelpText = "Add line endings to the result report.")]
        public bool LineEndings { get; set; }

        [Option('s', "statistics", Required = false, HelpText = "Add statistics to the result report.")]
        public bool Statistics { get; set; }
    }
}

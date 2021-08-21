using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace TextFilesFormatAnalizer
{
    class PackedTasks
    {
        public PackedTasks(Options options,
                           List<IFileTask> initialized_tasks)
        {
            m_options = options ?? throw new ArgumentNullException(nameof(options));
            m_initialized_tasks = initialized_tasks ?? throw new ArgumentNullException(nameof(initialized_tasks));
        }

        public void Execute()
        {
            Console.WriteLine("Collecting the files...");

            string extensions = GetExtensions();
            if (string.IsNullOrEmpty(extensions))
            {
                Console.WriteLine("Extensions not found, please specify extensions");
                return;
            }

            string path_to_scan = GetPath();
            if (string.IsNullOrEmpty(path_to_scan) || !Directory.Exists(path_to_scan))
            {
                Console.WriteLine("Path to scan not found, please specify path to directory to scan");
                return;
            }

            var files_stuff = GetFiles(extensions, path_to_scan);
            var all_files = files_stuff.Item1;
            Console.WriteLine("Total files count: " + all_files.Count.ToString());
            if (all_files.Count == 0)
            {
                Console.WriteLine("Files not found...");
                return;
            }

            var extensions_stats = files_stuff.Item2;
            foreach (var extension_record in extensions_stats)
            {
                Console.WriteLine(extension_record.Key + " pattern have " + extension_record.Value + " files");
            }

            Console.WriteLine("Analizing...");
            var report = GetReport(all_files);

            Console.WriteLine("Generating the report...");
            GenerateJsonReport(GetReportPath(), report);

            Console.WriteLine("Finished");
        }

        private static Tuple<ArrayList, Dictionary<string, string>> GetFiles(string extensions, string path_to_scan)
        {
            ArrayList all_files = new();
            Dictionary<string, string> extensions_stats = new();
            string[] arr_ext = extensions.Split('|');
            for (int i = 0; i < arr_ext.Length; i++)
            {
                var files_for_extension = Directory.GetFiles(path_to_scan, arr_ext[i], System.IO.SearchOption.AllDirectories);
                all_files.AddRange(files_for_extension);
                extensions_stats.Add(arr_ext[i], files_for_extension.Length.ToString());
            }

            Tuple<ArrayList, Dictionary<string, string>> result = new(all_files, extensions_stats);

            return result;
        }

        private string GetPath()
        {
            return m_options.Folder;
        }
        private string GetExtensions()
        {
            return m_options.Extensions;
        }
        private string GetReportPath()
        {
            return m_options.Report;
        }
        private Report GetReport(ArrayList files)
        {
            var report = new Report
            {
                FilesRecords = new List<FileRecord>()
            };

            if (m_options.Statistics)
            {
                report.Statistics = new Dictionary<string, Dictionary<string, int>>();
            }

            foreach (var file in files)
            {
                FileRecord file_record = GetFileDataAndCalculateStatisticsIfNeeded(report, file);
                report.FilesRecords.Add(file_record);
            }

            return report;
        }

        private FileRecord GetFileDataAndCalculateStatisticsIfNeeded(Report report, object file)
        {
            var file_record = new FileRecord
            {
                File = file.ToString(),
                Records = new Dictionary<string, Dictionary<string, string>>()
            };
            bool have_statistics = report.Statistics != null;

            foreach (var task in m_initialized_tasks)
            {
                var task_name = task.Name();
                if (have_statistics)
                {
                    if (!report.Statistics.ContainsKey(task_name))
                    {
                        report.Statistics[task_name] = new Dictionary<string, int>();
                    }
                }

                var result = task.Execute(file.ToString());
                foreach (var key_value in result)
                {
                    if (key_value.Key != "Value")
                        continue;

                    if (have_statistics)
                    {
                        if (!report.Statistics[task_name].ContainsKey(key_value.Value))
                        {
                            report.Statistics[task_name].Add(key_value.Value, 1);
                        }
                        else
                        {
                            report.Statistics[task_name][key_value.Value] += 1;
                        }
                    }
                }

                file_record.Records.Add(task_name, result);
            }

            return file_record;
        }

        private static void GenerateJsonReport(string report_path, Report report)
        {
            using StreamWriter json_report = new(report_path);
            string formatted_json = JsonConvert.SerializeObject(report, Formatting.Indented);
            json_report.Write(formatted_json);
        }

        [DataContract]
        private class FileRecord
        {
            [DataMember]
            public string File { get; set; }
            [DataMember]
            public Dictionary<string, Dictionary<string, string>> Records { get; set; }
        }

        [DataContract]
        private class Report
        {
            [DataMember]
            public List<FileRecord> FilesRecords { get; set; }
            [DataMember]
            public Dictionary<string, Dictionary<string, int>> Statistics { get; set; }
        }

        private readonly List<IFileTask> m_initialized_tasks;
        private readonly Options m_options;
    }
}

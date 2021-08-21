using CommandLine;
using System;
using System.Collections.Generic;

namespace TextFilesFormatAnalizer
{
    class TasksProcessor
    {
        public TasksProcessor(string[] args,
                              IFileTask[] tasks)
        {
            m_args = args ?? throw new System.ArgumentNullException(nameof(args));
            m_tasks = tasks ?? throw new System.ArgumentNullException(nameof(tasks));
        }

        public Action GetTask()
        {
            void action()
            {
                Parser.Default.ParseArguments<Options>(m_args).WithParsed<Options>(options =>
                {
                    List<IFileTask> initialized_tasks = new();
                    foreach (var task in m_tasks)
                    {
                        if (task.Initialize(options))
                        {
                            initialized_tasks.Add(task);
                        }
                    }
                    var result = new PackedTasks(options, initialized_tasks);
                    result.Execute();
                });
            }
            return action;
        }

        private readonly string[] m_args;
        private readonly IFileTask[] m_tasks;
    }
}

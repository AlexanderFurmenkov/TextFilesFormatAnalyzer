namespace TextFilesFormatAnalizer
{
    class TFFA
    {
        static void Main(string[] args)
        {
            IFileTask[] registred_tasks = { new EncodingTask(), new LineEndingTask() };
            var task_processor = new TasksProcessor(args, registred_tasks);
            var task = task_processor.GetTask();
            task.Invoke();
        }
    }
}

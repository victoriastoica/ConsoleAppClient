using System.Xml.Linq;

namespace ConsoleAppClient.Models
{
    public class TaskSample
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public DateTime TaskDate { get; set; }
        public string TaskType { get; set; }
        public bool TaskIsProcessed { get; set; }
        public DateTime TaskModifyDate { get; set; }
        public string TaskModifyBy { get; set; }

        public TaskSample(string name, DateTime date, string type, bool isProcessed, DateTime modifyDate, string modifyBy)
        {
            TaskName = name;
            TaskDate = date;
            TaskType = type;
            TaskIsProcessed = isProcessed;
            TaskModifyDate = modifyDate;
            TaskModifyBy = modifyBy;
        }

        public TaskSample(int id, string name, DateTime date, string type, bool isProcessed, DateTime modifyDate, string modifyBy) : this(name, date, type, isProcessed, modifyDate, modifyBy)
        {
            TaskId = id;
        }

        public TaskSample()
        {

        }
    }
}
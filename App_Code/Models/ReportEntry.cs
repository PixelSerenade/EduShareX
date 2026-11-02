using System;

namespace EduX
{
    public class ReportEntry
    {
        public string UserEmail { get; set; }
        public string ReporterName { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

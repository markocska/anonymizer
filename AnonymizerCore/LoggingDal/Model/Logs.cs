using System;
using System.Collections.Generic;

namespace LoggingDal.Model
{
    public partial class Logs
    {
        public int Id { get; set; }
        public string Msg { get; set; }
        public string Template { get; set; }
        public string Severity { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Ex { get; set; }
        public string LogEvent { get; set; }
        public string JobKey { get; set; }
        public string GroupKey { get; set; }
        public string JobDescription { get; set; }
    }
}

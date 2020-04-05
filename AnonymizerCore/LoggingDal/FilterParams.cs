using System;
using System.Collections.Generic;
using System.Text;

namespace LoggingDal
{
    public class FilterParams
    {
        public string GroupKey { get; set; }
        public string JobKey { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}

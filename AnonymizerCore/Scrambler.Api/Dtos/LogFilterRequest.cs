using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrambler.Api.Dtos
{
    public class LogFilterRequest
    {
        public string GroupKey { get; set; }
        public string JobKey { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool IsAscending { get; set; }
        public PaginationParams PaginationParams { get; set; }
    }
}

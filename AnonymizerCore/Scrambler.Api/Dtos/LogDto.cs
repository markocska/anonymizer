using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrambler.Api.Dtos
{
    public class LogDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Severity { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string GroupKey { get; set; }
        public string JobKey { get; set; }
        public string JobDescription { get; set; }
    }
}

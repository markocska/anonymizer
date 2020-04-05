using Scrambler.Api.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrambler.Api.Dtos
{
    public class LogReportResponse
    {
        public IEnumerable<Log> Logs { get; set; }
        public int TotalNumber { get; set; }
    }
}

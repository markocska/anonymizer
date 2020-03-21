using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrambler.Api.Dtos
{
    public class JobDescriptionReportResponse
    {
        public List<JobDescription> JobDescriptions { get; set; }
        public int TotalNumber { get; set; }
    }
}

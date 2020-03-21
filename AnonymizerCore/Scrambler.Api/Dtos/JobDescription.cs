using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrambler.Api.Dtos
{
    public class JobDescription
    {
        public string Id { get; set; }
        public string JobName { get; set; }
        public string JobGroup { get; set; }
        public bool RequestRecovery { get; set; }
        public string Description { get; set; }
        public bool IsDurable { get; set; }
        public List<TriggerDescription> Triggers { get; set; }
    }
}

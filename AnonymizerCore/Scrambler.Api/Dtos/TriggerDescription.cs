using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrambler.Api.Dtos
{
    public class TriggerDescription
    {
        public string TriggerGroup { get; set; }
        public string TriggerName { get; set; }
        public string Description { get; set; }
        public string CalendarName { get; set; }
    }
}

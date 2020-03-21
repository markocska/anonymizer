using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.Quartz.Model
{
    public class TriggerKeyWithDescription
    {
        public string TriggerGroup { get; set; }
        public string TriggerName { get; set; }
        public string Description { get; set; }
        public string CalendarName { get; set; }
        public string CronExpression { get; set; }
    }
}

using Quartz;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.Quartz
{

    public class SchedulingResult
    {
        public bool IsSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        public JobKey JobKey { get; set; }
        public TriggerKey TriggerKey { get; set; }
        public string TriggerDescription {get; set; }
        public string Calendar { get; set; }
        public string CronExpression { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.Quartz.Model
{
    public class JobKeyWithDescription
    {
        public string JobName { get; set; }
        public string JobGroup { get; set; }
        public bool RequestRecovery { get; set; }
        public string Description { get; set; }

    }
}

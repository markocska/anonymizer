using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.Quartz
{
    public class SchedulingResult
    {
        public bool IsSuccessful { get; set; }

        public string ErrorMessage { get; set; }
    }
}

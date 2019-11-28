using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.MySql.SqlScripts
{
    public partial class TurnOnOffIndexes
    {
        public bool Enable { get; set; }

        public TurnOnOffIndexes(bool enable)
        {
            Enable = enable;
        }
    }
}

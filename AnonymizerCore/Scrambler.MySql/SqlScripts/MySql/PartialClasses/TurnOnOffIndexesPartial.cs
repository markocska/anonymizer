using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.MySql.SqlScripts.MySql
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

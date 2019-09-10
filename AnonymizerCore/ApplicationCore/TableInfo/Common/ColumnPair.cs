using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.TableInfo.Common
{
    public struct ColumnPair
    {
        public string FirstColumn { get; }
        public string SecondColumn { get; }

        public ColumnPair(string firstColumn, string secondColumn)
        {
            FirstColumn = firstColumn;
            SecondColumn = secondColumn;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.TableInfo.Common
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

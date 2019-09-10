using System;
using System.Collections.Generic;
using System.Text;

namespace Scrambler.DatabaseServices.ColumnTypes
{
    public class ColumnTypesException : Exception
    {
        public ColumnTypesException(string message): base(message)
        {

        }

        public ColumnTypesException(string message, Exception innerException): base(message, innerException)
        {

        }
    }
}

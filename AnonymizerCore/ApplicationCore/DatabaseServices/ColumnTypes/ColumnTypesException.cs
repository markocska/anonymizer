using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DatabaseServices.ColumnTypes
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

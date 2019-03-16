using System;

namespace Anonymizer.Exceptions
{
    public class TableInfoException : Exception
    {
        public string TableName { get; private set; }
        public string ConnectionString { get; private set; }

        public TableInfoException(string tableName, string connectionString, string message) : base(message)
        {
            TableName = tableName;
            ConnectionString = connectionString;
        }

        public TableInfoException(string tableName, string connectionString, string message, Exception innerException) : base(message, innerException)
        {
            TableName = tableName;
            ConnectionString = connectionString;
        }
    }
}

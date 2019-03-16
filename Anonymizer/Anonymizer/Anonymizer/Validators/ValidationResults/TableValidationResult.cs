using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonymizer.Validators.ValidationResults
{
    public struct TableValidationResult
    {
        public bool IsValid { get; private set; }
        public string ValidationMessage { get; private set; }
        public string TableNameWithSchema { get; private set; }
        public string ConnectionString { get; private set; }

        public TableValidationResult(bool isValid, string validationMessage, string tableNameWithSchema,
            string connectionString)
        {
            IsValid = isValid;
            ValidationMessage = validationMessage;
            TableNameWithSchema = tableNameWithSchema;
            ConnectionString = connectionString;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonymizer.Validators.ValidationResults
{
    public struct ParameterValidationResult
    {
        public bool IsValid { get; private set; }
        public string FullTableName { get; private set; }
        public string ValidationMessage { get; private set; }

        public ParameterValidationResult(bool isValid, string fullTableName, string validationMessage)
        {
            IsValid = isValid;
            FullTableName = fullTableName;
            ValidationMessage = validationMessage;
        }
    }
}

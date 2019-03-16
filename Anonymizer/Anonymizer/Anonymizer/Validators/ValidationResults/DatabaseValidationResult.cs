namespace Anonymizer.Validators.ValidationResults
{
    public struct DatabaseValidationResult
    {
        public bool IsValid { get; private set; }
        public string ValidationMessage { get; private set; }
        public string ConnectionString { get; private set; }
        public DatabaseValidationResult(bool isValid, string validationMessage, string connectionString)
        {
            IsValid = isValid;
            ValidationMessage = validationMessage;
            ConnectionString = connectionString;
        }
    }

}

using ApplicationCore.TableInfo;

namespace ApplicationCore.Validators.ParameterValidators
{
    public interface IParameterValidator
    {
        bool AreParamsValid(ITableInfo tableInfo);
    }
}
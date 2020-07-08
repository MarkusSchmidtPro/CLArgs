using MSPro.CLArgs.ErrorHandling;

namespace MSPro.CLArgs
{
    public interface ICommand2
    {
        ErrorDetailList ValidationErrors { get; }


        void Execute(Arguments arguments, bool throwIf = true);
    }
}
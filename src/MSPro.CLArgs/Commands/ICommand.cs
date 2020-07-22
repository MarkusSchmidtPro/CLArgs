using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs
{
    public interface ICommand
    {
        ErrorDetailList Errors { get; }
        void Execute(Arguments arguments, bool throwIf = true);
    }
}
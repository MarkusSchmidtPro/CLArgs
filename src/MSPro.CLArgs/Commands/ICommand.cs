namespace MSPro.CLArgs
{
    public interface ICommand
    {
        void Execute(Arguments commandParameters, Settings settings);
    }
}
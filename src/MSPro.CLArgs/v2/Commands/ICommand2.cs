using JetBrains.Annotations;



namespace MSPro.CLArgs;

public interface ICommand2
{
    void Execute();

    [NotNull]
    IOptionCollection CommandOptions { get; }
}
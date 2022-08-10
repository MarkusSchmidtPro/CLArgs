using System.Collections.Generic;



namespace MSPro.CLArgs;

/// <summary>
/// </summary>
public interface ICommand
{
    /// <summary>
    /// </summary>
    List<OptionDescriptorAttribute> OptionDescriptors { get; }



    /// <summary>
    /// </summary>
    /// <param name="commandParameters"></param>
    /// <param name="settings"></param>
    void Execute(CommandLineArguments commandParameters, Settings settings);
}   


public interface ICommand2
{
    void Execute();
}
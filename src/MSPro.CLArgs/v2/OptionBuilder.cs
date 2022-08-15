using System;
using System.Collections.Generic;
using JetBrains.Annotations;



namespace MSPro.CLArgs;

[PublicAPI]
public class OptionBuilder
{
    private readonly List<Action<IArgumentCollection>> _configureSourceActions = new();
    private readonly List<Action<IOptionCollection>> _configureTargetActions = new();






    public IOptionCollection Build()
    {
        var arguments = new ArgumentCollection();
        foreach (var build in _configureSourceActions) build(arguments);
        var commandOptions = new OptionCollection();
        foreach (var build in _configureTargetActions) build(commandOptions);
        return commandOptions;
    }



    #region Configure

    public void ConfigureSource(Action<IArgumentCollection> action)
    {
        _configureSourceActions.Add(action);
    }



    public void ConfigureTarget(Action<IOptionCollection> action)
    {
        _configureTargetActions.Add(action);
    }

    #endregion
}
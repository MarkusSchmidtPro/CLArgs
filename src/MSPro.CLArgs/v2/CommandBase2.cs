using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using MSPro.CLArgs;



public abstract class CommandBase2<TContext> : ICommand2
{
    private readonly ContextBuilder _contextBuilder;



    protected CommandBase2(IServiceProvider serviceProvider)
    {
        _contextBuilder = serviceProvider.GetRequiredService<ContextBuilder>();
    }



    protected virtual TContext Context { get; private set; }



    void ICommand2.Execute()
    {
        this.Context = _contextBuilder.Build<TContext>(
            out HashSet<string> unresolvedPropertyNames,
            out ErrorDetailList errors);

        BeforeExecute(unresolvedPropertyNames, errors);
        if (errors.HasErrors()) { }

        Execute();
    }



    protected virtual void BeforeExecute(
        HashSet<string> unresolvedPropertyNames,
        ErrorDetailList errors)
    {
    }



    protected abstract void Execute();
}
﻿using System;
using Microsoft.Extensions.DependencyInjection;



namespace MSPro.CLArgs;

public abstract class CommandWithContext(Type contextType)
{
    private ContextPropertyCollection? _contextProperties;


    /// <summary>
    ///     Get all annotated Context properties.
    /// </summary>
    public ContextPropertyCollection ContextProperties
        => _contextProperties ??= ContextPropertyCollection.FromType(contextType);
}



public abstract class CommandBase2<TContext>(IServiceProvider serviceProvider) : CommandWithContext(typeof(TContext)), ICommand2
    where TContext : class
{
    //protected readonly IPrinter Print = serviceProvider.GetRequiredService<IPrinter>();
    protected readonly IServiceProvider ServiceProvider = serviceProvider;


    protected TContext _context { get; private set; } = null!;



    void ICommand2.Execute()
    {
        ErrorDetailList errors = new();
        var arguments = ServiceProvider.GetRequiredService<IArgumentCollection>();
        var contextBuilder = ServiceProvider.GetRequiredService<ContextBuilder>();
        _context = contextBuilder.Build<TContext>(arguments, ContextProperties, errors);

        if (!errors.HasErrors())
        {
            BeforeExecute(errors);
            if (!errors.HasErrors())
            {
                try
                {
                    Execute();
                }
                catch (Exception exception)
                {
                    errors.AddException(exception);
                }
            }
        }

        AfterExecute(errors);
    }



    protected virtual void AfterExecute(ErrorDetailList errors)
    {
    }



    protected abstract void Execute();



    protected virtual void BeforeExecute(ErrorDetailList errors)
    {
    }
}
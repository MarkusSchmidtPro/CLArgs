using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;



namespace MSPro.CLArgs;


[PublicAPI]
public abstract class CommandWithContext
{
    private readonly Type _contextType;
    private ContextPropertyCollection _contextProperties;



    protected CommandWithContext(Type contextType)
    {
        _contextType      = contextType;
    }



    /// <summary>
    /// Get all annotated Context properties.
    /// </summary>
    public ContextPropertyCollection ContextProperties 
        => _contextProperties ??= ContextPropertyCollection.FromType(_contextType);
}




[PublicAPI]
public abstract class CommandBase2<TContext> : CommandWithContext, ICommand2 where TContext : class
{
    protected readonly IPrinter Print;
    protected readonly IServiceProvider ServiceProvider;

    
    protected CommandBase2(IServiceProvider serviceProvider) : base(typeof(TContext))
    {
        ServiceProvider = serviceProvider;
        Print           = serviceProvider.GetRequiredService<IPrinter>();
    }

    protected TContext Context { get; private set; }


    void ICommand2.Execute()
    {
        ErrorDetailList errors = new();

        var arguments = ServiceProvider.GetRequiredService<IArgumentCollection>();
        
        ContextBuilder contextBuilder = ServiceProvider.GetRequiredService<ContextBuilder>();
        Context = contextBuilder.Build<TContext>( arguments, ContextProperties, errors);

        if (!errors.HasErrors())
        {
            BeforeExecute( errors);
            if (!errors.HasErrors())
            {
                try
                {
                    Execute();
                }
                catch (Exception exception)
                {
                    errors.AddError("CommandExecution", exception.Message);
                }
            }
        }

        if (errors.HasErrors()) OnError(errors, false);
    }



    protected abstract void Execute();



    /// <summary>
    ///     Error handler in case of any error.
    /// </summary>
    /// <remarks>
    ///     The default implementation of <see cref="OnError" /> simply throws an
    ///     <see cref="AggregateException" /> in case of any error. You can avoid this by overriding this method.
    /// </remarks>
    /// <param name="errors">The errors that have occurred.</param>
    /// <param name="handled">If <c>true</c> the method does nothing anymore, because it expects the errors have been handled.</param>
    /// <exception cref="AggregateException">Always</exception>
    protected virtual void OnError(ErrorDetailList errors, bool handled)
    {
        if (handled) return;
        if (errors.Details.Count > 1)
            throw new AggregateException(errors.Details.Select(
                                             e => new ArgumentException(e.ErrorMessages[0], e.AttributeName)));

        throw new ArgumentException(errors.Details[0].ErrorMessages[0], errors.Details[0].AttributeName);
    }



    protected virtual void BeforeExecute(ErrorDetailList errors)
    {
    }
}
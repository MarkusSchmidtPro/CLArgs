using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;



namespace MSPro.CLArgs;

[PublicAPI]
public abstract class CommandWithContext(Type contextType)
{
    private ContextPropertyCollection _contextProperties;


    /// <summary>
    ///     Get all annotated Context properties.
    /// </summary>
    public ContextPropertyCollection ContextProperties
        => _contextProperties ??= ContextPropertyCollection.FromType(contextType);
}



[PublicAPI]
public abstract class CommandBase2<TContext>(IServiceProvider serviceProvider) : CommandWithContext(typeof(TContext)), ICommand2
    where TContext : class
{
    protected readonly IPrinter Print = serviceProvider.GetRequiredService<IPrinter>();
    protected readonly IServiceProvider ServiceProvider = serviceProvider;


    protected TContext Context { get; private set; }



    void ICommand2.Execute()
    {
        ErrorDetailList errors = new();

        IArgumentCollection arguments = ServiceProvider.GetRequiredService<IArgumentCollection>();

        ContextBuilder contextBuilder = ServiceProvider.GetRequiredService<ContextBuilder>();
        Context = contextBuilder.Build<TContext>(arguments, ContextProperties, errors);

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
                    errors.AddError("CommandExecution", exception.Message);
                }
            }
        }

        AfterExecute(errors);
    }



    protected virtual void AfterExecute(ErrorDetailList errors)
    {
        switch (errors.Details.Count)
        {
            case > 1:
                throw new AggregateException(errors.Details.Select(
                    e => new ArgumentException(e.ErrorMessages[0], e.AttributeName)));
            case > 0:
                throw new ArgumentException(errors.Details[0].ErrorMessages[0], errors.Details[0].AttributeName);
        }
    }



    protected abstract void Execute();



    protected virtual void BeforeExecute(ErrorDetailList errors)
    {
    }
}
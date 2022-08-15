using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace MSPro.CLArgs;

/// <summary>
///     Provides a convenient way to use command-line for most applications.
/// </summary>
/// <typeparam name="TContext">
///     The type of the parameter object that is passed to the command.
/// </typeparam>
[PublicAPI]
public abstract class CommandBase<TContext> : ICommand where TContext : class, new()
{
    /// <summary>
    ///     The settings instance as provided by Console.Main().
    /// </summary>
    protected Settings Settings { get; private set; }

    /// <summary>
    ///     Get the execution context as it is provided to the Execute method.
    /// </summary>
    protected TContext ExecutionContext { get; private set; }



    /// <summary>
    ///     Execute the command.
    /// </summary>
    /// <param name="commandLineArguments"></param>
    /// <param name="settings"></param>
    public void Execute(
        [NotNull] CommandLineArguments commandLineArguments, 
        [CanBeNull] Settings settings = null)
    {
        this.Settings = settings ?? new Settings();
        BeforeArgumentConversion(commandLineArguments);
        ArgumentConverter<TContext> c = new(this.Settings);

        // Convert command-line arguments and create the execution context
        var errors = c.TryConvert(commandLineArguments, this.OptionDescriptors,
            out var executionContext,
            out var unresolvedPropertyNames);

        this.ExecutionContext = executionContext;

        if (!errors.HasErrors())
        {
            BeforeExecute(this.ExecutionContext, unresolvedPropertyNames, errors);
            if (!errors.HasErrors())
            {
                try
                {
                    Execute(this.ExecutionContext);
                }
                catch (Exception exception)
                {
                    errors.AddError("CommandExecution", exception.Message);
                }
            }
        }

        if (errors.HasErrors()) OnError(errors, false);
    }


    /// <summary>
    /// Get a list of all OptionDescriptorAttributes.
    /// </summary>
    public List<OptionDescriptorAttribute> OptionDescriptors
    {
        get
        {
            var provider = new OptionDescriptorFromTypeProvider<TContext>();
            return provider.Get().ToList();
        }
    }



    /// <summary>
    /// BeforeArgumentConversion
    /// </summary>
    /// <remarks>
    ///     This method is called before any Argument conversion takes place.
    ///     Override this method to add your custom Argument to Property
    ///     <see cref="ValueConverters">TypeConverters</see>.<br />
    /// </remarks>
    protected virtual void BeforeArgumentConversion([NotNull] CommandLineArguments clArgs)
    {
    }



    /// <summary>
    ///     Make sure everything is set-up and ready to execute the command.
    /// </summary>
    /// <remarks>
    ///     Use this method to validate <see paramref="parameters" />,
    ///     to provide provide dynamic defaults and/or to resolve parameter.<br />
    ///     The method s called immediately before the Command <see cref="Execute(TContext)" /> method is called.
    ///     In case, <paramref name="errors" /> contains any value, <see cref="OnError" /> is called instead of
    ///     <see cref="Execute(TContext)" />.
    /// </remarks>
    /// <param name="parameters">
    ///     The parameter object (target instance) that will be used to execute the Command.
    /// </param>
    /// <param name="unresolvedPropertyNames">
    ///     A <see cref="HashSet{T}" /> containing the names of those properties in the CommandContext
    ///     which haven't got a value, neither by assigning a command-line option
    ///     nor by a default value defined in the <c>CommandContext</c>.
    ///     Such properties will have their C# defaults, like <c>false</c> for boolean properties.<br />
    ///     If you want to check for unresolved properties is is best practice to use <c>nameof()</c>
    ///     instead of plain string: <code>if (unresolvedPropertyNames.Contains(nameof(CommandContext.CSV))) ...</code>
    /// </param>
    /// <param name="errors">
    ///     The error object. In case of any error, use <see cref="ErrorDetailList.AddError(string,string)" />
    ///     to add your errors to this list.
    /// </param>
    /// <seealso cref="OnError" />
    protected virtual void BeforeExecute(
        TContext parameters,
        HashSet<string> unresolvedPropertyNames,
        ErrorDetailList errors)
    {
    }



    protected abstract void Execute(TContext ps);



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
        if( errors.Details.Count>1)
            throw new AggregateException(errors.Details.Select(
                e => new ArgumentException(e.ErrorMessages[0], e.AttributeName)));

        throw new ArgumentException(errors.Details[0].ErrorMessages[0], errors.Details[0].AttributeName);
    }
}
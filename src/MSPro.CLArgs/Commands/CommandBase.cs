using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    /// <summary>
    ///     Provides a convenient way to use command-line for most applications.
    /// </summary>
    /// <typeparam name="TCommandParameters">
    ///     The type of the parameter object that is passed to the command.
    /// </typeparam>
    [PublicAPI]
    public abstract class CommandBase<TCommandParameters> : ICommand where TCommandParameters : class, new()
    {
        public void Execute([NotNull] Arguments arguments, [CanBeNull] Settings settings = null)
        {
            settings ??= new Settings();
            BeforeArgumentConversion(arguments, settings);
            ArgumentConverter<TCommandParameters> c = new ArgumentConverter<TCommandParameters>(settings);
            var errors = c.TryConvert(arguments,
                                      out var commandParameters,
                                      out var unresolvedPropertyNames);
            if (!errors.HasErrors())
            {
                BeforeExecute(commandParameters, unresolvedPropertyNames, errors);
                if (!errors.HasErrors()) Execute(commandParameters);
            }

            if (errors.HasErrors()) OnError(errors, false);
        }



        /// <summary>
        /// </summary>
        /// <remarks>
        ///     This method is called before any Argument conversion takes place.
        ///     Override this method to add your custom Argument to Property
        ///     <see cref="Settings.ValueConverters">TypeConverters</see>.<br />
        ///     You may also use this method to
        ///     <see cref="Arguments.SetOption(MSPro.CLArgs.Option)">add missing options</see>
        ///     to your arguments.
        /// </remarks>
        protected virtual void BeforeArgumentConversion( [NotNull] Arguments arguments, [NotNull] Settings settings )
        {
        }



        /// <summary>
        ///     Make sure everything is set-up and ready to execute the command.
        /// </summary>
        /// <remarks>
        ///     Use this method to validate <see cref="parameters" />,
        ///     to provide provide dynamic defaults and/or to resolve parameter.<br />
        ///     The method s called immediately before the Command <see cref="Execute(TCommandParameters)" /> method is called.
        ///     In case, <paramref name="errors" /> contains any value, <see cref="OnError" /> is called instead of
        ///     <see cref="Execute(TCommandParameters)" />.
        /// </remarks>
        /// <param name="parameters">
        ///     The parameter object (target instance) that will be used to execute the Command.
        /// </param>
        /// <param name="unresolvedPropertyNames">
        ///     A <see cref="HashSet{T}" /> containing those parameter properties that haven't yet got a value: neither by assigning a
        ///     command-line option nor was there a default value defined in the properties
        ///     <see cref="OptionDescriptorAttribute" />.
        /// </param>
        /// <param name="errors">
        ///     The error object. In case of any error, use <see cref="ErrorDetailList.AddError(string,string)" />
        ///     to add your errors to this list.
        /// </param>
        /// <seealso cref="OnError" />
        protected virtual void BeforeExecute(
            TCommandParameters parameters,
            HashSet<string> unresolvedPropertyNames,
            ErrorDetailList errors)
        {
        }



        protected abstract void Execute(TCommandParameters ps);



        /// <summary>
        ///     Error handler in case of any error.
        ///     <remarks>
        ///         The default implementation of <see cref="OnError" /> simply throws an
        ///         <see cref="AggregateException" /> in case of any error. You can avoid this by overriding this method.
        ///     </remarks>
        /// </summary>
        /// <param name="errors">The errors that have occured.</param>
        /// <param name="handled">If <c>true</c> the method does nothing anymore, because it expects the errors have been handled.</param>
        /// <exception cref="AggregateException">Always</exception>
        protected virtual void OnError(ErrorDetailList errors, bool handled)
        {
            if (handled) return;
            throw new AggregateException(errors.Details.Select(
                                             e => new ArgumentException(e.ErrorMessages[0], e.AttributeName)));
        }
    }
}
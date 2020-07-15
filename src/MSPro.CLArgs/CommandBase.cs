using System;
using System.Collections.Generic;
using System.Linq;
using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs
{
    /// <summary>
    ///     Provides a convenient way to use command-line for most applications.
    /// </summary>
    /// <typeparam name="TCommandParameters">
    ///     The type of the parameter object that is passed to the command.
    /// </typeparam>
    public abstract class CommandBase<TCommandParameters> : ICommand where TCommandParameters : class, new()
    {
        protected abstract void OnExecute(TCommandParameters commandParameters);



        protected virtual void OnResolveProperties(TCommandParameters commandParameters,
                                                   List<string> unresolvedPropertyNames)
        {
        }



        #region ICommand

        public ErrorDetailList Errors { get; } = new ErrorDetailList();



        void ICommand.Execute(Arguments arguments, bool throwIf)
        {
            var options = CommandLineOptions.FromArguments<TCommandParameters>(arguments);

            this.Errors.Add(options.Errors);
            if (!this.Errors.HasErrors())
            {
                // Convert all known options
                CommandLineOptionsConverter converter = new CommandLineOptionsConverter();
                var commandParameters =
                    converter.ToCommandParameters<TCommandParameters>(options, out List<string> unresolvedProperties);

                this.Errors.Add(converter.Errors);
                if (!this.Errors.HasErrors())
                {
                    OnResolveProperties(commandParameters, unresolvedProperties);
                    if (!this.Errors.HasErrors())
                    {
                        OnExecute(commandParameters);
                    }
                }
            }

            if (!this.Errors.HasErrors() || !throwIf) return;

            throw new AggregateException(this.Errors.Details.Select(
                                             e => new ArgumentException(e.ErrorMessages[0], e.AttributeName)));
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs
{
    public abstract class CommandBase2<TCommandOptions> : ICommand2 where TCommandOptions : class, new()
    {
        protected abstract void OnExecute(TCommandOptions parameters);
        protected virtual void OnResolveOptions(IEnumerable<string> unresolvedOptionNames, TCommandOptions targetInstance, ErrorDetailList errors){}



        #region ICommand2

        public ErrorDetailList Errors { get; } = new ErrorDetailList();



        void ICommand2.Execute(Arguments arguments, bool throwIf)
        {
            var options = CommandLineOptions.FromArguments<TCommandOptions>(arguments);
            
            this.Errors.Add(options.Errors);
            if (!this.Errors.HasErrors())
            {
                CommandLineOptionsConverter converter = new CommandLineOptionsConverter();
                var commandOptions = converter.ToObject<TCommandOptions>(options, OnResolveOptions);
                
                this.Errors.Add(converter.Errors);
                if (!this.Errors.HasErrors())
                {
                    OnExecute(commandOptions);
                }
            }

            if (!this.Errors.HasErrors() || !throwIf) return;

            throw new AggregateException(this.Errors.Details.Select(
                e => new ArgumentException(e.ErrorMessages[0], e.AttributeName)));
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MSPro.CLArgs.ErrorHandling;



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
        public void Execute(Arguments arguments, Settings settings=null)
        {
            if (settings != null) settings = new Settings();
            
            ArgumentConverter<TCommandParameters> c
                = new ArgumentConverter<TCommandParameters>(settings);
            var errors= c.TryConvert(arguments, 
                                     out var commandParameters,
                out var unresolvedPropertyNames);
            if (!errors.HasErrors())
            {
                if( unresolvedPropertyNames.Count>0)
                    OnResolveProperties(commandParameters,unresolvedPropertyNames);
                
                OnExecute( commandParameters);
            }
            else OnError(errors);
        }



        protected virtual void OnError(ErrorDetailList errors)
        {
            throw new AggregateException(errors.Details.Select(
                                             e => new ArgumentException(e.ErrorMessages[0], e.AttributeName)));

        }

        protected virtual void OnResolveProperties(
            TCommandParameters commandParameters,
            HashSet<string> unresolvedPropertyNames)
        {
        }



        protected abstract void OnExecute(TCommandParameters commandParameters);
    }
}
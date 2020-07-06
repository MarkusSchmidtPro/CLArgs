using System;
using System.Collections.Generic;
using System.Linq;
using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs
{
    public abstract class CommandBase : ICommand
    {
        protected CommandBase(Arguments arguments)
        {
            this.Arguments = arguments;
            this.ValidationErrors = new ErrorDetailList();
        }

        
        protected int ToInt(string optionName)
        {
            if (!Arguments.Options.ContainsKey(optionName)) return 0;

            if (!int.TryParse(Arguments.Options[optionName].Value, out int v))
                ValidationErrors.AddError(optionName,
                    $"Cannot parse the value '{Arguments.Options[optionName].Value}' for Option '{optionName}' into an integer.");
            return v;
        }    
        
        
        protected bool ToBool(string optionName)
        {
            if (!Arguments.Options.ContainsKey(optionName)) return false;

            if (!bool.TryParse(Arguments.Options[optionName].Value, out bool boolValue))
            {
                // boolean conversion failed, try int conversion on <>0
                if (int.TryParse(Arguments.Options[optionName].Value, out int intValue))
                {
                    // int conversion possible 
                    boolValue = intValue != 0;
                }
                else
                {
                    ValidationErrors.AddError(optionName,
                        $"Cannot parse the value '{Arguments.Options[optionName].Value}' for Option '{optionName}' into an boolean.");
                }
            }

            return boolValue;
        }

        public Arguments Arguments { get; }
        public ErrorDetailList ValidationErrors { get; }



        /// <inheritdoc cref="ICommand.ValidateAndParseArguments" />
        bool ICommand.ValidateAndParseArguments(bool throwIf)
        {
            OnValidate();
            if (throwIf && this.ValidationErrors.HasErrors())
                throw new AggregateException(this.ValidationErrors.Details.Select(
                    e => new ArgumentException(e.ErrorMessages[0], e.AttributeName)));

            return this.ValidationErrors.HasErrors();
        }



        void ICommand.Execute() => OnExecute();




        // Ease of use for one argument name per argument.
        protected bool CheckMandatory(params string[] mandatoryArgumentNames) 
            => CheckMandatory( mandatoryArgumentNames.Select(
                mandatoryArgumentName => new string[] {mandatoryArgumentName}).ToArray());



        protected bool CheckMandatory(string[][] mandatoryArgumentNames)
        {
            // each argument can have any number of tags
            // n Arguments with m tags
            foreach (string[] tags in mandatoryArgumentNames)
            {
                string argumentName = string.Join(",", tags);

                if (!tags.Any(tag => this.Arguments.Options.ContainsKey(tag)))
                    this.ValidationErrors.AddError(argumentName, $"The mandatory command-line argument '{argumentName}' was not provided.");
            }
            return !this.ValidationErrors.HasErrors();
        }



        /// <summary>
        ///     Validate and probably convert all command-lines arguments.
        /// </summary>
        /// <remarks>
        ///     Make use of the <see cref="ValidationErrors" /> list
        ///     in case of any validation error.
        /// </remarks>
        protected virtual void OnValidate()
        {
        }



        protected abstract void OnExecute();
    }
}
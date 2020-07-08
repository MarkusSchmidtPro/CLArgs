using System;
using System.Linq;
using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs
{
    public abstract class CommandBase2 : ICommand2
    {
        protected CommandBase2()
        {
            this.ValidationErrors = new ErrorDetailList();
        }



        public Arguments Arguments { get; }
        public ErrorDetailList ValidationErrors { get; }



        void ICommand2.Execute(Arguments arguments, bool throwIf)
        {
            OnValidate();
            if (!this.ValidationErrors.HasErrors())
            {
                OnMap();
                if (!this.ValidationErrors.HasErrors()) OnExecute();
            }

            if (!this.ValidationErrors.HasErrors() || !throwIf) return;

            throw new AggregateException(this.ValidationErrors.Details.Select(
                e => new ArgumentException(e.ErrorMessages[0], e.AttributeName)));
        }



        protected int ToInt(string optionName)
        {
            if (!this.Arguments.Options.ContainsKey(optionName)) return 0;

            if (!int.TryParse(this.Arguments.Options[optionName].Value, out int v))
                this.ValidationErrors.AddError(optionName,
                    $"Cannot parse the value '{this.Arguments.Options[optionName].Value}' for Option '{optionName}' into an integer.");
            return v;
        }



        protected bool ToBool(string optionName)
        {
            if (!this.Arguments.Options.ContainsKey(optionName)) return false;

            if (!bool.TryParse(this.Arguments.Options[optionName].Value, out bool boolValue))
            {
                // boolean conversion failed, try int conversion on <>0
                if (int.TryParse(this.Arguments.Options[optionName].Value, out int intValue))
                {
                    // int conversion possible 
                    boolValue = intValue != 0;
                }
                else
                {
                    this.ValidationErrors.AddError(optionName,
                        $"Cannot parse the value '{this.Arguments.Options[optionName].Value}' for Option '{optionName}' into an boolean.");
                }
            }

            return boolValue;
        }



        protected virtual void OnMap()
        {
        }



        // Ease of use for one argument name per argument.
        protected bool CheckMandatory(params string[] mandatoryArgumentNames)
            => CheckMandatory(mandatoryArgumentNames.Select(
                mandatoryArgumentName => new[] {mandatoryArgumentName}).ToArray());



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
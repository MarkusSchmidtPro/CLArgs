using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MSPro.CLArgs.ErrorHandling;

namespace MSPro.CLArgs
{
    public abstract class CommandBase2<TParameters> : ICommand2 where TParameters : class, new()
    {
        private readonly Dictionary<Type, Func<Arguments, string, string, object>> _argumentMapperFunctions;

        protected CommandBase2()
        {
            _argumentMapperFunctions = new Dictionary<Type, Func<Arguments, string, string, object>>
            {
                {typeof(string), toString},
                {typeof(int), toInt},
                {typeof(bool), toBool}
            };
        }

        /// <summary>
        ///     A dictionary containing all command-line properties.
        /// </summary>
        /// <remarks>
        ///     A command-line property is decorated with the <see cref="CommandLineOptionAttribute" />.
        ///     The <c>key</c> contains the <see cref="PropertyInfo" />.
        /// </remarks>
        public Dictionary<PropertyInfo, CommandLineOptionAttribute> OptionProperties { get; set; }


        /// <summary>
        ///     Check if all mandatory parameters are there.
        /// </summary>
        /// <param name="arguments"></param>
        private void checkMandatory(Arguments arguments)
        {
            var mandatoryArgumentNames = OptionProperties.Values
                .Where(od => od.Mandatory)
                .Select(od => od.Tags);

            // each argument can have any number of tags
            // n Arguments with m tags
            foreach (var tags in mandatoryArgumentNames)
            {
                var argumentName = string.Join(",", tags);

                if (!tags.Any(tag => arguments.Options.ContainsKey(tag)))
                    ValidationErrors.AddError(argumentName,
                        $"The mandatory command-line argument '{argumentName}' was not provided.");
            }
        }


        /// <summary>
        ///     Validate and probably convert all command-lines arguments.
        /// </summary>
        /// <remarks>
        ///     Make use of the <see cref="ValidationErrors" /> list
        ///     in case of any validation error.
        /// </remarks>
        protected virtual bool OnValidate(Arguments arguments)
        {
            if (OptionProperties == null)
                // Get Option descriptors from class (attribute) definition.
                OptionProperties = CustomAttributes.getSingle<CommandLineOptionAttribute>(typeof(TParameters));

            // populate non mandatory with default values
            var optionalArguments = OptionProperties.Values
                .Where(od => !od.Mandatory);

            // each argument can have any number of tags
            // n Arguments with m tags
            foreach (var optionAttribute in optionalArguments)
            {
                var argumentName = string.Join(",", optionAttribute);

                if (!optionAttribute.Tags.Any(tag => arguments.Options.ContainsKey(tag)))
                {
                    // not provided
                    arguments.AddOption( new Option( optionAttribute.Tags[0], optionAttribute.Default ));
                }
            }

            checkMandatory(arguments);
            return !ValidationErrors.HasErrors();
        }


        protected virtual TParameters OnMap(Arguments arguments)
        {
            var result = new TParameters();
            foreach (var commandLineProperty in OptionProperties)
            {
                var pi = commandLineProperty.Key;
                var att = commandLineProperty.Value;
                var argumentName = att.Name;

                if (!_argumentMapperFunctions.ContainsKey(pi.PropertyType))
                {
                    ValidationErrors.AddError(argumentName,
                        $"No mapper found fpr type {pi.PropertyType} of property {pi.Name} ");
                    continue;
                }

                var mapFunc = _argumentMapperFunctions[pi.PropertyType];
                pi.SetValue(result, mapFunc(arguments, argumentName, att.Default));
            }

            return result;
        }


        protected abstract void OnExecute(TParameters parameters);

        #region Default Mappers

        private object toInt(Arguments arguments, string optionName, string defaultValue)
        {
            if (!arguments.Options.ContainsKey(optionName)) return 0;

            if (!int.TryParse(arguments.Options[optionName].Value, out var v))
                ValidationErrors.AddError(optionName,
                    $"Cannot parse the value '{arguments.Options[optionName].Value}' for Option '{optionName}' into an integer.");
            return v;
        }


        private object toString(Arguments arguments, string optionName, string defaultValue)
        {
            if (!arguments.Options.ContainsKey(optionName)) return null;
            return arguments.Options[optionName].Value;
        }


        private object toBool(Arguments arguments, string optionName, string defaultValue)
        {
            if (!arguments.Options.ContainsKey(optionName)) return false;

            if (!bool.TryParse(arguments.Options[optionName].Value, out var boolValue))
            {
                // boolean conversion failed, try int conversion on <>0
                if (int.TryParse(arguments.Options[optionName].Value, out var intValue))
                    // int conversion possible 
                    boolValue = intValue != 0;
                else
                    ValidationErrors.AddError(optionName,
                        $"Cannot parse the value '{arguments.Options[optionName].Value}' for Option '{optionName}' into an boolean.");
            }

            return boolValue;
        }

        #endregion


        #region ICommand2

        public ErrorDetailList ValidationErrors { get; } = new ErrorDetailList();


        void ICommand2.Execute(Arguments arguments, bool throwIf)
        {
            if (OnValidate(arguments))
            {
                var parameters = OnMap(arguments);
                if (!ValidationErrors.HasErrors()) OnExecute(parameters);
            }

            if (!ValidationErrors.HasErrors() || !throwIf) return;

            throw new AggregateException(ValidationErrors.Details.Select(
                e => new ArgumentException(e.ErrorMessages[0], e.AttributeName)));
        }

        #endregion
    }
}
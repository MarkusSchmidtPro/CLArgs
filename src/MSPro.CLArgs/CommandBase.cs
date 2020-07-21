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
        protected abstract void OnExecute(TCommandParameters commandParameters);



        protected virtual void OnResolveProperties(TCommandParameters ps,
                                                   List<string> unresolvedPropertyNames)
        {
        }



        public TypeConverters TypeConverters { get; } = new TypeConverters();



        #region ICommand

        public ErrorDetailList Errors { get; } = new ErrorDetailList();



        void ICommand.Execute(Arguments arguments, bool throwIf)
        {
            var options = CommandLineOptions.FromArguments<TCommandParameters>(arguments);

            this.Errors.Add(options.Errors);
            if (!this.Errors.HasErrors())
            {
                // Convert all known options
                var commandParameters =
                    toCommandParameters<TCommandParameters>(options, out List<string> unresolvedProperties);

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
        
        
        
        

        /// <summary>
        ///     Convert the options provided in the command-line into an object.
        /// </summary>
        /// <typeparam name="TCommandOptions">The type of the object that is created and populated.</typeparam>
        /// <param name="commandLineOptions">
        ///     The options and their values as provided in the command-line and those who had a
        ///     default value.
        /// </param>
        /// <param name="unresolvedProperties">
        ///     A list of properties which were not resolved - where there
        ///     was no value provided in the command-line. Only not <see cref="OptionDescriptorAttribute.Required" />
        ///     options will be listed here, because required options must be in the command-line or they are listed
        ///     in the <see cref="Errors" /> list.
        /// </param>
        /// <returns></returns>
        private TCommandOptions toCommandParameters<TCommandOptions>(
            CommandLineOptions commandLineOptions, out List<string> unresolvedProperties)
            where TCommandOptions : class, new()
        {
            var targetInstance = new TCommandOptions();
            unresolvedProperties = new List<string>();

            // 
            // Iterate through all properties of the target type 
            //  which are decorated with a CommandLineOptionAttribute
            //
            foreach (var targetPropertyInfo in typeof(TCommandOptions).GetProperties())
            {
                // Need to find the related option-name for each property
                // The option was created from the command-line.
                // 1. Check if current Target-Property has a CommandLineOptionAttribute,
                //      then the attribute's Name specifies the option's name.
                // 2. Otherwise, the Property-Name will be used to look for a matching optionName.

                var allCustomAttributesOfType =
                    targetPropertyInfo.GetCustomAttributes(typeof(OptionDescriptorAttribute), true);
                OptionDescriptorAttribute firstOptionDescriptorAttribute
                    = (OptionDescriptorAttribute) (allCustomAttributesOfType.Length > 0
                        ? allCustomAttributesOfType[0]
                        : null);
                if (firstOptionDescriptorAttribute == null) continue;

                string optionName = firstOptionDescriptorAttribute.Name;
                string optionValue = commandLineOptions.GetProvidedValue(optionName);
                string targetPropertyName = targetPropertyInfo.Name;
                Type targetType = targetPropertyInfo.PropertyType;

                if (optionValue == null)
                {
                    //this.Errors.AddError(optionName,
                    //    $"Option {optionName} is missing from command-line and/or nor default value is specified." +
                    //    $"Cannot satisfy mapping for target property {targetPropertyInfo.DeclaringType.Name}.{optionName}.");
                    unresolvedProperties.Add(targetPropertyName);
                    continue;
                }

                if (!TypeConverters.CanConvert(targetType))
                {
                    Errors.AddError(targetPropertyName,
                                    $"No type converter found for type {targetType} of property {targetPropertyName} ");
                }
                else
                {
                    object propertyValue =TypeConverters.Convert(optionName, optionValue, this.Errors, targetType);
                    targetPropertyInfo.SetValue(targetInstance, propertyValue);
                }
            }

            return targetInstance;
            
        }
    }
}
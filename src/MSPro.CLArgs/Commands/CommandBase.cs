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
        /// <summary>
        ///     Get a list of Options unique by their name.
        /// </summary>
        /// <remarks>
        ///     Initially in <see cref="Arguments" /> there was an Option for each Tag that was provided in the command-line.
        ///     ..
        /// </remarks>
        public List<Option> ResolvedOptions { get; private set; }


        public TypeConverters TypeConverters { get; } = new TypeConverters();
        protected abstract void OnExecute(TCommandParameters commandParameters);



        protected virtual void OnResolveProperties(
            TCommandParameters commandParameters, 
            HashSet<string> unresolvedPropertyNames)
        {
        }



        /// <summary>
        ///     Convert the options provided in the command-line into an object.
        /// </summary>
        /// <typeparam name="TCommandOptions">The type of the object that is created and populated.</typeparam>
        /// <param name="optionResolver">
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
        private TCommandParameters createInstance(List<Option> options)
        {
            HashSet<string> unresolvedPropertyNames = new HashSet<string>();

            var targetInstance = new TCommandParameters();

            // 
            // Iterate through all properties of the target type 
            //  which are decorated with a CommandLineOptionAttribute
            //
            foreach (var targetPropertyInfo in typeof(TCommandParameters).GetProperties())
            {
                // Need to find the related option-name for each property
                // The option was created from the command-line.
                // 1. Check if current Target-Property has a CommandLineOptionAttribute,
                //      then the attribute Name specifies the option's name.
                // 2. Otherwise, the Property-Name will be used to look for a matching optionName.

                var allCustomAttributesOfType =
                    targetPropertyInfo.GetCustomAttributes(typeof(OptionDescriptorAttribute), true);
                OptionDescriptorAttribute firstOptionDescriptorAttribute
                    = (OptionDescriptorAttribute) (allCustomAttributesOfType.Length > 0
                        ? allCustomAttributesOfType[0]
                        : null);
                if (firstOptionDescriptorAttribute == null) continue;


                // the name of the Option that is bound to the property 
                string boundOptionName = firstOptionDescriptorAttribute.OptionName;
                // the name and the of the property
                string targetPropertyName = targetPropertyInfo.Name;
                Type targetType = targetPropertyInfo.PropertyType;


                // Check if the Option is defined. It is defined when it was in the
                // OptionDescriptorList that was used to ResolveOptions. 
                var option = options.FirstOrDefault(o => o.Key == boundOptionName);
                if (option == null || !option.IsResolved)
                {
                    // Should not happen because ResolveOptions should have added
                    // and Option for each item in the OptionDescriptorList.
                    // Console.WriteLine("WARN");

                    // Anyways: we cannot set the target property's value
                    // when there is no matching option.  
                    unresolvedPropertyNames.Add(targetPropertyName);
                }
                else if (!this.TypeConverters.CanConvert(targetType))
                {
                    this.Errors.AddError(targetPropertyName,
                                         $"No type converter found for type {targetType} of property {targetPropertyName} ");
                }
                else
                {
                    object propertyValue =
                        this.TypeConverters.Convert(boundOptionName, option.Value, this.Errors, targetType);
                    targetPropertyInfo.SetValue(targetInstance, propertyValue);
                }
            }
            // Assignment of known Options to Properties finished
            // Try to resolve the rest
            OnResolveProperties(targetInstance, unresolvedPropertyNames);
            return targetInstance;
        }



        #region ICommand

        public ErrorDetailList Errors { get; } = new ErrorDetailList();



        void ICommand.Execute(Arguments arguments, bool throwIf)
        {
            IOptionDescriptorProvider provider = new OptionDescriptorFromTypeProvider<TCommandParameters>();
            var clOpts = new OptionResolver(provider);
            this.ResolvedOptions = clOpts.ResolveOptions(arguments, this.Errors).ToList();
            if (!this.Errors.HasErrors())
            {
                var commandParameters = createInstance(this.ResolvedOptions);
                if (!this.Errors.HasErrors())
                {
                    OnExecute(commandParameters);
                }
            }

            if (!this.Errors.HasErrors() || !throwIf) return;

            throw new AggregateException(this.Errors.Details.Select(
                                             e => new ArgumentException(e.ErrorMessages[0], e.AttributeName)));
        }

        #endregion
    }
}
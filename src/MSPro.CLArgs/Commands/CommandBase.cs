using System;
using System.Collections.Generic;
using System.Diagnostics;
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


        public TypeConverters TypeConverters { get; } = new TypeConverters();    // TODO
        
        
        protected abstract void OnExecute(TCommandParameters commandParameters);



        protected virtual void OnResolveProperties(
            TCommandParameters commandParameters, 
            HashSet<string> unresolvedPropertyNames)
        {
        }



        /// <summary>
        /// The handler that is called when the command execution completed.
        /// </summary>
        /// <remarks>
        ///    The default handler does nothing on success or it throws an
        ///     <see cref="AggregateException"/> in case <see cref="Errors"/> contains any record.
        ///     Depending on the <see cref="Settings.TraceLevel"/> errors might be traced before.
        /// </remarks>
        public Action<ErrorDetailList> OnCompleted { get; set; } = errors =>
        {
            if (!errors.HasErrors()) return;
            throw new AggregateException(errors.Details.Select(
                                             e => new ArgumentException(e.ErrorMessages[0], e.AttributeName)));
        };



        /// <summary>
        ///     Convert the options provided in the command-line into an object.
        /// </summary>
        private TCommandParameters createInstance(List<Option> options)
        {
            HashSet<string> unresolvedPropertyNames = new HashSet<string>();
            TCommandParameters targetInstance = (TCommandParameters)
                createInstance(typeof(TCommandParameters), options, unresolvedPropertyNames);
            OnResolveProperties(targetInstance, unresolvedPropertyNames);
            return targetInstance;
        }



        private object createInstance(Type instanceType, 
                                      List<Option> options,
                                      HashSet<string> unresolvedPropertyNames)
        {
            object instance = Activator.CreateInstance(instanceType);
            foreach (var pi in instanceType.GetProperties())
            {
                if (pi.GetFirst<OptionSetAttribute>() != null)
                {
                    var o = createInstance( pi.PropertyType, options, unresolvedPropertyNames);
                    pi.SetValue(instance, o);
                }
                else
                {
                    var optionDescriptor = pi.GetFirst<OptionDescriptorAttribute>();
                    if (optionDescriptor == null) continue;


                    // the name of the Option that is bound to the property 
                    string boundOptionName = optionDescriptor.OptionName;
                    // the name and the of the property
                    string targetPropertyName = pi.Name;
                    Type targetType = pi.PropertyType;


                    // Check if the Option is defined. It is defined when it was in the
                    // OptionDescriptorList that was used to ResolveOptions. 
                    var option = options.FirstOrDefault(o => string.Equals( o.Key , boundOptionName));
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
                        pi.SetValue(instance, propertyValue);
                    }
                }
            }
            return instance;
        }



        #region ICommand

        public ErrorDetailList Errors { get; } = new ErrorDetailList();



        void ICommand.Execute(Arguments arguments, bool ignoreCase)
        {
            var provider = new OptionDescriptorFromTypeProvider<TCommandParameters>();
            var optionDescriptorList = provider.Get().ToList();
            Commander.Settings.RunIf(TraceLevel.Verbose, () =>
            {
                foreach (var optionDescriptorAttribute in optionDescriptorList)
                {
                    Commander.Settings.Trace( optionDescriptorAttribute.ToString());
                }
            });
            
            var optionResolver = new OptionResolver(optionDescriptorList);
            this.ResolvedOptions = optionResolver.ResolveOptions(
                arguments, 
                this.Errors, 
                Commander.Settings.IgnoreCase,
                Commander.Settings.IgnoreUnknownOptions).ToList();
            
            if (!this.Errors.HasErrors())
            {
                var commandParameters = createInstance(this.ResolvedOptions);
                if (!this.Errors.HasErrors())
                {
                    OnExecute(commandParameters);
                }
            }

            OnCompleted(this.Errors);
        }

        #endregion
    }
}
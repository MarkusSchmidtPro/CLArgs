﻿using MSPro.CLArgs;



namespace CLArgs.Sample.Options.DefaultCommand
{
    /// <summary>
    /// A very special parameter class,
    ///     which inherits from a base-class <see cref="BaseParameters"/>
    ///     and which uses an <c>OptionSet</c> class with
    ///     separate parameters.
    /// </summary>
    /// <remarks>
    ///    Finally, the following command-line option tags are supported
    ///    OptionName and/or any OptionTag:
    ///     --t  | --DatabaseTableName (from OptionDescriptorName)
    ///     --bs | --BaseSetting       (from OptionDescriptorName)
    ///     --u  | 
    ///     --p  | 
    ///     --d  | 
    /// </remarks>
    internal class Parameters : BaseParameters
    {
        [OptionSet]
        public Connection DbConnection { get; set; }

        [OptionDescriptor("DatabaseTableName", "t", Required = false)]
        public string DatabaseTableName { get; set; }
    }



    /// <summary>
    ///     Base class for parameters (will be automatically resolved).
    /// </summary>
    internal class BaseParameters
    {
        [OptionDescriptor("BaseSetting", "bs", Required = true)]
        public bool BaseSetting { get; set; }
    }



    /// <summary>
    ///     This class is a reusable type for command parameters.
    /// </summary>
    /// <remarks>
    ///     The connection-triple is used by many parameters.
    ///     So we can define these parameters in a separate class as an <see cref="OptionSetAttribute"/>.
    /// </remarks>
    internal class Connection
    {
        [OptionDescriptor("u", Required = true)]
        public string UserName { get; set; }

        [OptionDescriptor("p", Required = true)]
        public string Password { get; set; }

        [OptionDescriptor("d", Required = false)]
        public string Domain { get; set; }
    }
}
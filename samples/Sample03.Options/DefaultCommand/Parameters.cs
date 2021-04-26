using System.Collections.Generic;
using MSPro.CLArgs;



namespace CLArgs.Sample.Options.DefaultCommand
{
    /// <summary>
    /// A "very special" parameter class,
    ///     which inherits from a base-class <see cref="BaseContext"/>
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
    internal class Context : BaseContext
    {
        [OptionSet]
        public Connection DbConnection { get; set; }

        [OptionDescriptor("DatabaseTableName", "t", Required = false, AllowMultiple= nameof(DatabaseTableNames), AllowMultipleSplit=",;")] // 
        public string DatabaseTableName { get; set; }

        /// <summary>
        /// This property takes all /t parameters specified in the commandline, because /t is
        /// allowed to be specified multiple times.
        /// </summary>
        /// <remarks>
        /// DatabaseTableNames[0] = DatabaseTableName<br/>
        /// DatabaseTableNames is null if /t is not provided in the command-line.
        /// </remarks>
        public List<string> DatabaseTableNames { get; } = new  List<string>();
    }


    /*
    class XmlConverterParameters
    {
        [OptionDescriptor("filename", "f", Required = true)]
        public string Filename { get; set; }

        [OptionDescriptor("out", "o", Required = false, Default = "out")]
        public string OutDir  { get; set; }

        [OptionDescriptor("forceOverride", "fo", Required = false, Default = true)]
        public bool ForceOverride { get; set; }
    }
    */

    /// <summary>
    ///     Base class for parameters (will be automatically resolved).
    /// </summary>
    internal class BaseContext
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
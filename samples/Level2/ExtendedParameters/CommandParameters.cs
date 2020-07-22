using System.IO;
using MSPro.CLArgs;



namespace Level2.ExtendedParameters
{
    internal class CommandParameters
    {
        [OptionDescriptor("Connection", tag:"c", Required = true)]
        public Connection DBConnection { get; set; }

        [OptionDescriptor("DatabaseTableName", "t", Required = false)]
        public string DatabaseTableName { get; set; }
    }



    /// <summary>
    /// This class is a reusable type for command parameters.
    /// </summary>
    /// <remarks>
    ///    The connection-triple is used by many parameters.
    ///     So we can define these parameters in a separate class as some kind of <c>option group</c>
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
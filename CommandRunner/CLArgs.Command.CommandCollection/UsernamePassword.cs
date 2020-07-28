using MSPro.CLArgs;



namespace CLArgs.Command.CommandCollection
{
    /// <summary>
    /// Reusable Username / Password class for many Parameters 
    /// </summary>
    internal class UsernamePassword
    {
        [OptionDescriptor("UserName", "u")]
        public string UserName { get; set; }

        [OptionDescriptor("Password", "p")]
        public string Password { get; set; }

        [OptionDescriptor("WindowsAuthentication", "ad")]
        public bool WindowsAuthentication { get; set; }
    }
}
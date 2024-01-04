using System;



namespace MSPro.CLArgs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(string verb, string helpText = null)
        {
            Verb = verb;
            HelpText = helpText;
        }



        public string Verb { get; set; }
        public string HelpText { get; set; }
    }
}
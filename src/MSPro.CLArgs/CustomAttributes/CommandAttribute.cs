using System;
using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    [AttributeUsage(AttributeTargets.Class)]
    [PublicAPI]
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
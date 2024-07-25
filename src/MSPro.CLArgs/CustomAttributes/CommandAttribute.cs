using System;



namespace MSPro.CLArgs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute(string verb, string? helpText = null) : Attribute
    {
        public string Verb { get; set; } = verb;
        public string? HelpText { get; set; } = helpText;
    }
}
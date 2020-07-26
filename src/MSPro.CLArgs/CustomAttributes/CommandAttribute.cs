using System;
using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    [AttributeUsage(AttributeTargets.Class)]
    [PublicAPI]
    public class CommandAttribute : Attribute
    {
        public string Verb { get; set; }



        public CommandAttribute(string verb)
        {
            this.Verb = verb;
        }
    }
}
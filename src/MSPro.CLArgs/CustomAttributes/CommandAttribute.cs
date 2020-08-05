using System;
using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    [AttributeUsage(AttributeTargets.Class)]
    [PublicAPI]
    public class CommandAttribute : Attribute
    {
        public string Verb { get; set; }
        public string Description { get; set;}



        public CommandAttribute(string verb, string description=null)
        {
            this.Verb = verb;
            this.Description = description;
        }
    }
}
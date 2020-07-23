using System;
using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [PublicAPI]
    public class CommandAttribute : Attribute
    {
        public string Name { get; set; }



        public CommandAttribute(string name)
        {
            this.Name = name;
        }
    }
}
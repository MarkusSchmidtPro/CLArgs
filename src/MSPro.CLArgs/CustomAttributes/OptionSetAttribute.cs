using System;
using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    [PublicAPI]
    public class OptionSetAttribute : Attribute
    {
    }
}
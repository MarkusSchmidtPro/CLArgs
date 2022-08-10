using System;
using JetBrains.Annotations;



namespace MSPro.CLArgs;

[AttributeUsage(AttributeTargets.Property)]
[PublicAPI]
public class TargetsAttribute : Attribute
{
}
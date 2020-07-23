using System;
using System.Diagnostics;
using System.Reflection;



namespace MSPro.CLArgs
{
    static class ExtensionMethods
    {
        public static TAttribute GetFirst<TAttribute>(this ICustomAttributeProvider pi) where TAttribute : Attribute
        {
            TAttribute[] customAttributeOfType = (TAttribute[]) pi.GetCustomAttributes(typeof(TAttribute), true);
            if (customAttributeOfType.Length == 0) return null;
            
            Debug.Assert(customAttributeOfType.Length == 1);
            return (TAttribute) customAttributeOfType[0];
        }
    }
}
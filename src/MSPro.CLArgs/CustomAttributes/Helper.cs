using System;
using System.Diagnostics;
using System.Reflection;



namespace MSPro.CLArgs
{
    internal static class ExtensionMethods
    {
        public static TAttribute GetFirst<TAttribute>(this ICustomAttributeProvider pi) where TAttribute : Attribute
        {
            TAttribute[] customAttributeOfType = (TAttribute[]) pi.GetCustomAttributes(typeof(TAttribute), true);
            if (customAttributeOfType.Length == 0) return null;
            
            Debug.Assert(customAttributeOfType.Length == 1);
            return customAttributeOfType[0];
        }



        /// <summary>
        /// Run the code block enclosed in <c>action</c> if trace level allows.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="level"></param>
        /// <param name="action"></param>
        public static void RunIf(this Settings settings, TraceLevel level, Action action)
        {
            if (settings.TraceLevel >= level) action();
        }
        
        public static void Trace(this Settings settings, TraceLevel level, string message)
        {
            if (settings.TraceLevel >= level) settings.Trace?.Invoke(message);
        }
    }

}
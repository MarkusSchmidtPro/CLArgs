using System;
using System.Collections.Generic;



namespace MSPro.CLArgs
{
    /// <summary>
    /// Contains extension methods the Settings2.
    /// </summary>
    internal static class Settings2Extensions
    {
        /// <summary>
        /// Compare two string regarding the configured <see cref="Settings2.IgnoreCase"/> setting.
        /// </summary>
        /// <seealso cref="string.Equals(object?)"/>
        internal static bool Equals(this Settings2 settings2, string a, string b) =>
            string.Equals(a, b, settings2.IgnoreCase
                ? StringComparison.InvariantCultureIgnoreCase
                : StringComparison.InvariantCulture);
    
        /// <summary>
        /// A string comparer for collections
        /// that regards the the configured <see cref="Settings2.IgnoreCase"/> setting.
        /// </summary>
        /// <returns></returns>
        public static IEqualityComparer<string> GetStringComparer(this Settings2 settings2) =>
            settings2.IgnoreCase
                ? StringComparer.InvariantCultureIgnoreCase
                : StringComparer.InvariantCulture;
    }
}
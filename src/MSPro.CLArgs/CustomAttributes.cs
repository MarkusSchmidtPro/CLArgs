using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MSPro.CLArgs
{
    public static class CustomAttributes
    {
        public static Dictionary<PropertyInfo, TAttribute> getSingle<TAttribute>(Type t)
        {
            Dictionary<PropertyInfo, TAttribute> result = new Dictionary<PropertyInfo, TAttribute>();
            foreach (PropertyInfo pi in t.GetProperties())
            {
                var customAttributeOfType = pi.GetCustomAttributes(typeof(TAttribute), true);
                Debug.Assert(customAttributeOfType.Length==1);
                result[pi] = (TAttribute) customAttributeOfType[0];
            }
            return result;
        }
    }
}

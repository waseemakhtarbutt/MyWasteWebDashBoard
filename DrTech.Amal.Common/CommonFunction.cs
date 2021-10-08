using System;

namespace DrTech.Common
{
    public class CommonFunction
    {
        public static string GetDynamicPropertyValue(dynamic _Object, string _PropertyName)
        {
            return Convert.ToString(_Object.GetType().GetProperty(_PropertyName).GetValue(_Object, null));
        }
    }
}

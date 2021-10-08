using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using DrTech.Common.Attributes;

namespace DrTech.Common.Extentions
{
    public static class EnumExtensionMethod
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return null;
            var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute.Description;
        }
        //public static string GetMessage(this Enum value, string key)
        //{
        //    FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
        //    if (fieldInfo == null) return null;



        //    var attribute = (MessageAttribute)fieldInfo.GetCustomAttribute(typeof(MessageAttribute));
        //    return attribute.Messgae;
        //}

        public static string GetSubTitle<T>(string key)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var descriptionAttribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                var titleAttribute = Attribute.GetCustomAttribute(field,
                    typeof(SubTypeTitleAttribute)) as SubTypeTitleAttribute;
                if (descriptionAttribute != null)
                {
                    if (descriptionAttribute.Description == key)
                    {
                        return titleAttribute.Title;
                    }
                        //return (T)field.GetValue(null);
                }
                //else
                //{
                //    if (field.Name == key)
                //        return (T)field.GetValue(null);
                //}
            }
            return "";
        }
        public static string GetTitle<T>(string key)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var descriptionAttribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                var titleAttribute = Attribute.GetCustomAttribute(field,
                    typeof(TypeTitleAttribute)) as TypeTitleAttribute;
                if (descriptionAttribute != null)
                {
                    if (descriptionAttribute.Description == key)
                    {
                        return titleAttribute.Title;
                    }
                    //return (T)field.GetValue(null);
                }
                //else
                //{
                //    if (field.Name == key)
                //        return (T)field.GetValue(null);
                //}
            }
            return "";
        }
    }
}

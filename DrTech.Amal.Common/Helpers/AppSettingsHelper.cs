using DrTech.Amal.Common.Extentions;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration;
using System.IO;

namespace DrTech.Amal.Common.Helpers
{
    public class AppSettingsHelper
    {
        //static IConfigurationRoot Root;
        //static AppSettingsHelper()
        //{
        //    var configurationBuilder = new ConfigurationBuilder();

        //    var path = Path.Combine(Directory.GetCurrentDirectory(), Constants.AppSettings.FILE_NAME);

        //    configurationBuilder.AddJsonFile(path, false);

        //    Root = configurationBuilder.Build();
        //}

        //public static string GetAttributeValue(string SectionName, string AttributeName)
        //{
        //    IConfigurationSection Env = Root.GetSection(Constants.AppSettings.ENV);

        //    IConfigurationSection Section = Root.GetSection(Env.Value + ":" + SectionName);

        //    string AttributeValue = Section[AttributeName];

        //    return AttributeValue;
        //}

        //public static string GetEnvironmentValue(string SectionName, string AttributeName)
        //{
        //    IConfigurationSection Env = Root.GetSection(Constants.AppSettings.ENV);

        //    IConfigurationSection Section = Root.GetSection(Env.Value + ":" + SectionName);

        //    string AttributeValue = Env.Value;

        //    return AttributeValue;
        //}
    }
}

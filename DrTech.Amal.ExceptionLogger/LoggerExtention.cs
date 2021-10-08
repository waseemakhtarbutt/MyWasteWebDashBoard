using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace DrTech.Amal.ExceptionLogger
{
    public class LoggerExtention
    {
        public static void LogException(Exception exp)
        {
            ExceptionLogging dd = new ExceptionLogging
            {
                Date = DateTime.Now.ToString(),
                StackTrace = exp.StackTrace,
                ErrorMessage = exp.Message,
                InnerErrorMessage = GetAllExceptionText(exp.InnerException)
            };

            try
            {
                insertToDB(dd);
            }
            catch (Exception)
            {
                Logger logger = GetLogger();
                logger.Error(exp);
            }

        }

        private static void insertToDB(ExceptionLogging obj)
        {
            string ConnectionString = GetAttributeValue("MongoDbSettings", "ConnectionString");
            string Database = GetAttributeValue("MongoDbSettings", "DatabaseName");

            //string ConnectionString = "";
            //string Database = "";

            //var client = new MongoClient(ConnectionString);
            //var db = client.GetDatabase(Database);

            //var collection = db.GetCollection<ExceptionLogging>("ExceptionLogging");
            //collection.InsertOne(obj);
        }


        private static string GetAttributeValue(string SectionName, string AttributeName)
        {
            //var configurationBuilder = new ConfigurationBuilder();

            //var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            //configurationBuilder.AddJsonFile(path, false);

            //var Root = configurationBuilder.Build();

            //IConfigurationSection Env = Root.GetSection("ENV");

            //IConfigurationSection Section = Root.GetSection(Env.Value + ":" + SectionName);

            //string AttributeValue = Section[AttributeName];

            //return AttributeValue;
            return "";
        }
        private static Logger GetLogger()
        {
            //FileTarget fileTargetDebug = new FileTarget()
            //{
            //    Layout = "${date:format=HH\\:MM\\:ss} ${logger} ${message}",
            //    FileName = "${basedir}/log/debug.txt",
            //    ArchiveFileName = "${basedir}/Logs/previous-{#}.log",
            //    ArchiveEvery = FileArchivePeriod.Sunday,
            //    ArchiveNumbering = ArchiveNumberingMode.Rolling,
            //    ConcurrentWrites = true,
            //    KeepFileOpen = false,
            //    Encoding = Encoding.Unicode


            //};
            FileTarget fileTargetError = new FileTarget()
            {
                Layout = "${longdate} ${logger}${newline}${message}${onexception:${newline}${exception:maxInnerExceptionLevel=10:format=shortType,message,stacktrace:separator=*:innerExceptionSeparator=&#xD;&#xA;&#x9;}}${newline}${newline}",
                FileName = "${basedir}/log/error.txt",
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveNumbering = ArchiveNumberingMode.Rolling,
                ConcurrentWrites = true,
                KeepFileOpen = false,
                Encoding = Encoding.Unicode
            };
            //FileTarget fileTargetWarning = new FileTarget()
            //{
            //    Layout = "${date:format=HH\\:MM\\:ss} ${logger} ${message}",
            //    FileName = "${basedir}/log/warning.txt",
            //    ArchiveEvery = FileArchivePeriod.Sunday,
            //    ArchiveNumbering = ArchiveNumberingMode.Rolling,
            //    ConcurrentWrites = true,
            //    KeepFileOpen = false,
            //    Encoding = Encoding.Unicode
            //};

            // LoggingRule ruleDebug = new LoggingRule("*", LogLevel.Debug, fileTargetDebug);
            LoggingRule ruleError = new LoggingRule("*", LogLevel.Error, fileTargetError);
            //  LoggingRule ruleWarning = new LoggingRule("*", LogLevel.Warn, fileTargetWarning);


            LoggingConfiguration config = new LoggingConfiguration();

            //    config.AddTarget("file", fileTargetDebug);
            config.AddTarget("file", fileTargetError);
            // config.AddTarget("file", fileTargetWarning);

            // config.LoggingRules.Add(ruleDebug);
            config.LoggingRules.Add(ruleError);
            // config.LoggingRules.Add(ruleWarning);

            LogManager.Configuration = config;

            Logger logger = LogManager.GetLogger("loggerextension");

            return logger;
        }

        private static string GetAllExceptionText(Exception exp)
        {
            if (exp == null) return "";

            string message = exp.Message;

            if (exp.InnerException != null)
                return message;
            return GetAllExceptionText(exp.InnerException);
        }
    }
}

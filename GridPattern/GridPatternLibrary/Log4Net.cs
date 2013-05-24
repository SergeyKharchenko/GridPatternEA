using System;
using System.Globalization;
using System.IO;
using System.Linq;
using GridPatternLibrary.Support;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace GridPatternLibrary
{
    public class Log4Net
    {        
        private const string LogPattern = "%date{yyyy.MM.dd HH:mm:ss} - %message%newline";

        public PatternLayout DefaultLayout
        {
            get { return defaultLayout; }
        }

        private readonly PatternLayout defaultLayout = new PatternLayout();

        public Log4Net()
        {
            defaultLayout.ConversionPattern = LogPattern;
            defaultLayout.ActivateOptions();
        }

        public static ILog GetLogger(string appName, int id)
        {
            var repositoryName = id.ToString(CultureInfo.InvariantCulture);
            var loggerName = id.ToString(CultureInfo.InvariantCulture);

            if (IsRepositoryExist(repositoryName))
                return LogManager.GetLogger(repositoryName, loggerName);

            CreateRepository(appName, repositoryName);
            return LogManager.GetLogger(repositoryName, loggerName);
        }

        private static bool IsRepositoryExist(string repositoryName)
        {
            return LogManager.GetAllRepositories().Any(repository => repository.Name == repositoryName);
        }

        private static void CreateRepository(string appName, string repositoryName)
        {
            var repository = LoggerManager.CreateRepository(repositoryName);
            var hierarchy = (Hierarchy) repository;
            var tracer = new TraceAppender();
            var patternLayout = new PatternLayout
            {
                ConversionPattern = LogPattern
            };

            patternLayout.ActivateOptions();

            tracer.Layout = patternLayout;
            tracer.ActivateOptions();
            hierarchy.Root.AddAppender(tracer);

            var logFileCommonFolder = Path.Combine(Functions.GetAppDataDirectory(appName), "logs");
            var logFileFolder = Path.Combine(logFileCommonFolder, repositoryName);
            var logFileName = Path.Combine(logFileFolder, "logfile_.txt");

            var roller = new RollingFileAppender
            {
                Layout = patternLayout,
                AppendToFile = true,
                RollingStyle = RollingFileAppender.RollingMode.Date,
                MaxSizeRollBackups = -1,
                DatePattern = "yyyy.MM.dd",
                StaticLogFileName = false,
                File = logFileName,
                PreserveLogFileNameExtension = true
            };
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;
        }

        public static void ShoutdownLogger(int id)
        {
            var repositoryName = id.ToString(CultureInfo.InvariantCulture);
            var loggerName = id.ToString(CultureInfo.InvariantCulture);
            if (IsRepositoryExist(repositoryName))
            {                
                var logger = LogManager.GetLogger(repositoryName, loggerName);
                
                foreach (var appender in ((IAppenderAttachable) logger.Logger).Appenders)
                {
                    appender.Close();
                }
            }

        }
    }
}

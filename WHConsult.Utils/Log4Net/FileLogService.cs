// Copied from http://www.codeproject.com/Tips/381212/log4Net-logging-framework
// Thanks to Prabu ram for writing this article

using System;
using System.IO;
using log4net;

namespace WHConsult.Utils.Log4Net
{
    public sealed class FileLogService : ILogService
    {
        readonly ILog _logger;

        static FileLogService()
        {
            // Gets directory path of the calling application
            // RelativeSearchPath is null if the executing assembly i.e. calling assembly is a
            // stand alone exe file (Console, WinForm, etc). 
            // RelativeSearchPath is not null if the calling assembly is a web hosted application i.e. a web site
            var log4NetConfigDirectory = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;

            var log4NetConfigFilePath = Path.Combine(log4NetConfigDirectory, "Log4Net\\log4net.config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(log4NetConfigFilePath));
        }

        public FileLogService(Type logClass)
        {
            _logger = LogManager.GetLogger(logClass);
        }

        public void Fatal(string errorMessage)
        {
            if (_logger.IsFatalEnabled)
                _logger.Fatal(errorMessage);
        }

        public void Error(string errorMessage)
        {
            if (_logger.IsErrorEnabled)
                _logger.Error(errorMessage);
        }

        public void Warn(string message)
        {
            if (_logger.IsWarnEnabled)
                _logger.Warn(message);
        }

        public void Info(string message)
        {
            if (_logger.IsInfoEnabled)
                _logger.Info(message);
        }

        public void Debug(string message)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug(message);
        }
    }
}

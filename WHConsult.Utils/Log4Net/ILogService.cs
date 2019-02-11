// Copied from http://www.codeproject.com/Tips/381212/log4Net-logging-framework
// Thanks to Prabu ram for writing this article

namespace WHConsult.Utils.Log4Net
{
    public interface ILogService
    {
        void Fatal(string message);
        void Error(string message);
        void Warn(string message);
        void Info(string message);
        void Debug(string message);
    }
}

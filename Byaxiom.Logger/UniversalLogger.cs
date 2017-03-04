using Byaxiom.Matrix;
using NLog;
using System;

namespace Byaxiom.Logger
{

    public interface IUniversalLogger {
        void Trace(string message);
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Fatal(string message);

        void Log(Exception ex);
    }

    public class UniversalLogger : IUniversalLogger {
        private static readonly NLog.Logger Logger = LogManager.GetCurrentClassLogger();

        public void Trace(string message) {
            Logger.Trace(message);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            WriteToConsole(message);
            EventLogger.WriteToEventLog(message);
        }

        public void Debug(string message) {
            Logger.Debug(message);
            WriteToConsole(message);
            EventLogger.WriteToEventLog(message);
        }

        public void Info(string message) {
            Logger.Info(message);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            WriteToConsole(message);
            EventLogger.WriteToEventLog(message);
        }

        public void Warn(string message) {
            Logger.Warn(message);
            Console.ForegroundColor = ConsoleColor.Red;
            WriteToConsole(message);
            EventLogger.WriteToEventLog(message);
        }

        public void Error(string message) {
            Logger.Error(message);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            WriteToConsole(message);
            EventLogger.WriteToEventLog(message);
        }

        public void Fatal(string message) {
            Logger.Fatal(message);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            WriteToConsole(message);
            EventLogger.WriteToEventLog(message);
        }

        public void Log(Exception ex) {
            ex.Log();
            var message = ex.ToDetails();

            //Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.DarkRed;

            WriteToConsole(message);
            Logger.Error(message);
            EventLogger.WriteToEventLog(ex);
        }

        public void WriteToConsole(string message) {
            message = string.Format("{0}: {1}", DateTime.Now.ToString(DateFormat.Long), message);

            Console.WriteLine(message);
            Console.ResetColor();
        }

    }
}
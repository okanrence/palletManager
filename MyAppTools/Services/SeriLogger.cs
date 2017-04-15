using Serilog;
using Serilog.Core;
using System;

namespace Byaxiom.Logger
{


    public class SeriLogger : IUniversalLogger
    {
        private static readonly Serilog.Core.Logger Logger = new LoggerConfiguration()
             .WriteTo.LiterateConsole()
             .WriteTo.RollingFile("log-{Date}.txt")
             .CreateLogger();
       public void Trace(string message)
        {
            Logger.Fatal(message);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            //WriteToConsole(message);
            //EventLogger.WriteToEventLog(message);
        }

        public void Debug(string message)
        {
            Logger.Debug(message);
            //WriteToConsole(message);
            //EventLogger.WriteToEventLog(message);
        }

        public void Info(string message)
        {
            Logger.Information(message);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            //WriteToConsole(message);
            //EventLogger.WriteToEventLog(message);
        }

        public void Warn(string message)
        {
            Logger.Warning(message);
            Console.ForegroundColor = ConsoleColor.Red;
            //WriteToConsole(message);
            //EventLogger.WriteToEventLog(message);
        }

        public void Error(string message)
        {
            Logger.Error(message);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            //WriteToConsole(message);
            //EventLogger.WriteToEventLog(message);
        }

        public void Fatal(string message)
        {
            Logger.Fatal(message);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            //WriteToConsole(message);
            //EventLogger.WriteToEventLog(message);
        }

        public void Log(Exception ex)
        {
            // ex.Log();
            var message = $"{ex.Message} { ex.StackTrace } {ex.InnerException}";

            //Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.DarkRed;

          //  WriteToConsole(message);
            Logger.Error(message);
            //EventLogger.WriteToEventLog(ex);
        }

        //public void WriteToConsole(string message)
        //{
        //    message = string.Format("{0}: {1}", DateTime.Now.ToString("dd-M-yyyy hh:mm:ss tt"), message);

        //    Console.WriteLine(message);
        //    Console.ResetColor();
        //}


    }
}
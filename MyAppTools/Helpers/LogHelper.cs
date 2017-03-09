using System;

namespace Byaxiom.Logger
{
    public static class LogHelper {
        private static readonly IUniversalLogger Logger = new UniversalLogger();
        public static bool DebugMode { get; set; }


        public static void Trace(string message) {
            Logger.Trace(message);
        }

        public static void Debug(string message) {
            if (DebugMode) {
                Logger.Debug(message);
            }
        }

        public static void Info(string message) {
            Logger.Info(message);
        }

        public static void Warn(string message) {
            Logger.Warn(message);
        }

        public static void Error(string message) {
            Logger.Error(message);
        }

        public static void Fatal(string message) {
            Logger.Fatal(message);
        }

        public static void Log(Exception ex) {
            Logger.Log(ex);
        }
    }
}
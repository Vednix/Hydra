using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hydra.Extensions
{
    public class Logger
    {
        static FileStream fsLogger;// = new FileStream(Path.Combine(Service.logs_dir, $"chat-log-{Service.logdate}.txt"), FileMode.Append,FileAccess.ReadWrite,FileShare.Read);
        static StreamWriter logWriter;// = new StreamWriter(fsLogger, new UTF8Encoding(false));
        static SemaphoreSlim loglocker = new SemaphoreSlim(1, 1);
        public static string DateTimeNow => DateTime.Now.ToString(Base.Config.DateTimeFormat);
        public static string DateTimeNowFile => DateTime.Now.ToString(Base.Config.DateTimeFormatLogFile);

        public static void WriteLine(string toWrite, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(toWrite);
            Console.ResetColor();
        }
        public static void Write(string toWrite, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write(toWrite);
            Console.ResetColor();
        }
        public static void doLog(string message, Config.DebugLevel level)
        {
            string toLog = "";
            ConsoleColor color = ConsoleColor.White;
            switch (level)
            {
                case Config.DebugLevel.All:
                    toLog = $"[{DateTimeNow}] [DEBUG] {message}";
                    color = ConsoleColor.DarkCyan;
                    break;
                case Config.DebugLevel.Info:
                    toLog = $"[{DateTimeNow}] [INFO] {message}";
                    color = ConsoleColor.DarkYellow;
                    break;
                case Config.DebugLevel.Error:
                    toLog = $"[{DateTimeNow}] [ERROR] {message}";
                    color = ConsoleColor.Red;
                    break;
                case Config.DebugLevel.Critical:
                    toLog = $"[{DateTimeNow}] [CRITICAL] {message}";
                    color = ConsoleColor.DarkRed;
                    break;
                case Config.DebugLevel.Unsecure:
                    toLog = $"[{DateTimeNow}] [UNSECURE] {message}";
                    color = ConsoleColor.DarkMagenta;
                    if ((int)level <= (int)Base.Config.debugLevel)
                        WriteLine(toLog, color);
                    return;
            }
            loglocker.Wait();
            if (fsLogger is null)
            {
                fsLogger = new FileStream(Path.Combine(Base.Config.logPath, $"hydra-{DateTimeNowFile}.log"), FileMode.Append, FileAccess.Write, FileShare.Read);
                logWriter = new StreamWriter(fsLogger, new UTF8Encoding(false));
                logWriter.AutoFlush = false;
            }
            try
            {
                logWriter.WriteLine(toLog);
                logWriter.Flush();
                if ((int)level <= (int)Base.Config.debugLevel)
                    WriteLine(toLog, color);
            }
            finally { loglocker.Release(); }
        }
        public static void doLogLang(string DefaultMessage, Config.DebugLevel level, TSPlayerB.Language language, string PortugueseMessage = null, string SpanishMessage = null, string EnglishMessageIfNotDefault = null)
        {
            string toLog = "";
            ConsoleColor color = ConsoleColor.White;
            switch (language)
            {
                case TSPlayerB.Language.English:
                    DefaultMessage = EnglishMessageIfNotDefault == null ? DefaultMessage : EnglishMessageIfNotDefault; ;
                    break;
                case TSPlayerB.Language.Portuguese:
                    DefaultMessage = PortugueseMessage == null ? DefaultMessage : PortugueseMessage; ;
                    break;
                case TSPlayerB.Language.Spanish:
                    DefaultMessage = SpanishMessage == null ? DefaultMessage : SpanishMessage; ;
                    break;
                default:
                    //Pass
                    break;
            }
            switch (level)
            {
                case Config.DebugLevel.All:
                    toLog = $"[{DateTimeNow}] [DEBUG] {DefaultMessage}";
                    color = ConsoleColor.DarkCyan;
                    break;
                case Config.DebugLevel.Info:
                    toLog = $"[{DateTimeNow}] [INFO] {DefaultMessage}";
                    color = ConsoleColor.DarkYellow;
                    break;
                case Config.DebugLevel.Error:
                    toLog = $"[{DateTimeNow}] [ERROR] {DefaultMessage}";
                    color = ConsoleColor.Red;
                    break;
                case Config.DebugLevel.Critical:
                    toLog = $"[{DateTimeNow}] [CRITICAL] {DefaultMessage}";
                    color = ConsoleColor.DarkRed;
                    break;
                case Config.DebugLevel.Unsecure:
                    toLog = $"[{DateTimeNow}] [UNSECURE] {DefaultMessage}";
                    color = ConsoleColor.DarkMagenta;
                    if ((int)level <= (int)Base.Config.debugLevel)
                        WriteLine(toLog, color);
                    return;
            }
            loglocker.Wait();
            if (fsLogger is null)
            {
                fsLogger = new FileStream(Path.Combine(Base.Config.logPath, $"hydra-{DateTimeNowFile}.log"), FileMode.Append, FileAccess.Write, FileShare.Read);
                logWriter = new StreamWriter(fsLogger, new UTF8Encoding(false));
                logWriter.AutoFlush = false;
            }
            try
            {
                logWriter.WriteLine(toLog);
                logWriter.Flush();
                if ((int)level <= (int)Base.Config.debugLevel)
                    WriteLine(toLog, color);
            }
            finally { loglocker.Release(); }
        }
    }
}

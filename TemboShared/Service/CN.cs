using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using TemboContext.Context;

namespace TemboShared.Service
{
    public static class CN
    {
        public static DbContext Db = new DbContext();
        private static Int64 inmemoryId;
        public static readonly string Base = AppDomain.CurrentDomain.BaseDirectory;
        private static dynamic settings;
        public static bool ReloadSettings = false;
        public static double Threshold = 0.00001;
        static Random rnd=new Random();
        public static dynamic Settings
        {
            get
            {
                if (settings == null)
                {
                    lock (FileLock)
                    {
                        var fsSource = new FileStream($"{Base}settings.json", FileMode.Open, FileAccess.Read);
                        var sr = new StreamReader(fsSource);
                        settings = JsonConvert.DeserializeObject<dynamic>(sr.ReadToEnd());
                        sr.Close();
                        sr.Dispose();
                        fsSource.Close();
                        fsSource.Dispose();
                    }
                }
                if (ReloadSettings)
                {
                    lock (FileLock)
                    {
                        var fsSource = new FileStream($"{Base}settings.json", FileMode.Open, FileAccess.Read);
                        var sr = new StreamReader(fsSource);
                        settings = JsonConvert.DeserializeObject<dynamic>(sr.ReadToEnd());
                        ReloadSettings = false;
                        sr.Close();
                        sr.Dispose();
                        fsSource.Close();
                        fsSource.Dispose();
                    }

                }
                return settings;
            }
        }

        public static void SaveSettings()
        {
            File.WriteAllText($"{Base}settings.json",JsonConvert.SerializeObject(Settings));
        }
        public static Int64 InMemoryId()
        {
            inmemoryId =rnd.Next();
            return inmemoryId;
        }
        public static void Assert(bool condition, string message = "Condition not true")
        {
            if (!condition)
            {
                throw new Exception(message);
            }
        }
        /// <summary>
        /// Converts a bool to an int
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        public static int ToInt(this bool bl) => bl ? 1 : 0;

        /// <summary>
        /// C:\inetpub\wwwroot\levy\files\logs\log.txt
        /// </summary>
        /// <param name="logFileName"></param>
        /// <returns></returns>
        public static string LogFile(string logFileName = "log.txt")
        {
            var logs = $"{Base}logs\\";
            Directory.CreateDirectory(logs);
            return $"{logs}{logFileName}";
        }
        public static Object FileLock = new Object();
        /// <summary>
        /// Prints message to screen
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type">0 White, 1 Red, 2 Blue, 3 Green, 4 Yellow</param>
        public static void Log(this string message, int type = 0, bool skipLog = false)
        {
            try
            {
                if (type == 0)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                if (type == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                if (type == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                if (type == 3)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                if (type == 4)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                if (type == 5)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                }
                if (type == 6)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
                if (skipLog) return;
                lock (FileLock)
                {
                    var fileInfo = new FileInfo(CN.LogFile());
                    if (fileInfo.Length >= 10000000)
                    {
                        var nextNumber = $"00{(int.Parse(CN.Settings.lastLogNumber)+1)}";
                        CN.Settings.lastLogNumber = nextNumber;
                        SaveSettings();
                        File.Copy(CN.LogFile(), $"{LogFile()}log_{nextNumber}.bak");
                        File.Delete(CN.LogFile());
                    }
                    File.AppendAllText(CN.LogFile(), $"Time: {DateTime.Now:dd/MM/yyyy hh:mm:ss} {message}\r\n");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}

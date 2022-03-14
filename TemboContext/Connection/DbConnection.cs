using System;
using System.Data.SqlClient;
using System.IO;
using Newtonsoft.Json;

namespace TemboContext.Connection
{
	public static class DbConnection
	{
        private static dynamic settings;
        public static readonly string Base = AppDomain.CurrentDomain.BaseDirectory;
        public static SqlConnection CreateConnection()
		{
			var connection = new SqlConnection(GetConnectionString());
			
			return connection;
		}
		public static string GetConnectionString()
		{
			return Settings.databaseConnectionString;
		}

        public static dynamic Settings
        {
            get
            {
                var fsSource = new FileStream($"{Base}settings.json", FileMode.Open, FileAccess.Read);
                var sr = new StreamReader(fsSource);
                settings = JsonConvert.DeserializeObject<dynamic>(sr.ReadToEnd());
                sr.Close();
                sr.Dispose();
                fsSource.Close();
                fsSource.Dispose();
                return settings;
            }
        }
    }
}

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace CalculatorApi
{
    public class DbLogger : ILogger
    {

        IConfiguration configuration;
        public DbLogger(IConfiguration configuration) 
        { 
            this.configuration = configuration;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var log = new JObject();
            log["LogLevel"] = logLevel.ToString();
            log["EventId"] = eventId.Id;
            log["EventName"] = eventId.Name;
            log["Message"] = formatter(state,exception);
            log["StackTrace"] = exception?.StackTrace?? string.Empty;

            using (var connection = new SqliteConnection(configuration.GetConnectionString("default")))
            {
                connection.Open();
                var sql = "INSERT INTO Logger (Log, Created) VALUES (@Log, @Created)";
                using(var command = connection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = sql;
                        var jsonLog = JsonConvert.SerializeObject(log, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            DefaultValueHandling = DefaultValueHandling.Ignore,
                            Formatting = Formatting.None
                        });
                        command.Parameters.Add(new SqliteParameter("@Log", jsonLog));
                        command.Parameters.Add(new SqliteParameter("@Created", DateTimeOffset.Now));
                        command.ExecuteNonQuery();
                    }
                    catch(SqliteException ex)
                    {
                        if( ex.Message.Equals("SQLite Error 1: 'no such table: Logger'."))
                        {
                            using (var cmd = connection.CreateCommand())
                            {
                                cmd.CommandText = @"CREATE TABLE Logger (id INTEGER PRIMARY KEY,Log TEXT, Created DATE)";
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        { throw; }
                    }
                    finally { connection.Close(); }
                }
            }
        }
    }
}
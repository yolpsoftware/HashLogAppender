using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Appender;
using System.Data.SqlClient;

namespace HashLogAppender.Log4Net
{
    public class HashLogAppender : AppenderSkeleton
    {
        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            var hashMessage = new StringBuilder()
                .Append(loggingEvent.Level.Value.ToString())
                .Append(loggingEvent.LoggerName)
                .Append("_")
                .Append(loggingEvent.RenderedMessage)
                .ToString();

            var smallHash = hashMessage.Substring(0, 32);
            smallHash = Hash.GetHash(smallHash, Hash.HashType.SHA256);
            var bigHash = Hash.GetHash(hashMessage, Hash.HashType.SHA256);

            var connection = new SqlConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();
            var cmd = new SqlCommand("AddHashLog", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("smallhash", System.Data.SqlDbType.Char, 64).Value = smallHash;
            cmd.Parameters.Add("bighash", System.Data.SqlDbType.Char, 64).Value = bigHash;
            cmd.Parameters.Add("logger", System.Data.SqlDbType.VarChar).Value = loggingEvent.LoggerName;
            cmd.Parameters.Add("level", System.Data.SqlDbType.TinyInt).Value = (byte)(loggingEvent.Level.Value + 1);
            cmd.Parameters.Add("date", System.Data.SqlDbType.DateTime2, 7).Value = loggingEvent.TimeStamp;
            cmd.Parameters.Add("message", System.Data.SqlDbType.VarChar).Value = loggingEvent.RenderedMessage;
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        public string ConnectionString { get; set; }
    }
}

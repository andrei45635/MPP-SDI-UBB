using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using ConnectionUtils;

namespace ConnectionUtils
{
    public class SqliteConnectionFactory : ConnectionFactory
    {
        public override IDbConnection CreateConnection(IDictionary<string, string> props)
        {
            String ConnectionString = props["ConnectionString"];
            Console.WriteLine("SQlite: opening connection at {0}", ConnectionString);
            return new SQLiteConnection(ConnectionString);
        }
    }
}
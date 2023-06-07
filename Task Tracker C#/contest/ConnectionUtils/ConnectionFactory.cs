using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace ConnectionUtils
{
    public abstract class ConnectionFactory
    {
        protected ConnectionFactory()
        {
        }

        private static ConnectionFactory Instance;

        public static ConnectionFactory GetInstance()
        {
            if (Instance == null)
            {
                Assembly assembler = Assembly.GetExecutingAssembly();
                Type[] types = assembler.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsSubclassOf(typeof(ConnectionFactory)))
                    {
                        Instance = (ConnectionFactory)Activator.CreateInstance(type);
                    }
                }
            }
            return Instance;
        }

        public abstract IDbConnection CreateConnection(IDictionary<string, string> props);
    }
}
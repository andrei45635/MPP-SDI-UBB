using System.Collections.Generic;
using System.Data;
using ConnectionUtils1;

namespace contest.utils
{
    public class DButils
    {
        private static IDbConnection Instance = null;
        
        public static IDbConnection GetConnection(IDictionary<string, string> props)
        {
            if (Instance == null || Instance.State == ConnectionState.Closed)
            {
                Instance = GetNewConnection(props);
                Instance.Open();
            }
            return Instance;
        }

        private static IDbConnection GetNewConnection(IDictionary<string, string> props)
        {
            return ConnectionFactory.GetInstance().CreateConnection(props);
        }
    }
}
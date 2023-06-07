using System;
using System.Collections.Generic;
using System.Data;
using contest.domain;
using contest.utils;
using log4net;

namespace contest.repo.users
{
    public class UserDBRepository : IRepository<int, User>
    {
        private static readonly ILog logger = LogManager.GetLogger("UserDBRepository");

        private IDictionary<string, string> Properties;

        public UserDBRepository(IDictionary<string, string> properties)
        {
            logger.Info("Creating User DB Repo");
            Properties = properties;
        }

        public IEnumerable<User> FindAll()
        {
            IDbConnection dbConnection = DButils.GetConnection(Properties);
            IList<User> users = new List<User>();
            
            using (var comm = dbConnection.CreateCommand())
            {
                logger.InfoFormat("all users");
                comm.CommandText = "SELECT id, username, password FROM users";
                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int idU = dataR.GetInt32(0);
                        String username = dataR.GetString(1);
                        String password = dataR.GetString(2);
                        User user = new User(idU, username, password);
                        users.Add(user);
                    }
                }
            }

            return users;
        }

        public User Save(User e)
        {
            var dbConnection = DButils.GetConnection(Properties);

            using (var comm = dbConnection.CreateCommand())
            {
                logger.InfoFormat("saving user {0}", e);
                comm.CommandText = "INSERT INTO users VALUES (@userid, @username, @password)";
                
                var paramID = comm.CreateParameter();
                paramID.ParameterName = "@userid";
                paramID.Value = e.ID;
                comm.Parameters.Add(paramID);

                var paramUsername = comm.CreateParameter();
                paramUsername.ParameterName = "@username";
                paramUsername.Value = e.Username;
                comm.Parameters.Add(paramUsername);

                var paramPassword = comm.CreateParameter();
                paramPassword.ParameterName = "@password";
                paramPassword.Value = e.Password;
                comm.Parameters.Add(paramPassword);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                {
                    logger.InfoFormat("Error DB");
                    throw new Exception("No user added!\n");
                }
            }

            return e;
        }

        public bool Delete(User e)
        {
            throw new System.NotImplementedException();
        }

        public User Update(User e)
        {
            throw new System.NotImplementedException();
        }

        public int Size()
        {
            IDbConnection dbConnection = DButils.GetConnection(Properties);
            
            using (var comm = dbConnection.CreateCommand())
            {
                comm.CommandText = "SELECT count(*) FROM users";
                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        logger.InfoFormat("count(*) users");
                        return dataR.GetInt32(0);
                    }
                }
            }

            return 0;
        }
    }
}
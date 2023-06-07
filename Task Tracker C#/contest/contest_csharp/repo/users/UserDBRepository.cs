using System;
using System.Collections.Generic;
using System.Data;
using contest_csharp.domain;
using contest_csharp.utils;
using log4net;

namespace contest_csharp.repo.users
{
    public class UserDBRepository : UserRepo
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
            logger.InfoFormat("all users");
            IDbConnection dbConnection = DButils.GetConnection(Properties);
            IList<User> users = new List<User>();

            using (var comm = dbConnection.CreateCommand())
            {
                logger.InfoFormat("all users");
                comm.CommandText = "SELECT userid, username, password FROM users";
                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int idU = dataR.GetInt32(0);
                        string username = dataR.GetString(1);
                        string password = dataR.GetString(2);
                        User user = new User(idU, username, password);
                        users.Add(user);
                    }
                }
            }

            return users;
        }


        public bool FindUser(string username, string password)
        {
            logger.Info("checking to see if the requested user exists");
            IDbConnection dbConnection = DButils.GetConnection(Properties);

            using (var comm = dbConnection.CreateCommand())
            {
                logger.InfoFormat("check if the user exists");
                comm.CommandText = "SELECT userid, username, password FROM users WHERE username=@username AND password=@password";
                
                var paramUsername = comm.CreateParameter();
                paramUsername.ParameterName = "@username";
                paramUsername.Value = username;
                comm.Parameters.Add(paramUsername);

                var paramPassword = comm.CreateParameter();
                paramPassword.ParameterName = "@password";
                paramPassword.Value = password;
                comm.Parameters.Add(paramPassword);

                using(var result = comm.ExecuteReader())
                {
                    if(result.Read() && int.Parse(result[0].ToString()) > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public User Save(User e)
        {
            logger.Info("add user");
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
            logger.InfoFormat("delete user");
            IDbConnection connection = DButils.GetConnection(Properties);
            using (var comm = connection.CreateCommand())
            {
                logger.InfoFormat("delete user");
                comm.CommandText = "DELETE FROM users WHERE userid=@userid";
                IDbDataParameter paramID = comm.CreateParameter();
                paramID.ParameterName = "@userid";
                paramID.Value = e.ID;
                comm.Parameters.Add(paramID);
                var dataR = comm.ExecuteNonQuery();
                if (dataR == 0)
                {
                    throw new Exception("can't delete user");
                }
            }

            return false;
        }

        public User Update(User e)
        {
            IDbConnection connection = DButils.GetConnection(Properties);

            using (var comm = connection.CreateCommand())
            {
                comm.CommandText = "UPDATE users SET username=@username, password=@password WHERE userid=@userid";

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
                    throw new Exception("No user updated!\n");
                }
            }

            return e;
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
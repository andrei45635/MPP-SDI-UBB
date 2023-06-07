using System;
using System.Collections.Generic;
using System.Data;
using contest.domain;
using contest.domain.enums;
using contest.utils;
using log4net;
using Type = contest.domain.enums.Type;

namespace contest.repo.tasks
{
    public class TaskDBRepository : IRepository<int, Task>
    {
        private static readonly ILog logger = LogManager.GetLogger("TaskDBRepository");

        private IDictionary<string, string> Properties;

        public TaskDBRepository(IDictionary<string, string> properties)
        {
            logger.Info("Creating Task DB Repo");
            Properties = properties;
        }

        public IEnumerable<Task> FindAll()
        {
            IDbConnection dbConnection = DButils.GetConnection(Properties);
            IList<Task> tasks = new List<Task>();
            
            using (var comm = dbConnection.CreateCommand())
            {
                logger.InfoFormat("all tasks");
                comm.CommandText = "SELECT id, type, agegroup FROM users";
                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int idT = dataR.GetInt32(0);
                        Type type = (Type)Enum.Parse(typeof(Type), dataR.GetString(1));
                        AgeGroup ageGroup = (AgeGroup)Enum.Parse(typeof(AgeGroup), dataR.GetString(2));
                        Task task = new Task(idT, type, ageGroup);
                        tasks.Add(task);
                    }
                }
            }

            return tasks;
        }

        public Task Save(Task e)
        {
            var dbConnection = DButils.GetConnection(Properties);

            using (var comm = dbConnection.CreateCommand())
            {
                logger.InfoFormat("saving task {0}", e);
                comm.CommandText = "INSERT INTO tasks VALUES (@taskid, @type, @agegroup)";
                
                var paramID = comm.CreateParameter();
                paramID.ParameterName = "@taskid";
                paramID.Value = e.ID;
                comm.Parameters.Add(paramID);

                var paramType = comm.CreateParameter();
                paramType.ParameterName = "@type";
                paramType.Value = e.Type.ToString();
                comm.Parameters.Add(paramType);

                var paramAgeGroup = comm.CreateParameter();
                paramAgeGroup.ParameterName = "@agegroup";
                paramAgeGroup.Value = e.AgeGroup.ToString();
                comm.Parameters.Add(paramAgeGroup);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                {
                    logger.InfoFormat("Error DB");
                    throw new Exception("No task added!\n");
                }
            }

            return e;
        }

        public bool Delete(Task e)
        {
            throw new NotImplementedException();
        }

        public Task Update(Task e)
        {
            throw new NotImplementedException();
        }
        
        public int Size()
        {
            IDbConnection dbConnection = DButils.GetConnection(Properties);
            
            using (var comm = dbConnection.CreateCommand())
            {
                comm.CommandText = "SELECT count(*) FROM tasks";
                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        logger.InfoFormat("count(*) tasks");
                        return dataR.GetInt32(0);
                    }
                }
            }

            return 0;
        }
    }
}
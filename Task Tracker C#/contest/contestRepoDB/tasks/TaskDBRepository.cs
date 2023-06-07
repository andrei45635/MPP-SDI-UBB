using System;
using System.Collections.Generic;
using System.Data;
using contestDomain;
using contestDomain.enums;
using log4net;
using Type = contestDomain.enums.Type;
using Task = contestDomain.Task;

namespace contestRepoDB.tasks
{
    public class TaskDBRepository : TaskRepo
    {
        private static readonly ILog logger = LogManager.GetLogger("TaskDBRepository");

        private IDictionary<string, string> Properties;

        public TaskDBRepository(IDictionary<string, string> properties)
        {
            logger.Info("Creating Task DB Repo");
            Properties = properties;
        }

        public IEnumerable<Task> FindTaskByAge(AgeGroup age)
        {
            IDbConnection dbConnection = DButils.GetConnection(Properties);
            IList<Task> tasks = new List<Task>();

            using (var comm = dbConnection.CreateCommand())
            {
                logger.InfoFormat("all tasks with a given age");
                comm.CommandText = "SELECT * FROM tasks WHERE age=@age";
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

        public IEnumerable<Task> FindTaskByType(Type type)
        {
            IDbConnection dbConnection = DButils.GetConnection(Properties);
            IList<Task> tasks = new List<Task>();

            using (var comm = dbConnection.CreateCommand())
            {
                logger.InfoFormat("all tasks with a given type");
                comm.CommandText = "SELECT * FROM tasks WHERE type=@type";
                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int idT = dataR.GetInt32(0);
                        Type typeD = (Type)Enum.Parse(typeof(Type), dataR.GetString(1));
                        AgeGroup ageGroup = (AgeGroup)Enum.Parse(typeof(AgeGroup), dataR.GetString(2));
                        Task task = new Task(idT, typeD, ageGroup);
                        tasks.Add(task);
                    }
                }
            }
            return tasks;
        }

        public IEnumerable<Task> FindAll()
        {
            IDbConnection dbConnection = DButils.GetConnection(Properties);
            IList<Task> tasks = new List<Task>();
            
            using (var comm = dbConnection.CreateCommand())
            {
                logger.InfoFormat("all tasks");
                comm.CommandText = "SELECT taskid, type, agegroup FROM tasks";
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
            IDbConnection connection = DButils.GetConnection(Properties);
            using (var comm = connection.CreateCommand())
            {
                comm.CommandText = "DELETE FROM tasks WHERE taskid=@taskid";
                IDbDataParameter paramID = comm.CreateParameter();
                paramID.ParameterName = "@taskid";
                paramID.Value = e.ID;
                comm.Parameters.Add(paramID);
                var dataR = comm.ExecuteNonQuery();
                if (dataR == 0)
                {
                    throw new Exception("can't delete task");
                }
            }

            return false;
        }

        public Task Update(Task e)
        {
            IDbConnection connection = DButils.GetConnection(Properties);

            using (var comm = connection.CreateCommand())
            {
                comm.CommandText = "UPDATE tasks SET type=@type, agegroup=@agegroup WHERE taskid=@taskid";
                
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
                    throw new Exception("No task updated!\n");
                }
            }

            return e;
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

        public int CountTasksByAgeAndType(Type type, AgeGroup ageGroup)
        {
            IDbConnection dbConnection = DButils.GetConnection(Properties);
            string query = "SELECT COUNT(*) FROM tasks t INNER JOIN participantstasks pt ON pt.taskID2 = t.taskid OR pt.taskID1 = t.taskid\n" +
                "                             INNER JOIN participants p on p.participantid = pt.participantID\n" +
                " WHERE type=? AND agegroup=?";
            using(var cmd =  dbConnection.CreateCommand())
            {
                cmd.CommandText = query;

                var paramType = cmd.CreateParameter();
                paramType.ParameterName = "@type";
                paramType.Value = type.ToString();
                cmd.Parameters.Add(paramType);

                var paramAgeGroup = cmd.CreateParameter();
                paramAgeGroup.ParameterName = "@agegroup";
                paramAgeGroup.Value = ageGroup.ToString();
                cmd.Parameters.Add(paramAgeGroup);

                using(var dataR = cmd.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        return dataR.GetInt32(0);
                    }
                }
            }
            return 0;
        }
    }
}
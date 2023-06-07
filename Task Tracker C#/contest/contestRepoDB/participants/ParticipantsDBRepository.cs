using System;
using System.Collections.Generic;
using System.Data;
using contestDomain;
using contestDomain.enums;
using Type = contestDomain.enums.Type;
using log4net;

namespace contestRepoDB.participants
{
    public class ParticipantsDBRepository : ParticipantsRepo
    {
        private static readonly ILog logger = LogManager.GetLogger("ParticipantsDBRepository");

        private IDictionary<string, string> Properties;

        public ParticipantsDBRepository(IDictionary<string, string> properties)
        {
            logger.Info("Creating Participants DB Repo");
            Properties = properties;
        }

        public Participant FindById(int id)
        {
            return null;
        }

        public List<Participant> FindParticipantsByAgeAndType(Type type, AgeGroup ageGroup)
        {
            IDbConnection dbConnection = DButils.GetConnection(Properties);
            string query = "select p.participantid, p.name, p.age from participants p INNER JOIN participantstasks pt on p.participantid=pt.participantID INNER JOIN tasks t on pt.taskID1=t.taskid OR pt.taskID2 = t.taskid where t.type=@type AND t.agegroup=@agegroup";
            List<Participant> participants = new List<Participant>();

            using(var cmd = dbConnection.CreateCommand())
            {
                logger.InfoFormat("Searching participants with a given age and type of task");
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
                    while (dataR.Read())
                    {
                        var idP = dataR.GetInt32(0);
                        var name = dataR.GetString(1);
                        var age = dataR.GetInt32(2);
                        var participant = new Participant(idP, name, age);
                        participants.Add(participant);
                    }
                }
            }

            return participants;
        }

        public IEnumerable<Participant> FindAll()
        {
            IDbConnection dbConnection = DButils.GetConnection(Properties);
            IList<Participant> participants = new List<Participant>();
            
            using (var comm = dbConnection.CreateCommand())
            {
                logger.InfoFormat("all participants");
                comm.CommandText = "SELECT participantid, name, age FROM participants";
                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        var idP = dataR.GetInt32(0);
                        var name = dataR.GetString(1);
                        var age = dataR.GetInt32(2);
                        var participant = new Participant(idP, name, age);
                        participants.Add(participant);
                    }
                }
            }

            return participants;
        }

        public Participant Save(Participant e)
        {
            var dbConnection = DButils.GetConnection(Properties);

            using (var comm = dbConnection.CreateCommand())
            {
                logger.InfoFormat("saving participant {0}", e);
                comm.CommandText = "INSERT INTO participants VALUES (@participantid, @name, @age)";
                
                var paramID = comm.CreateParameter();
                paramID.ParameterName = "@participantid";
                paramID.Value = e.ID;
                comm.Parameters.Add(paramID);

                var paramName = comm.CreateParameter();
                paramName.ParameterName = "@name";
                paramName.Value = e.Name;
                comm.Parameters.Add(paramName);

                var paramAge = comm.CreateParameter();
                paramAge.ParameterName = "@age";
                paramAge.Value = e.Age;
                comm.Parameters.Add(paramAge);

                var result = comm.ExecuteNonQuery();
                if (result != 0) return e;
                logger.InfoFormat("Error DB");
                throw new Exception("No participant added!\n");
            }
        }

        public bool Delete(Participant e)
        {
            IDbConnection connection = DButils.GetConnection(Properties);
            using (var comm = connection.CreateCommand())
            {
                comm.CommandText = "DELETE FROM participants WHERE participantid=@participantid";
                IDbDataParameter paramID = comm.CreateParameter();
                paramID.ParameterName = "@participantid";
                paramID.Value = e.ID;
                comm.Parameters.Add(paramID);
                var dataR = comm.ExecuteNonQuery();
                if (dataR == 0)
                {
                    throw new Exception("can't delete participant");
                }
            }

            return false;
        }

        public Participant Update(Participant e)
        {
            IDbConnection connection = DButils.GetConnection(Properties);

            using (var comm = connection.CreateCommand())
            {
                logger.InfoFormat("updating participant {0}", e);
                comm.CommandText = "UPDATE participants SET name=@name, age=@age WHERE participantid=@participantid";
                
                var paramID = comm.CreateParameter();
                paramID.ParameterName = "@participantid";
                paramID.Value = e.ID;
                comm.Parameters.Add(paramID);

                var paramName = comm.CreateParameter();
                paramName.ParameterName = "@name";
                paramName.Value = e.Name;
                comm.Parameters.Add(paramName);

                var paramAge = comm.CreateParameter();
                paramAge.ParameterName = "@age";
                paramAge.Value = e.Age;
                comm.Parameters.Add(paramAge);

                var result = comm.ExecuteNonQuery();
                if (result != 0) return e;
                logger.InfoFormat("Error DB");
                throw new Exception("No participant updated!\n");
            }

            return e;
        }

        public int Size()
        {
            IDbConnection dbConnection = DButils.GetConnection(Properties);
            
            using (var comm = dbConnection.CreateCommand())
            {
                comm.CommandText = "SELECT count(*) FROM participants";
                using (var dataR = comm.ExecuteReader())
                {
                    if (!dataR.Read()) return 0;
                    logger.InfoFormat("count(*) participants");
                    return dataR.GetInt32(0);
                }
            }
        }
    }
}
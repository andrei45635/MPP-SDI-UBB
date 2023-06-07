using System;
using System.Collections.Generic;
using System.Data;
using contest.domain;
using contest.utils;
using log4net;

namespace contest.repo.participants
{
    public class ParticipantsDBRepository : IRepository<int, Participant>
    {
        private static readonly ILog logger = LogManager.GetLogger("ParticipantsDBRepository");

        private IDictionary<string, string> Properties;

        public ParticipantsDBRepository(IDictionary<string, string> properties)
        {
            logger.Info("Creating Participants DB Repo");
            Properties = properties;
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
            throw new System.NotImplementedException();
        }

        public Participant Update(Participant e)
        {
            throw new System.NotImplementedException();
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
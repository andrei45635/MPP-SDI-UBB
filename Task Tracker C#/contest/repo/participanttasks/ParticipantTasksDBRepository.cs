using System;
using System.Collections.Generic;
using System.Data;
using contest.domain;
using contest.domain.enums;
using contest.utils;
using log4net;
using Type = System.Type;

namespace contest.repo.participanttasks
{
    public class ParticipantTasksRepository : IRepository<int, ParticipantTasks>
    {
        private static readonly ILog logger = LogManager.GetLogger("ParticipantTasksDBRepository");

        private IDictionary<string, string> Properties;

        public ParticipantTasksRepository(IDictionary<string, string> properties)
        {
            logger.InfoFormat("Participant Tasks DB Repository");
            Properties = properties;
        }

        public IEnumerable<ParticipantTasks> FindAll()
        {
            IDbConnection dbConnection = DButils.GetConnection(Properties);
            IList<ParticipantTasks> ptasks = new List<ParticipantTasks>();
            
            using (var comm = dbConnection.CreateCommand())
            {
                logger.InfoFormat("all participant tasks");
                comm.CommandText = "SELECT participantstasksid, participantid, taskid1, taskid2 FROM participantstasks";
                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int idPT = dataR.GetInt32(0);
                        int pID = dataR.GetInt32(1);
                        int t1ID = dataR.GetInt32(2);
                        int t2ID = dataR.GetInt32(3);
                        var PT = new ParticipantTasks(idPT, pID, t1ID, t2ID);
                        ptasks.Add(PT);
                    }
                }
            }

            return ptasks;
        }

        public ParticipantTasks Save(ParticipantTasks e)
        {
            var dbConnection = DButils.GetConnection(Properties);

            using (var comm = dbConnection.CreateCommand())
            {
                logger.InfoFormat("saving task {0}", e);
                comm.CommandText = "INSERT INTO participantstasks VALUES (@participantstaksid, @participantid, @taskid1, @taskid2)";
                
                var paramID = comm.CreateParameter();
                paramID.ParameterName = "@participantstaksid";
                paramID.Value = e.ID;
                comm.Parameters.Add(paramID);

                var paramParticipantID = comm.CreateParameter();
                paramParticipantID.ParameterName = "@participantid";
                paramParticipantID.Value = e.ParticipantID;
                comm.Parameters.Add(paramParticipantID);
                
                var paramTaskID1 = comm.CreateParameter();
                paramTaskID1.ParameterName = "@taskid1";
                paramTaskID1.Value = e.Task1ID;
                comm.Parameters.Add(paramTaskID1);
                
                var paramTaskID2 = comm.CreateParameter();
                paramTaskID2.ParameterName = "@taskid2";
                paramTaskID2.Value = e.Task2ID;
                comm.Parameters.Add(paramTaskID2);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                {
                    logger.InfoFormat("Error DB");
                    throw new Exception("No participant task added!\n");
                }
            }

            return e;
        }

        public bool Delete(ParticipantTasks e)
        {
            throw new NotImplementedException();
        }

        public ParticipantTasks Update(ParticipantTasks e)
        {
            throw new NotImplementedException();
        }
        
        public int Size()
        {
            IDbConnection dbConnection = DButils.GetConnection(Properties);
            
            using (var comm = dbConnection.CreateCommand())
            {
                comm.CommandText = "SELECT count(*) FROM participantstasks";
                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        logger.InfoFormat("count(*) participantstasks");
                        return dataR.GetInt32(0);
                    }
                }
            }

            return 0;
        }
    }
}
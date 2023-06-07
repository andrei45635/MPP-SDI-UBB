using Com.Example.Networking;
using contestService;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Type = contestDomain.enums.Type;

namespace contestNetworking
{
    public class ContestProtoWorker : IContestObserver
    {

        private IContestService server;
        private TcpClient connection;

        private NetworkStream stream;
        private volatile bool connected;

        public ContestProtoWorker(IContestService server, TcpClient connection)
        {
            this.server = server;
            this.connection = connection;
            try
            {

                stream = connection.GetStream();
                connected = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public virtual void run()
        {
            while (connected)
            {
                try
                {
                    ContestRequest request = ContestRequest.Parser.ParseDelimitedFrom(stream);
                    ContestResponse response = HandleRequest(request);
                    if (response != null)
                    {
                        SendResponse(response);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

                try
                {
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            try
            {
                stream.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error " + e);
            }
        }

        private ContestResponse HandleRequest(ContestRequest request)
        {
            ContestResponse response = null;
            ContestRequest.Types.Req requestType = request.Req;
            switch (requestType)
            {
                case ContestRequest.Types.Req.Login:
                    {
                        Console.WriteLine("Logging in...");
                        contestDomain.User user = ProtoUtils.GetUser(request);
                        Console.WriteLine("user " + user);
                        try
                        {
                            lock (server)
                            {
                                Console.WriteLine("before login");
                                server.Login(user, this);
                                Console.WriteLine("Logged in the suer");
                            }
                            return ProtoUtils.CreateOKResp();
                        }
                        catch (ContestException cx)
                        {
                            connected = false;
                            return ProtoUtils.CreateErrorResponse(cx.Message);
                        }
                    }
                case ContestRequest.Types.Req.Logout:
                    {
                        Console.WriteLine("Logging out...");
                        contestDomain.User user = ProtoUtils.GetUser(request);
                        try
                        {
                            lock (server)
                            {
                                server.Logout(user, this);
                            }
                            connected = false;
                            return ProtoUtils.CreateOKResp();
                        }
                        catch (ContestException cx)
                        {
                            return ProtoUtils.CreateErrorResponse(cx.Message);
                        }
                    }
                case ContestRequest.Types.Req.AddParticipantExperimental:
                    {
                        Console.WriteLine("Adding participant...");
                        contestDomain.ParticipantTasks participant = ProtoUtils.GetPT(request);
                        try
                        {
                            lock (server)
                            {
                                server.Register(participant.Name, participant.Age, participant.Type1, participant.Type2);
                            }
                            return ProtoUtils.CreateRegisterResponse();
                        }
                        catch (ContestException cx)
                        {
                            return ProtoUtils.CreateErrorResponse(cx.Message);
                        }
                    }
                case ContestRequest.Types.Req.GetAllTasksExperimental:
                    {
                        Console.WriteLine("Getting all tasks...");
                        List<contestDomain.dto.TaskDTO> tk = new List<contestDomain.dto.TaskDTO>();
                        try
                        {
                            lock (server)
                            {
                                tk = server.GetTasksEnrolled();
                            }
                            return ProtoUtils.CreateGetTasksEnrolledResponse(ProtoUtils.GetTasksConverted(tk));
                        }
                        catch (ContestException cx)
                        {
                            return ProtoUtils.CreateErrorResponse(cx.Message);
                        }
                    }
                case ContestRequest.Types.Req.Filter:
                    {
                        Console.WriteLine("Filtering participants...");
                        var filters = ProtoUtils.GetFilters(request);
                        List<contestDomain.Participant> filteredParticipants = new List<contestDomain.Participant>();
                        try
                        {
                            lock (server)
                            {
                                filteredParticipants = server.GetFilteredParticipants((Type)Enum.Parse(typeof(Type), filters[0]), (contestDomain.enums.AgeGroup)Enum.Parse(typeof(contestDomain.enums.AgeGroup), filters[1]));
                            }
                            return ProtoUtils.CreateGetFilteredParticipantsResponse(ProtoUtils.GetFilteredParticipantsDTO(filteredParticipants));
                        }
                        catch (ContestException cx)
                        {
                            return ProtoUtils.CreateErrorResponse(cx.Message);
                        }
                    }
            }
            return response;
        }

        private void SendResponse(ContestResponse response)
        {
            Console.WriteLine("Sending response " + response);
            lock (stream)
            {
                response.WriteDelimitedTo(stream);
                stream.Flush();
            }
            Console.WriteLine("Sent");
        }

        public void updateTaskList()
        {
            Console.WriteLine("Updating tables...");
            try
            {
                SendResponse(ProtoUtils.CreateUpdateResponse());
            }
            catch (ContestException cx)
            {
                Console.WriteLine(cx.Message);
            }
        }
    }
}

using Com.Example.Networking;
using contestService;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;


namespace contestNetworking
{
    public class ContestProtoProxy : IContestService
    {
        private string host;
        private int port;

        private IContestObserver client;

        private NetworkStream stream;

        private TcpClient connection;

        private Queue<ContestResponse> responses;
        private volatile bool finished;
        private EventWaitHandle _waitHandle;

        public ContestProtoProxy(string host, int port)
        {
            this.host = host;
            this.port = port;
            responses = new Queue<ContestResponse>();
        }

        private bool IsUpdate(ContestResponse response)
        {
            return response.Rep == ContestResponse.Types.Reply.Update;
        }

        private void HandleUpdate(ContestResponse response)
        {
            Console.WriteLine("Updating...");
            try
            {
                client.updateTaskList();
            }
            catch (ContestException cx)
            {
                Console.WriteLine(cx.StackTrace);
            }
        }

        private void StartReader()
        {
            Thread tw = new Thread(Run);
            tw.Start();
        }

        private void InitializeConnection()
        {
            try
            {
                connection = new TcpClient(host, port);
                stream = connection.GetStream();
                finished = false;
                _waitHandle = new AutoResetEvent(false);
                StartReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private void CloseConnection()
        {
            finished = true;
            try
            {
                stream.Close();
                connection.Close();
                _waitHandle.Close();
                client = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private void SendRequest(ContestRequest request)
        {
            try
            {
                request.WriteDelimitedTo(stream);
                stream.Flush();
            }
            catch (Exception e)
            {
                throw new ContestException("Error sending object " + e);
            }
        }

        private ContestResponse ReadResponse()
        {
            ContestResponse response = null;
            try
            {
                _waitHandle.WaitOne();
                lock (responses)
                {
                    response = responses.Dequeue();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return response;
        }

        public virtual void Run()
        {
            while (!finished)
            {
                try
                {
                    ContestResponse response = ContestResponse.Parser.ParseDelimitedFrom(stream);
                    Console.WriteLine("Response received " + response);
                    if (IsUpdate(response))
                    {
                        HandleUpdate(response);
                    }
                    else
                    {
                        lock (responses)
                        {
                            responses.Enqueue(response);
                        }
                        _waitHandle.Set();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Reading error " + e);
                }
            }
        }

        public virtual void Login(contestDomain.User user, IContestObserver obs)
        {
            InitializeConnection();
            Console.WriteLine("Login proxy");
            SendRequest(ProtoUtils.CreateLoginRequest(user));
            ContestResponse response = ReadResponse();
            Console.WriteLine("Login proxy after response");
            if (response.Rep == ContestResponse.Types.Reply.Ok)
            {
                this.client = obs;
                return;
            }
            if (response.Rep == ContestResponse.Types.Reply.Error)
            {
                string err = ProtoUtils.GetError(response);
                CloseConnection();
                throw new ContestException(err);
            }
        }

        public virtual void Logout(contestDomain.User user, IContestObserver observer)
        {
            SendRequest(ProtoUtils.CreateLogoutRequest(user));
            ContestResponse response = ReadResponse();
            CloseConnection();
            if (response.Rep == ContestResponse.Types.Reply.Error)
            {
                string err = ProtoUtils.GetError(response);
                throw new ContestException(err);
            }
        }

        private List<contestDomain.dto.TaskDTO> Model2ProtoMapper(List<TaskDTO> tasks)
        {
            List<contestDomain.dto.TaskDTO> res = new List<contestDomain.dto.TaskDTO>();
            foreach (var tk in tasks)
            {
                contestDomain.dto.TaskDTO taskDTO = new contestDomain.dto.TaskDTO(tk.TaskID, (contestDomain.enums.Type)Enum.Parse(typeof(contestDomain.enums.Type), tk.Type.ToString().ToUpper()), (contestDomain.enums.AgeGroup)Enum.Parse(typeof(contestDomain.enums.AgeGroup), tk.Age.ToString().ToUpper()), tk.Enrolled);
                res.Add(taskDTO);
            }
            return res;
        }

        public virtual List<contestDomain.dto.TaskDTO> GetTasksEnrolled()
        {
            Console.WriteLine("Requesting all the tasks...");
            Request request = new GetTasksEnrolled();
            SendRequest(ProtoUtils.CreateTasksEnrolledRequest());
            ContestResponse response = ReadResponse();
            Console.WriteLine("Finished reading the get all response...");
            if(response.Rep == ContestResponse.Types.Reply.GetAllTasksExperimental)
            {
                return Model2ProtoMapper(ProtoUtils.GetEnrolledTasks(response));
            }
            if (response.Rep == ContestResponse.Types.Reply.Error)
            {
                string err = ProtoUtils.GetError(response);
                throw new ContestException(err);
            }
            return null;
        }

        private List<contestDomain.Participant> Model2ProtoParticipantMapper(List<ParticipantDTO> parts)
        {
            List<contestDomain.Participant> res = new List<contestDomain.Participant>();
            foreach (var tk in parts)
            {
                contestDomain.Participant p = new contestDomain.Participant(new Random().Next(500, 1488), tk.Name, tk.Age);
                res.Add(p);
            }
            return res;
        }

        public virtual List<contestDomain.Participant> GetFilteredParticipants(contestDomain.enums.Type type, contestDomain.enums.AgeGroup age)
        {
            string[] filters = new string[2];
            filters[0] = type.ToString();
            filters[1] = age.ToString();
            Console.WriteLine("Requesting to filter the participants...");
            SendRequest(ProtoUtils.CreateGetFilteredParticipantsRequest(type, age));
            ContestResponse response = ReadResponse();
            Console.WriteLine("Finished reading the filter response...");
            if (response.Rep == ContestResponse.Types.Reply.Filter)
            {
                return Model2ProtoParticipantMapper(ProtoUtils.GetFilteredPartsDTO(response));
            }
            if (response.Rep == ContestResponse.Types.Reply.Error)
            {
                string err = ProtoUtils.GetError(response);
                throw new ContestException(err);
            }
            return null;
        }

        public int GetParticipantSize()
        {
            throw new NotImplementedException();
        }

        public void Register(string name, int age, contestDomain.enums.Type type1, contestDomain.enums.Type type2)
        {
            Console.WriteLine("Requesting to add a participant...");
            SendRequest(ProtoUtils.CreateRegisterRequest(name, age, type1, type2));
            ContestResponse response = ReadResponse();
            Console.WriteLine("Finished reading the add response...");
            if (response.Rep == ContestResponse.Types.Reply.Error)
            {
                string err = ProtoUtils.GetError(response);
                throw new ContestException(err);
            }
        }

        public List<contestDomain.Task> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        public bool CheckUserExists(string username, string password)
        {
            throw new NotImplementedException();
        }

        public contestDomain.User FindLoggedInUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public List<contestDomain.Task> GetAllTasksWithMaxEnrolled()
        {
            throw new NotImplementedException();
        }

        public int GetEnrolled(contestDomain.enums.AgeGroup ageGroup, contestDomain.enums.Type type)
        {
            throw new NotImplementedException();
        }

        public List<contestDomain.enums.AgeGroup> GetAllAgeGroups()
        {
            throw new NotImplementedException();
        }

        public List<contestDomain.enums.Type> GetAllTypes()
        {
            throw new NotImplementedException();
        }
    }
}

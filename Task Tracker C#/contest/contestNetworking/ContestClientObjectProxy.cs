using contestService;
using contestDomain;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using contestDomain.dto;
using Type = contestDomain.enums.Type;
using Task = contestDomain.Task;
using contestDomain.enums;

namespace contestNetworking
{
    public class ContestClientObjectProxy : IContestService
    {
        private string host;
        private int port;

        private IContestObserver client;

        private NetworkStream stream;

        private IFormatter formatter;
        private TcpClient connection;

        private Queue<Response> responses;
        private volatile bool finished;
        private EventWaitHandle _waitHandle;
        public ContestClientObjectProxy(string host, int port)
        {
            this.host = host;
            this.port = port;
            responses = new Queue<Response>();
        }

        public virtual void Run()
        {
            while (!finished)
            {
                try
                {
                    object response = formatter.Deserialize(stream); 
                    Console.WriteLine("Response received " + response);
                    if (response is UpdateResponse)
                    {
                        HandleUpdate((UpdateResponse)response);
                    }
                    else
                    {
                        lock (responses)
                        {
                            responses.Enqueue((Response)response);
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

        private void HandleUpdate(UpdateResponse response)
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
                formatter = new BinaryFormatter();
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

        private void SendRequest(Request request)
        {
            try
            {
                formatter.Serialize(stream, request);
                stream.Flush();
            }
            catch (Exception e)
            {
                throw new ContestException("Error sending object " + e);
            }
        }

        private Response ReadResponse()
        {
            Response response = null;
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

        public virtual void Login(User user, IContestObserver obs)
        {
            InitializeConnection();
            Console.WriteLine("Login proxy");
            Request request = new LoginRequest(user);
            SendRequest(request);
            Response response = ReadResponse();
            Console.WriteLine("Login proxy after response");
            if (response is OkResponse)
            {
                this.client = obs;
                return;
            }
            if (response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse)response;
                CloseConnection();
                throw new ContestException(err.Message);
            }
        }

        public virtual void Logout(User user, IContestObserver observer)
        {
            SendRequest(new LogoutRequest(user));
            Response response = ReadResponse();
            CloseConnection();
            if (response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse)response;
                throw new ContestException(err.Message);
            }
        }

        public virtual List<TaskDTO> GetTasksEnrolled()
        {
            Console.WriteLine("Requesting all the tasks...");
            Request request = new GetTasksEnrolled();
            SendRequest(request);
            Response response = ReadResponse();
            Console.WriteLine("Finished reading the get all response...");
            if (response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse)response;
                throw new ContestException(err.Message);
            }
            GetTasksEnrolledResponse resp = (GetTasksEnrolledResponse)response;
            return resp.Tasks;
        }

        public virtual List<Participant> GetFilteredParticipants(Type type, AgeGroup age)
        {
            string[] filters = new string[2];
            filters[0] = type.ToString();
            filters[1] = age.ToString();
            Console.WriteLine("Requesting to filter the participants...");
            Request request = new FilterParticipants(filters);
            SendRequest(request);
            Response response = ReadResponse();
            Console.WriteLine("Finished reading the filter response...");
            if (response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse)response;
                throw new ContestException(err.Message);
            }
            FilterResponse resp = (FilterResponse)response;
            return resp.Participants;
        }

        public virtual User FindLoggedInUser(string username, string password)
        {
            Console.WriteLine("Requesting to find the specified user...");
            Request request = new FindLoggedInUser(new User(new Random().Next(500, 900), username, password));
            SendRequest(request);
            Response response = ReadResponse();
            Console.WriteLine("Finished reading the search response...");
            if (response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse)response;
                throw new ContestException(err.Message);
            }
            FindLoggedInUserResponse resp = (FindLoggedInUserResponse)response;
            return resp.User;
        }

        public int GetParticipantSize()
        {
            throw new NotImplementedException();
        }

        public void Register(string name, int age, Type type1, Type type2)
        {
            Random rnd = new Random();
            Console.WriteLine("Requesting to add a participant...");
            Request request = new AddParticipant(new ParticipantTasks(rnd.Next(500, 1488), name, age, type1, type2));
            SendRequest(request);
            Response response = ReadResponse();
            Console.WriteLine("Finished reading the add response...");
            if (response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse)response;
                throw new ContestException(err.Message);
            }
        }

        public List<Task> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        public bool CheckUserExists(string username, string password)
        {
            throw new NotImplementedException();
        }

        public List<Task> GetAllTasksWithMaxEnrolled()
        {
            throw new NotImplementedException();
        }

        public int GetEnrolled(AgeGroup ageGroup, Type type)
        {
            throw new NotImplementedException();
        }

        public List<AgeGroup> GetAllAgeGroups()
        {
            Console.WriteLine("Requesting to get all the age groups...");
            Request request = new GetAllAgeGroups();
            SendRequest(request);
            Response response = ReadResponse();
            Console.WriteLine("Finished reading the search response for the age groups...");
            if (response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse)response;
                throw new ContestException(err.Message);
            }
            GetAllAgeGroupsResponse resp = (GetAllAgeGroupsResponse)response;
            return resp.AgeGroups;
        }

        public List<Type> GetAllTypes()
        {
            Console.WriteLine("Requesting to get all the types...");
            Request request = new GetAllTypes();
            SendRequest(request);
            Response response = ReadResponse();
            Console.WriteLine("Finished reading the search response for the types...");
            if (response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse)response;
                throw new ContestException(err.Message);
            }
            GetAllTypesResponse resp = (GetAllTypesResponse)response;
            return resp.Types;
        }
    }
}
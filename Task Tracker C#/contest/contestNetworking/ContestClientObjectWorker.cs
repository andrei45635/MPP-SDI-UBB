using contestService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using contestDomain;
using contestDomain.dto;
using Type = contestDomain.enums.Type;
using contestDomain.enums;

namespace contestNetworking
{
    public class ContestClientObjectWorker : IContestObserver
    {

        private IContestService server;
        private TcpClient connection;

        private NetworkStream stream;
        private IFormatter formatter;
        private volatile bool connected;

        public ContestClientObjectWorker(IContestService server, TcpClient connection)
        {
            this.server = server;
            this.connection = connection;
            try
            {

                stream = connection.GetStream();
                formatter = new BinaryFormatter();
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
                    object request = formatter.Deserialize(stream);
                    object response = handleRequest((Request)request);
                    if (response != null)
                    {
                        sendResponse((Response)response);
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

        private Response handleRequest(Request request)
        {
            Response response = null;
            if (request is LoginRequest)
            {
                Console.WriteLine("Logging in...");
                LoginRequest loginRequest = (LoginRequest)request;
                User user = loginRequest.User;
                Console.WriteLine("user " + user);
                try
                {
                    lock (server)
                    {
                        Console.WriteLine("before login");
                        server.Login(user, this);
                        Console.WriteLine("Logged in the suer");
                    }
                    return new OkResponse();
                }
                catch (ContestException cx)
                {
                    connected = false;
                    return new ErrorResponse(cx.Message);
                }
            }
            if (request is LogoutRequest)
            {
                Console.WriteLine("Logging out...");
                LogoutRequest logoutRequest = (LogoutRequest)request;
                User user = logoutRequest.User;
                try
                {
                    lock (server)
                    {
                        server.Logout(user, this);
                    }
                    connected = false;
                    return new OkResponse();
                }
                catch (ContestException cx)
                {
                    return new ErrorResponse(cx.Message);
                }
            }
            if (request is AddParticipant)
            {
                Console.WriteLine("Adding participant...");
                AddParticipant addParticipant = (AddParticipant)request;
                ParticipantTasks participant = addParticipant.ParticipantTasks;
                try
                {
                    lock (server)
                    {
                        server.Register(participant.Name, participant.Age, participant.Type1, participant.Type2);
                    }
                    return new OkResponse();
                }
                catch (ContestException cx)
                {
                    return new ErrorResponse(cx.Message);
                }
            }
            if (request is GetTasksEnrolled)
            {
                Console.WriteLine("Getting all tasks...");
                GetTasksEnrolled getEnrolled = (GetTasksEnrolled)request;
                List<TaskDTO> tk = new List<TaskDTO>();
                try
                {
                    lock (server)
                    {
                        tk = server.GetTasksEnrolled();
                    }
                    return new GetTasksEnrolledResponse(tk);
                }
                catch (ContestException cx)
                {
                    return new ErrorResponse(cx.Message);
                }
            }
            if (request is FilterParticipants)
            {
                Console.WriteLine("Filtering participants...");
                FilterParticipants fp = (FilterParticipants)request;
                var filters = fp.Filters;
                List<Participant> filteredParticipants = new List<Participant>();
                try
                {
                    lock (server)
                    {
                        filteredParticipants = server.GetFilteredParticipants((Type)Enum.Parse(typeof(Type), filters[0]), (AgeGroup)Enum.Parse(typeof(AgeGroup), filters[1]));
                    }
                    return new FilterResponse(filteredParticipants);
                }
                catch (ContestException cx)
                {
                    return new ErrorResponse(cx.Message);
                }
            }

            if (request is FindLoggedInUser)
            {
                Console.WriteLine("Finding the user...");
                FindLoggedInUser user = (FindLoggedInUser)request;
                
                User foundUser = null;
                try
                {
                    lock (server)
                    {
                        foundUser = server.FindLoggedInUser(user.User.Username, user.User.Password);
                    }
                    return new FindLoggedInUserResponse(foundUser);
                }
                catch (ContestException cx)
                {
                    return new ErrorResponse(cx.Message);
                }
            }
            if(request is GetAllTypes)
            {
                Console.WriteLine("Getting all the types...");
                GetAllTypes getAllTypes = (GetAllTypes)request;
                List<Type> types = new List<Type>();
                try
                {
                    lock (server)
                    {
                        types = server.GetAllTypes();
                    }
                    return new GetAllTypesResponse(types);
                }
                 catch(ContestException cx)
                {
                    return new ErrorResponse(cx.Message);
                }
            }
            if (request is GetAllAgeGroups)
            {
                Console.WriteLine("Getting all the age groups...");
                GetAllAgeGroups getAllAgeGroups = (GetAllAgeGroups)request;
                List<AgeGroup> ageGroups = new List<AgeGroup>();
                try
                {
                    lock (server)
                    {
                        ageGroups = server.GetAllAgeGroups();
                    }
                    return new GetAllAgeGroupsResponse(ageGroups);
                }
                catch (ContestException cx)
                {
                    return new ErrorResponse(cx.Message);
                }
            }
            return response;
        }

        private void sendResponse(Response response)
        {
            Console.WriteLine("Sending response " + response);
            lock (stream)
            {
                formatter.Serialize(stream, response);
                stream.Flush();
            }
            Console.WriteLine("Sent");
        }

        public void updateTaskList()
        {
            Console.WriteLine("Updating tables...");
            try
            {
                sendResponse(new UpdateResponse());
            }
            catch (ContestException cx)
            {
                Console.WriteLine(cx.Message);
            }
        }
    }
}

using contestRepoDB.participants;
using contestRepoDB.participanttasks;
using contestRepoDB.users;
using contestRepoDB.tasks;

using System.Configuration;
using contestNetworking;
using System.Net.Sockets;
using contestService;

namespace contestServer
{
    public class StartObjectServer
    {
        public static void Main(string[] args)
        {
            IDictionary<String, string> props = new SortedList<String, String>();
            props.Add("ConnectionString", GetConnectionStringByName("contest.db"));

            ParticipantsRepo _participantsRepo = new ParticipantsDBRepository(props);
            ParticipantTasksRepo _participantTasksRepo = new ParticipantTasksDBRepository(props);
            TaskRepo _tasksRepo = new TaskDBRepository(props);
            UserRepo _userRepo = new UserDBRepository(props);
            IContestService serviceImpl = new ContestServerImpl(_participantsRepo, _participantTasksRepo, _tasksRepo, _userRepo);

            //SerialContestServer server = new SerialContestServer("127.0.0.1", 55599, serviceImpl);
            ProtoServer server = new ProtoServer("127.0.0.1", 55588, serviceImpl);
            //serviceImpl.Login(new contestDomain.User(-1241, "fff", "fasa"), null);
            server.Start();
            //ConnectToServer();
            Console.WriteLine("Server started ...");
            Console.ReadLine();
        }

        public class SerialContestServer : ConcurrentServer
        {
            private IContestService server;
            private ContestClientObjectWorker worker;
            public SerialContestServer(string host, int port, IContestService server) : base(host, port)
            {
                this.server = server;
                Console.WriteLine("SerialContestServer...");
            }
            protected override Thread createWorker(TcpClient client)
            {
                worker = new ContestClientObjectWorker(server, client);
                return new Thread(new ThreadStart(worker.run));
            }
        }

        public class ProtoServer : ConcurrentServer
        {
            private IContestService server;
            private ContestProtoWorker worker;
            public ProtoServer(string host, int port, IContestService server) : base(host, port)
            {
                this.server = server;
                Console.WriteLine("ProtoChatServer...");
            }
            protected override Thread createWorker(TcpClient client)
            {
                worker = new ContestProtoWorker(server, client);
                return new Thread(new ThreadStart(worker.run));
            }
        }

        static string GetConnectionStringByName(string name)
        {
            // Assume failure.
            string returnValue = null;

            // Look for the name in the connectionStrings section.
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];

            // If found, return the connection string.
            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }

        static void ConnectToServer()
        {
            var client = new TcpClient("127.0.0.1", 55599);

        }
    }
}
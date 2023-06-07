using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using contestDomain;
using contestDomain.dto;
using Type = contestDomain.enums.Type;
using contestService;
using contestDomain.enums;

namespace contestClientIDK
{
    public class ContestClientController : IContestObserver
    {

        public event EventHandler<ContestEventArgs> updateEvent;
        private readonly IContestService server;
        private User loggedInUser;

        public ContestClientController(IContestService server)
        {
            this.server = server;
            this.loggedInUser = null;
        }

        public void Login(string username, string password)
        {
            //User user = server.FindLoggedInUser(username, password);
            User user = new User(new Random().Next(500,1488), username, password);
            try
            {
                server.Login(user, this);
                this.loggedInUser = user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Logout()
        {
            server.Logout(loggedInUser, this);
            loggedInUser = null;
        }

        public List<TaskDTO> GetAllTasks()
        {
            return server.GetTasksEnrolled();
        }

        public void Register(string name, int age, Type type1, Type type2)
        {
            server.Register(name, age, type1, type2);
        }

        public List<Participant> FilterParticipants(Type type, AgeGroup age)
        {
            return server.GetFilteredParticipants(type, age);
        }

        public User FindLoggedUser(string username, string password)
        {
            return server.FindLoggedInUser(username, password);
        }

        public List<Type> GetAllTypes()
        {
            return server.GetAllTypes();
        }

        public List<AgeGroup> GetAllAgeGroups()
        {
            var ageGroups = server.GetAllAgeGroups();
            List<AgeGroup> allAges = new List<AgeGroup>();
            allAges = ageGroups.ToList();
            return allAges;
        }

        public void updateTaskList()
        {
            Console.WriteLine("Updating");
            ContestEventArgs clientArgs = new ContestEventArgs(ContestClientEvent.Update, null);
            OnUserEvent(clientArgs);
        }

        protected virtual void OnUserEvent(ContestEventArgs ca)
        {
            if (updateEvent == null) return;
            updateEvent(this, ca);
            Console.WriteLine("Update Event called");
        }
    }
}

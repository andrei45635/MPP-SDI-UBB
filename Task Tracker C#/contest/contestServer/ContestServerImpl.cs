using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using contestDomain;
using contestDomain.dto;
using contestDomain.enums;
using contestService;
using contestRepoDB.participants;
using contestRepoDB.participanttasks;
using contestRepoDB.users;
using contestRepoDB.tasks;
using MyTask = contestDomain.Task;
using Task = System.Threading.Tasks.Task;
using Type = contestDomain.enums.Type;

namespace contestServer
{
    public class ContestServerImpl : IContestService
    {
        private ParticipantTasksRepo _participantTasksRepo;
        private ParticipantsRepo _participantsRepo;
        private TaskRepo _tasksRepo;
        private UserRepo _userRepo;
        private readonly IDictionary<String, IContestObserver> loggedClients;

        public ContestServerImpl(ParticipantsRepo participantsRepo, ParticipantTasksRepo participantTasksRepo, TaskRepo tasksRepo, UserRepo userRepo)
        {
            _participantsRepo = participantsRepo;
            _participantTasksRepo = participantTasksRepo;
            _tasksRepo = tasksRepo;
            _userRepo = userRepo;
            loggedClients = new Dictionary<String, IContestObserver>();
        }

        private void notifyObservers()
        {
            foreach(var c in loggedClients.Keys)
            {
                if (loggedClients.ContainsKey(c))
                {
                    var observer = loggedClients[c];
                    Task.Run(() => observer.updateTaskList());
                }
            }
        }

        public List<Type> GetAllTypes()
        {
            HashSet<Type> types = new HashSet<Type>();
            foreach (var tk in _tasksRepo.FindAll())
            {
                types.Add(tk.Type);
            }
            List<Type> allTypes = new List<Type>();
            allTypes = types.ToList();
            return allTypes;
        }

        public List<AgeGroup> GetAllAgeGroups()
        {
            HashSet<AgeGroup> ages = new HashSet<AgeGroup>();
            foreach (var tk in _tasksRepo.FindAll())
            {
                ages.Add(tk.AgeGroup);
            }
            List<AgeGroup> ageGroups = new List<AgeGroup>();
            ageGroups = ages.ToList();
            return ageGroups;
        }

        public List<Participant> GetFilteredParticipants(contestDomain.enums.Type type, AgeGroup ageGroup)
        {
            return _participantsRepo.FindParticipantsByAgeAndType(type, ageGroup);
        }

        public int GetParticipantSize()
        {
            return _participantsRepo.Size();
        }

        public void Register(string name, int age, contestDomain.enums.Type type1, contestDomain.enums.Type type2)
        {
            Participant participant = new Participant(this.GetParticipantSize() + 2, name, age);
            MyTask task1 = null, task2 = null;

            if (age > 6 && age <= 9)
            {
                task1 = new MyTask(_tasksRepo.Size() + 1, type1, AgeGroup.YOUNG);
                task2 = new MyTask(_tasksRepo.Size() + 2, type2, AgeGroup.YOUNG);
            }
            else if (age >= 10 && age <= 11)
            {
                task1 = new MyTask(_tasksRepo.Size() + 1, type1, AgeGroup.PRETEEN);
                task2 = new MyTask(_tasksRepo.Size() + 2, type2, AgeGroup.PRETEEN);
            }
            else if (age >= 12 && age < 15)
            {
                task1 = new MyTask(_tasksRepo.Size() + 1, type1, AgeGroup.TEEN);
                task2 = new MyTask(_tasksRepo.Size() + 2, type2, AgeGroup.TEEN);
            }

            ParticipantTasks participantTask = new ParticipantTasks(_participantTasksRepo.Size() + 2, participant.ID, task1.ID, task2.ID);
            _participantsRepo.Save(participant);
            _tasksRepo.Save(task1);
            _tasksRepo.Save(task2);
            _participantTasksRepo.Save(participantTask);
            notifyObservers();
        }

        public List<MyTask> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        public bool CheckUserExists(string username, string password)
        {
            throw new NotImplementedException();
        }

        public User FindLoggedInUser(string username, string password)
        {
            return _userRepo.FindLoggedInUser(username, password);
        }

        private User FindUser(string username, string password)
        {
            foreach (var u in _userRepo.FindAll())
            {
                if (u.Username == username && u.Password == password)
                {
                    return u;
                }
            }
            return null;
        }

        public void Login(User user, IContestObserver client)
        {
            Console.WriteLine("before find user");
            //User loggedUser = FindUser(user.Username, user.Password);
            User loggedUser = _userRepo.FindLoggedInUser(user.Username, user.Password);
            Console.WriteLine("after find user");
            if (loggedUser != null)
            {
                if (loggedClients.ContainsKey(user.Username)) throw new ContestException("User already logged in.");
                loggedClients[user.Username] = client;
                Console.WriteLine("usser logged in");
            }
            else
                throw new ContestException("Authentication failed.");
        }

        public void Logout(User user, IContestObserver client)
        {
            IContestObserver localClient = loggedClients[user.Username];
            if (localClient == null) throw new ContestException("User " + user.Username + " is not logged in.");
            loggedClients.Remove(user.Username);
        }

        public List<MyTask> GetAllTasksWithMaxEnrolled()
        {
            throw new NotImplementedException();
        }

        public int GetEnrolled(AgeGroup ageGroup, contestDomain.enums.Type type)
        {
            throw new NotImplementedException();
        }

        public List<TaskDTO> GetTasksEnrolled()
        {
            List<TaskDTO> allOfThem = new List<TaskDTO>();
            foreach (var tk in _tasksRepo.FindAll())
            {
                allOfThem.Add(new TaskDTO(tk.ID, tk.Type, tk.AgeGroup, _tasksRepo.CountTasksByAgeAndType(tk.Type, tk.AgeGroup)));
            }
            return allOfThem;
        }
    }
}

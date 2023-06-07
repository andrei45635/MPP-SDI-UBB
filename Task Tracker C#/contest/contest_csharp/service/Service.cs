using contest_csharp.repo;
using contest_csharp.domain;
using Task = contest_csharp.domain.Task;
using Type = contest_csharp.domain.enums.Type;
using contest_csharp.domain.enums;
using contest_csharp.repo.users;
using System.Collections.Generic;
using System.Linq;
using contest_csharp.repo.participants;
using contest_csharp.repo.tasks;
using contest_csharp.repo.participanttasks;

namespace contest_csharp.service
{
    public class Service
    {
        ParticipantTasksRepo _participantTasksRepo;
        ParticipantsRepo _participantsRepo;
        TaskRepo _tasksRepo;
        UserRepo _userRepo;

        public Service(ParticipantsRepo participantsRepo, ParticipantTasksRepo participantTasksRepo, TaskRepo tasksRepo, UserRepo userRepo)
        {
            _participantsRepo = participantsRepo;
            _participantTasksRepo = participantTasksRepo;
            _tasksRepo = tasksRepo;
            _userRepo = userRepo;
        }

        public int GetParticipantsSize()
        {
            return _participantsRepo.Size();
        }

        public void RegisterUser(string name, int age, Type type1, Type type2)
        {
            Participant participant = new Participant(GetParticipantsSize() + 1, name, age);
            Task task1 = null, task2 = null;

            if (age > 6 && age <= 9)
            {
                task1 = new Task(_tasksRepo.Size() + 1, type1, AgeGroup.YOUNG);
                task2 = new Task(_tasksRepo.Size() + 2, type2, AgeGroup.YOUNG);
            }
            else if (age >= 10 && age <= 11)
            {
                task1 = new Task(_tasksRepo.Size() + 1, type1, AgeGroup.PRETEEN);
                task2 = new Task(_tasksRepo.Size() + 2, type2, AgeGroup.PRETEEN);
            }
            else if (age >= 12 && age < 15)
            {
                task1 = new Task(_tasksRepo.Size() + 1, type1, AgeGroup.TEEN);
                task2 = new Task(_tasksRepo.Size() + 2, type2, AgeGroup.TEEN);
            }

            ParticipantTasks participantTask = new ParticipantTasks(_participantTasksRepo.Size() + 1, participant.ID, task1.ID, task2.ID);
            _participantsRepo.Save(participant);
            _tasksRepo.Save(task1);
            _tasksRepo.Save(task2);
            _participantTasksRepo.Save(participantTask);
        }

        public User FindLoggedInUser(string username, string password)
        {
            foreach (User u in _userRepo.FindAll())
            {
                if (u.Username == username && u.Password == password)
                {
                    return u;
                }
            }
            return null;
        }

        public bool IsLoggedIn(string username, string password)
        {
            return _userRepo.FindUser(username, password);
        }

        public IList<Type> GetAllTypes()
        {
            HashSet<Type> allTypes = new HashSet<Type>();
            foreach(Task task in _tasksRepo.FindAll())
            {
                allTypes.Add(task.Type);
            }
            return allTypes.ToList<Type>();
        }

        public IList<AgeGroup> GetAllAgeGroups()
        {
            HashSet<AgeGroup> allAgeGroups = new HashSet<AgeGroup>();
            foreach (Task task in _tasksRepo.FindAll())
            {
                allAgeGroups.Add(task.AgeGroup);
            }
            return allAgeGroups.ToList<AgeGroup>();
        }

        public IList<Participant> FindParticipantsByAgeType(Type type, AgeGroup ageGroup)
        {
            return _participantsRepo.FindParticipantsByAgeAndType(type, ageGroup);
        }

        public IList<Task> GetAllTasks()
        {
            return (IList<Task>)_tasksRepo.FindAll();
        }
    }
}

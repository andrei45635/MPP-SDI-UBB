using System;
using contestDomain;
using contestDomain.dto;
using contestDomain.enums;
using Task = contestDomain.Task;
using Type = contestDomain.enums.Type;

namespace contestService
{
    public interface IContestService
    {
        List<Participant> GetFilteredParticipants(Type type, AgeGroup ageGroup);
        int GetParticipantSize();
        void Register(string name, int age, Type type1, Type type2);
        List<Task> GetAllTasks();
        bool CheckUserExists(string username, string password);
        User FindLoggedInUser(string username, string password);
        void Login(User user, IContestObserver client);
        void Logout(User user, IContestObserver client);
        List<Task> GetAllTasksWithMaxEnrolled();
        int GetEnrolled(AgeGroup ageGroup, Type type);
        List<TaskDTO> GetTasksEnrolled();
        public List<AgeGroup> GetAllAgeGroups();
        public List<Type> GetAllTypes();
    }
}
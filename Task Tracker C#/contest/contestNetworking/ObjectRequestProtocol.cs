using contestDomain;
using Task = contestDomain.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace contestNetworking
{
    public interface Request { }

    [Serializable]
    public class LoginRequest : Request
    {
        private User user;
        public LoginRequest(User user)
        {
            this.user = user;
        }

        public virtual User User { get { return user; } }
    }

    [Serializable]
    public class LogoutRequest : Request
    {
        private User user;
        public LogoutRequest(User user)
        {
            this.user = user;
        }

        public virtual User User { get { return user; } }
    }

    [Serializable]
    public class AddParticipant : Request
    {
        private ParticipantTasks participantTask;
        public AddParticipant(ParticipantTasks participantTask) { this.participantTask = participantTask; }
        public virtual ParticipantTasks ParticipantTasks { get { return participantTask; } }
    }

    [Serializable]
    public class GetTasksEnrolled : Request
    {
        private List<Task> tasks;
        //public GetTasksEnrolled(List<Task> tasks) { this.tasks = tasks; }
        public GetTasksEnrolled() { }
        public virtual List<Task> Tasks { get { return tasks; } }
    }

    [Serializable]
    public class FilterParticipants : Request
    {
        private String[] filters;
        public FilterParticipants(String[] filters) { this.filters = filters; }
        public virtual String[] Filters { get { return filters; } }
    }

    [Serializable]
    public class FindLoggedInUser : Request
    {
        private User user;
        public FindLoggedInUser(User user) { this.user = user; }
        public virtual User User { get { return user; } }
    }

    [Serializable]
    public class GetAllAgeGroups: Request
    {

    }

    [Serializable]
    public class GetAllTypes : Request
    {

    }
}

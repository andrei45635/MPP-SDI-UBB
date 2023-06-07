using contestDomain;
using contestDomain.dto;
using contestDomain.enums;
using Type = contestDomain.enums.Type;
using Task = contestDomain.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace contestNetworking
{
    public interface Response { }


    [Serializable]
    public class OkResponse : Response { }

    [Serializable]
    public class ErrorResponse : Response
    {
        private string message;

        public ErrorResponse(string message) { this.message = message; }

        public virtual string Message { get { return message; } }
    }

    [Serializable]
    public class AddParticipantResponse : Response
    {
        private Participant participant;
        public AddParticipantResponse(Participant participant) { this.participant = participant; }
        public virtual Participant Participant { get { return participant; } }
    }

    [Serializable]
    public class GetTasksEnrolledResponse : Response
    {
        private List<TaskDTO> tasks;
        public GetTasksEnrolledResponse(List<TaskDTO> tasks) { this.tasks = tasks; }
        public virtual List<TaskDTO> Tasks { get { return tasks; } }
    }

    [Serializable]
    public class FilterResponse : Response
    {
        private List<Participant> participants;
        public FilterResponse(List<Participant> participants) { this.participants = participants; }
        public virtual List<Participant> Participants { get { return participants; } }
    }

    [Serializable]
    public class FindLoggedInUserResponse : Response
    {
        private User user;
        public FindLoggedInUserResponse(User user) { this.user = user; }
        public virtual User User { get { return user; } }
    }

    [Serializable]
    public class GetAllAgeGroupsResponse : Response
    {
        List<AgeGroup> ageGroups;
        public GetAllAgeGroupsResponse(List<AgeGroup> ageGroups) { this.ageGroups = ageGroups; }

        public virtual List<AgeGroup> AgeGroups { get { return ageGroups; } }
    }

    [Serializable]
    public class GetAllTypesResponse : Response
    {
        List<Type> types;
        public GetAllTypesResponse(List<Type> types) { this.types = types; }
        public virtual List<Type> Types { get { return types; } }
    }

    [Serializable]
    public class UpdateResponse : Response { }
}

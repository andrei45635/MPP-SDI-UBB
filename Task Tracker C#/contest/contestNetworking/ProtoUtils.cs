using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Example.Networking;
using MyType = contestDomain.enums.Type;
using proto = Com.Example.Networking;
using System.Reflection.Metadata.Ecma335;
using System.Globalization;

namespace contestNetworking
{
    static class ProtoUtils
    {
        public static ContestRequest CreateLoginRequest(contestDomain.User user)
        {
            proto.User user1 = new proto.User { Id = user.ID, Username = user.Username, Password = user.Password };
            ContestRequest request = new ContestRequest { Req = ContestRequest.Types.Req.Login, User = user1 };
            return request;
        }

        public static ContestRequest CreateLogoutRequest(contestDomain.User user)
        {
            return new ContestRequest { Req = ContestRequest.Types.Req.Logout, User = new User { Id = user.ID, Username = user.Username, Password = user.Password } };
        }

        public static ContestRequest CreateTasksEnrolledRequest()
        {
            return new ContestRequest { Req = ContestRequest.Types.Req.GetAllTasksExperimental };
        }

        public static ContestRequest CreateGetFilteredParticipantsRequest(MyType type, contestDomain.enums.AgeGroup age)
        {
            string[] filters = new string[2] { nameof(type), nameof(age) };
            return new ContestRequest { Req = ContestRequest.Types.Req.Filter, Type = filters[0], Age = filters[1] };
        }

        public static ContestRequest CreateRegisterRequest(string name, int age, MyType task1, MyType task2)
        {
            proto.ParticipantTask pt = new proto.ParticipantTask { Name = name, Age = age, Type1 = task1.ToString(), Type2 = task2.ToString() };
            return new ContestRequest { Req = ContestRequest.Types.Req.AddParticipantExperimental, Pt = pt };
        }

        public static ContestResponse CreateOKResp()
        {
            return new ContestResponse { Rep = ContestResponse.Types.Reply.Ok };
        }

        public static ContestResponse CreateErrorResponse(string text)
        {
            return new ContestResponse { Rep = ContestResponse.Types.Reply.Error, Error = text };
        }

        public static ContestResponse CreateGetTasksEnrolledResponse(List<proto.TaskDTO> tasks)
        {
            ContestResponse response = new ContestResponse { Rep = ContestResponse.Types.Reply.GetAllTasksExperimental };
            foreach (var task in tasks)
            {
                proto.TaskDTO taskDTO = new proto.TaskDTO { TaskID = task.TaskID, Type = task.Type, Age = task.Age, Enrolled = task.Enrolled };
                response.Tasks.Add(taskDTO);
            }
            return response;
        }

        public static ContestResponse CreateGetFilteredParticipantsResponse(List<proto.ParticipantDTO> parts)
        {
            ContestResponse response = new ContestResponse { Rep = ContestResponse.Types.Reply.Filter, Participants = { parts } };
            /*foreach (var participant in parts)
            {
                response.Participants.Add(participant);
            }*/
            return response;
        }

        public static ContestResponse CreateRegisterResponse()
        {
            return new ContestResponse { Rep = ContestResponse.Types.Reply.AddParticipantExperimental };
        }

        public static ContestResponse CreateUpdateResponse()
        {
            return new ContestResponse { Rep = ContestResponse.Types.Reply.Update };
        }

        public static string GetError(ContestResponse response)
        {
            return response.Error;
        }

        public static contestDomain.User GetUser(ContestRequest request)
        {
            return new contestDomain.User(request.User.Id, request.User.Username, request.User.Password);
        }

        public static contestDomain.ParticipantTasks GetPT(ContestRequest request)
        {
            Console.WriteLine(request.Pt.Type1.ToString());
            return new contestDomain.ParticipantTasks(request.Pt.ParticipantID, request.Pt.Name, request.Pt.Age, (MyType)Enum.Parse(typeof(MyType), request.Pt.Type1.ToString()), (MyType)Enum.Parse(typeof(MyType), request.Pt.Type2.ToString()));
        }

        public static List<TaskDTO> GetTasksConverted(List<contestDomain.dto.TaskDTO> tasks)
        {
            List<TaskDTO> res = new List<TaskDTO>();
            foreach (var tk in tasks)
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                string title = textInfo.ToTitleCase(tk.type.ToString().ToLower());
                string ageG = textInfo.ToTitleCase(tk.ageGroup.ToString().ToLower());
                TaskDTO taskDTO = new TaskDTO { TaskID = tk.taskID, Type = Enum.Parse<proto.Type>(title), Age = (proto.AgeGroup)Enum.Parse(typeof(proto.AgeGroup), ageG), Enrolled = tk.Enrolled };
                res.Add(taskDTO);
            }
            return res;
        }

        public static string[] GetFilters(ContestRequest request)
        {
            return new string[2] { request.Type, request.Age };
        }

        public static List<Participant> GetFilteredParticipantsConverted(List<contestDomain.Participant> participants)
        {
            List<Participant> res = new List<Participant>();
            foreach (var participant in participants)
            {
                Participant p = new Participant { Id = participant.ID, Name = participant.Name, Age = participant.Age };
                res.Add(p);
            }
            return res;
        }

        public static List<ParticipantDTO> GetFilteredParticipantsDTO(List<contestDomain.Participant> participants)
        {
            List<ParticipantDTO> res = new List<ParticipantDTO>();
            foreach (var participant in participants)
            {
                ParticipantDTO pd = new ParticipantDTO { Name = participant.Name, Age = participant.Age };
                res.Add(pd);
            }
            return res;
        }

        public static List<TaskDTO> GetEnrolledTasks(ContestResponse response)
        {
            List<TaskDTO> tasks = new List<TaskDTO>();
            for (int i = 0; i < response.Tasks.Count; i++)
            {
                tasks.Add(response.Tasks.ElementAt(i));
            }
            return tasks;
        }

        public static List<ParticipantDTO> GetFilteredPartsDTO(ContestResponse response)
        {
            List<ParticipantDTO> parts = new List<ParticipantDTO>();
            for (int i = 0; i < response.Participants.Count; i++)
            {
                parts.Add(response.Participants.ElementAt(i));
            }
            return parts;
        }
    }
}

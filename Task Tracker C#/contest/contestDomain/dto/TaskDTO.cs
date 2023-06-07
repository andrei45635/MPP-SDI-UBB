using contestDomain.enums;
using Type = contestDomain.enums.Type;

namespace contestDomain.dto
{
    [Serializable]
    public class TaskDTO
    {
        public int taskID { get; set; }
        public Type type { get; set; }
        public AgeGroup ageGroup { get; set; }
        public int Enrolled { get; set; }

        public TaskDTO(Type type, AgeGroup ageGroup, int enrolled)
        {
            this.type = type;
            this.ageGroup = ageGroup;
            this.Enrolled = enrolled;
        }

        public TaskDTO(int id, Type type, AgeGroup ageGroup, int enrolled)
        {
            this.taskID = id;
            this.type = type;
            this.ageGroup = ageGroup;
            this.Enrolled = enrolled;
        }
    }
}

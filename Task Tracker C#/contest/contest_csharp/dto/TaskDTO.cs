using contest_csharp.domain.enums;
using Type = contest_csharp.domain.enums.Type;

namespace contest_csharp.dto
{
    public class TaskDTO
    {
        public Type type { get; set; }
        public AgeGroup ageGroup { get; set; }
        public int Enrolled { get; set; }   

        public TaskDTO(Type type, AgeGroup ageGroup, int enrolled) 
        { 
            this.type = type;  
            this.ageGroup = ageGroup;
            this.Enrolled = enrolled;
        }
    }
}

using System.Runtime.CompilerServices;
using Type = contestDomain.enums.Type;

namespace contestDomain
{
    [Serializable]
    public class ParticipantTasks : Entity<int>
    {
        public int ParticipantID { get; set;  }
        public int Task1ID { get; set; }
        public int Task2ID { get; set; }

        public string Name { get; set; }
        public int Age { get; set; }    
        public Type Type1 { get; set; } 
        public Type Type2 { get; set; } 

        public ParticipantTasks(int id) : base(id)
        {
        
        }

        public ParticipantTasks(int id, int participantid, int task1id, int task2id) : base(id)
        {
            ParticipantID = participantid;
            Task1ID = task1id;
            Task2ID = task2id;
        }
    
        public ParticipantTasks(int id, string Name,  int Age, Type Type1, Type Type2) : base (id)
        {
            this.Name = Name;
            this.Age = Age;
            this.Type1 = Type1;
            this.Type2 = Type2;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
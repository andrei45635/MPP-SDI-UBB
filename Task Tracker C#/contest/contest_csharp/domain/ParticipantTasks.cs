namespace contest_csharp.domain
{
    public class ParticipantTasks : Entity<int>
    {
        public int ParticipantID { get; set;  }
        public int Task1ID { get; set; }
        public int Task2ID { get; set; }

        public ParticipantTasks(int id) : base(id)
        {
        
        }

        public ParticipantTasks(int id, int participantid, int task1id, int task2id) : base(id)
        {
            ParticipantID = participantid;
            Task1ID = task1id;
            Task2ID = task2id;
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
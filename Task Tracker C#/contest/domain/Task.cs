using contest.domain.enums;

namespace contest.domain
{
    public class Task : Entity<int>
    {
        public Type Type { get; set; }

        public AgeGroup AgeGroup { get; set; }
        
        public Task(int id) : base(id)
        {
        }

        public Task(int id, Type type, AgeGroup ageGroup) : base(id)
        {
            Type = type;
            AgeGroup = ageGroup;
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
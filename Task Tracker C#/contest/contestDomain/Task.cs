using contestDomain.enums;
using System.Xml.Linq;
using Type = contestDomain.enums.Type;

namespace contestDomain
{
    [Serializable]
    public class Task : Entity<int>
    {
        public Type Type { get; set; }

        public AgeGroup AgeGroup { get; set; }
        public int Id { get; set; }

        public Task(int id) : base(id)
        {
        }

        public Task(int id, Type type, AgeGroup ageGroup) : base(id)
        {
            Id = id;
            Type = type;
            AgeGroup = ageGroup;
        }

        public override string ToString()
        {
            return string.Format("[Task: ID = {0}, Type = {1}, Age Group = {2}]", Id, Type, AgeGroup);
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
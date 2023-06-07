using System;

namespace contestDomain
{
    [Serializable]
    public class Participant : Entity<int>
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Participant(int id) : base(id)
        {
            
        }

        public Participant(int id, string name, int age) : base(id)
        {
            Name = name;
            Age = age;
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
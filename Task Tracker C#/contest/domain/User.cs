using System;

namespace contest.domain
{
    public class User : Entity<int>
    {
        public String Username { get; set; }
        public String Password { get; set; }

        public User(int id) : base(id)
        {
        }

        public User(int id, string username, string password) : base(id)
        {
            Username = username;
            Password = password;
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
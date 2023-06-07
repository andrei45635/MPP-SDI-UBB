using System;

namespace contestDomain
{
    [Serializable]
    public class User : Entity<int>
    {
        public string Username { get; set; }
        public string Password { get; set; }

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
            return string.Format("[User: Id={0}, Username={1}, Password={2}]", ID, Username, Password);
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
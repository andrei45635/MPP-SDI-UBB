using contestDomain;

namespace contestRepoDB.users
{
    public interface UserRepo : IRepository<int, User>
    {
        bool FindUser(string username, string password);
        User FindLoggedInUser(string username, string password);   
    }
}

using contest_csharp.domain;

namespace contest_csharp.repo.users
{
    public interface UserRepo : IRepository<int, User>
    {
        bool FindUser(string username, string password);
    }
}

using contestDomain.enums;
using Task = contestDomain.Task;
using Type = contestDomain.enums.Type;

namespace contestRepoDB.tasks
{
    public interface TaskRepo: IRepository<int, Task>
    {
        int CountTasksByAgeAndType(Type type, AgeGroup ageGroup);
    }
}

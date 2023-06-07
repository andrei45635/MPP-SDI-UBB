using contest_csharp.domain.enums;
using Task = contest_csharp.domain.Task;
using Type = contest_csharp.domain.enums.Type;

namespace contest_csharp.repo.tasks
{
    public interface TaskRepo: IRepository<int, Task>
    {
        int CountTasksByAgeAndType(Type type, AgeGroup ageGroup);
    }
}

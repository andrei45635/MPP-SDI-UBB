using contest_csharp.dto;
using contest_csharp.repo.tasks;
using System.Collections.Generic;
using Task = contest_csharp.domain.Task;

namespace contest_csharp.mapper
{
    public class Task2TaskDTO
    {
        private TaskRepo taskRepository;

        public Task2TaskDTO (TaskRepo taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        public TaskDTO Convert(Task task)
        {
            int enrolled = taskRepository.CountTasksByAgeAndType(task.Type, task.AgeGroup);
            return new TaskDTO(task.Type, task.AgeGroup, enrolled);
        }

        public IList<TaskDTO> ConvertList(List<Task> tasks)
        {
            IList<TaskDTO> tasksDTO = new List<TaskDTO>();
            foreach (Task task in tasks)
            {
                tasksDTO.Add(Convert(task));
            }
            return tasksDTO;
        }
    }
}

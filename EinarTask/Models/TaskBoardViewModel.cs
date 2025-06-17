// Models/ViewModels/TaskBoardViewModel.cs
namespace EinarTask.Models.ViewModels
{
    public class TaskBoardViewModel
    {
        public IEnumerable<TaskType> TaskTypes { get; set; }
        public IEnumerable<Task> AllTasks { get; set; } // Tüm task'lar için
        public Task NewTask { get; set; } // Yeni task oluşturma için
        public TaskType NewTaskType { get; set; } // Yeni task type oluşturma için
    }
}
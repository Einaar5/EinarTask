using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EinarTask.Models
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<TaskType> TaskTypes { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}

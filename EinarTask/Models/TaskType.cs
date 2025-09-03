using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EinarTask.Models
{
    public class TaskType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string? Description { get; set; }

        [StringLength(7)]
        public string Color { get; set; }

        public int Order { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Foreign Key - Artık int
        [Required]
        public int UserId { get; set; }

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }

        public TaskType()
        {
            Tasks = new HashSet<Task>();
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EinarTask.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? DueDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public bool IsCompleted { get; set; } = false;

        public int Priority { get; set; } = 1;

        // Foreign Keys - Artık int
        [Required]
        public int UserId { get; set; }

        [Required]
        public int TaskTypeId { get; set; }

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("TaskTypeId")]
        public virtual TaskType TaskType { get; set; }
    }
}

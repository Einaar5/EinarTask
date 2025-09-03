namespace EinarTask.Models
{
    public class UserDto
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime CreatedDate { get; set; }
        
        public IFormFile UserImage { get; set; }
    }
}

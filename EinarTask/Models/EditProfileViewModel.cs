using EinarTask.Models;

public class EditProfileViewModel
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string Email { get; set; } = "";

    public string UserImage { get; set; } = "";

    public UserDto? userDto { get; set; }
}

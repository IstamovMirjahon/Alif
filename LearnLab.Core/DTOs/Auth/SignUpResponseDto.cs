namespace LearnLab.Core.DTOs.Auth;

public class SignUpResponseDto
{
    public SignUpResponseDto(Guid id, string userName, string firstName, string lastName, List<string> roles)
    {
        Id = id;
        PhoneNumber = userName;
        FirstName = firstName;
        LastName = lastName;
        Roles = roles;
    }

    public Guid Id { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<string>? Roles { get; set; }
}

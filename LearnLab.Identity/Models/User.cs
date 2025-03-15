using LearnLab.Core.Enum.GenderTypes;
using Microsoft.AspNetCore.Identity;

namespace LearnLab.Identity.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public GenderEnum Gender { get; set; } = GenderEnum.Unknown;
        public DateTime? BirthDate { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public User(string firstName, string lastName, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = phoneNumber;
            PhoneNumber = phoneNumber;
            CreatedAt = DateTimeOffset.Now;
            IsDeleted = false;
        }

        public User(string firstName, string lastName, string phoneNumber,string email,  GenderEnum gender, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = phoneNumber??email;
            PhoneNumber = phoneNumber;
            Gender = gender;
            Email = email;
            BirthDate = birthDate;
            CreatedAt = DateTimeOffset.Now;
            IsDeleted = false;
        }
    }
}

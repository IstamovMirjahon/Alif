using LearnLab.Core.Enum.GenderTypes;
using Microsoft.AspNetCore.Identity;

namespace LearnLab.Identity.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderEnum Gender { get; set; } = GenderEnum.Unknown;
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public User(string firstName, string lastName, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = phoneNumber;
            PhoneNumber = phoneNumber;
            CreatedAt = DateTime.Now;
            IsDeleted = false;
        }

        public User(string firstName, string lastName, string phoneNumber, GenderEnum gender, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = phoneNumber;
            PhoneNumber = phoneNumber;
            Gender = gender;
            BirthDate = birthDate;
            CreatedAt = DateTime.Now;
            IsDeleted = false;
        }
    }
}

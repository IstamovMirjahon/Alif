using LearnLab.Core.Enum.GenderTypes;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Reflection;

namespace LearnLab.Identity.Models
{
    class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderEnum Gender { get; set; } = GenderEnum.Unknown;
        public DateTime BirthDate { get; set; }
        public User()
        {

        }
        public User(string firstName, string lastName, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = phoneNumber;
            PhoneNumber = phoneNumber;
        }

        public User(string firstName, string lastName, string phoneNumber, string email, GenderEnum gender, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = phoneNumber;
            PhoneNumber = phoneNumber;
            Email = email;
            Gender = gender;
            BirthDate = birthDate;
        }
    }
}

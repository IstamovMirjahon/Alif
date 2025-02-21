using Microsoft.AspNetCore.Identity;

namespace LearnLab.Identity.Models
{
    public class Role : IdentityRole
    {
        public Role(string roleName, string description)
        {
            Name = roleName;
            Description = description;
        }

        public string Description { get; set; }
    }
}


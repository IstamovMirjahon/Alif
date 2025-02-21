using Microsoft.AspNet.Identity.EntityFramework;
using System.Xml.Linq;

namespace LearnLab.Identity.Models
{
    public class Role : IdentityRole
    {
        public Role() { }

        public Role(string roleName, string description)
        {
            Name = roleName;
            Description = description;
        }

        public string Description { get; set; }
    }
}


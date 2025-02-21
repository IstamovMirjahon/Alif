using LearnLab.Core.Entities.SMS;
using LearnLab.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearnLab.Identity
{
    public class LearnLabIdentityDbContext : IdentityDbContext<User, Role, string>
    {
        public LearnLabIdentityDbContext(DbContextOptions<LearnLabIdentityDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
       base.OnModelCreating(modelBuilder);

        public DbSet<SmsToken> SmsTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}

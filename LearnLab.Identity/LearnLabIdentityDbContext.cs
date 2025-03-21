﻿using LearnLab.Core.Entities.SMS;
using LearnLab.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LearnLab.Identity
{
    public class LearnLabIdentityDbContext : IdentityDbContext<User, Role, string>
    {
        public LearnLabIdentityDbContext(DbContextOptions<LearnLabIdentityDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        base.OnModelCreating(modelBuilder);

        public DbSet<SmsToken> SmsTokens { get; set; }
        public DbSet<EmailToken> EmailTokens { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }
}

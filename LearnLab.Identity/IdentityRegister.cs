using LearnLab.Identity.ClaimsPrincipalFactory;
using LearnLab.Identity.Email;
using LearnLab.Identity.Models;
using LearnLab.Identity.Services.Auth;
using LearnLab.Identity.SMS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetShop.Core.AccesConfigurations;
using System.Reflection;
using System.Text;

namespace LearnLab.Identity
{
    public static class IdentityRegister
    {
        public static IServiceCollection RegisterIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LearnLabIdentityDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), opt =>
                    opt.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));

            });

            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                //options.User.RequireUniqueEmail = true;
            })
                .AddRoles<Role>()
                //.AddUserManager<User>()
                .AddRoleManager<RoleManager<Role>>()
                .AddEntityFrameworkStores<LearnLabIdentityDbContext>()
                .AddClaimsPrincipalFactory<LearnLabClaimsPrincipalFactory>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = configuration["AccessConfiguration:Audience"],
                ValidIssuer = configuration["AccessConfiguration:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey.TheSecretKey))
            };
        });

            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISmsSender, SmsSender>();

            services.AddSingleton<EskizTokenHandler>();

            services.AddOptions<SmsClientOptions>().Bind(configuration.GetSection(SmsClientOptions.SmsSectionName));

            using var provider = services.BuildServiceProvider();

            var dbContext = provider.GetService<LearnLabIdentityDbContext>();

            dbContext?.Database.Migrate();
            return services;
        }
    }
}

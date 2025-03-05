using LearnLab.Identity.Constants;
using LearnLab.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
namespace LearnLab.Identity.ClaimsPrincipalFactory;

public class LearnLabClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
{
    public LearnLabClaimsPrincipalFactory(UserManager<User> userManager,
        IOptions<IdentityOptions> optionAccessor) :
        base(userManager, optionAccessor)
    { }
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName));
        identity.AddClaim(new Claim(ClaimNames.UserName, user.UserName));
        identity.AddClaim(new Claim(ClaimNames.Email, user.Email));
        identity.AddClaim(new Claim(ClaimNames.UserId, user.Id));
        identity.AddClaim(new Claim(ClaimNames.FirstName, user.FirstName));
        identity.AddClaim(new Claim(ClaimNames.LastName, user.LastName));

        return identity;
    }
}

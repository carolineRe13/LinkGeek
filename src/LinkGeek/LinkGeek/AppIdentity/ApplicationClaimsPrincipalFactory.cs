using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace LinkGeek;

public class ApplicationClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
{
    public ApplicationClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> options)
        : base(userManager, roleManager, options)
    {
    }
    
    public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
    {
        var principal = await base.CreateAsync(user);
        if (!string.IsNullOrWhiteSpace(user.FirstName))
        {                      ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                new Claim(ClaimTypes.GivenName, user.FirstName)
            });
        }
 
        if (!string.IsNullOrWhiteSpace(user.LastName))
        {                      ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                new Claim(ClaimTypes.Surname, user.LastName),
            });
        }
        return principal;
    }
}
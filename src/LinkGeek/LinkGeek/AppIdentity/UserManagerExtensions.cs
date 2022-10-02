using LinkGeek.AppIdentity;
using Microsoft.AspNetCore.Identity;

namespace LinkGeek;

public static class UserManagerExtensions
{
    public static Task<string> GetGender(this UserManager<ApplicationUser> userManager, ApplicationUser user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        return Task.FromResult(user.Gender);
    }
}
using Microsoft.AspNetCore.Identity;

namespace LinkGeek;

public static class UserStoreExtensions
{
    public static Task SetUserFirstNameAsync(this IUserStore<ApplicationUser> userStore,
        ApplicationUser user, string firstName, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        user.FirstName = firstName;
        return Task.CompletedTask;
    }
    
    public static Task SetUserLastNameAsync(this IUserStore<ApplicationUser> userStore,
        ApplicationUser user, string lastName, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        user.LastName = lastName;
        return Task.CompletedTask;
    }
}
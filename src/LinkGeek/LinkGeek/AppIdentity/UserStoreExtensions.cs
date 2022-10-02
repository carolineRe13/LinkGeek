using Microsoft.AspNetCore.Identity;

namespace LinkGeek.AppIdentity;

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

    public static Task SetGenderAsync(this IUserStore<ApplicationUser> userStore,
        ApplicationUser user, string gender, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        user.Gender = gender;
        return Task.CompletedTask;
    }
    
    public static Task SetProfilePictureAsync(this IUserStore<ApplicationUser> userStore,
        ApplicationUser user, byte[] profilePicture, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        // user.ProfilePicture = profilePicture;
        return Task.CompletedTask;
    }
}
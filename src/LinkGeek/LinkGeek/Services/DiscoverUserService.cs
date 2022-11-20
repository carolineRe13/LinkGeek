using LinkGeek.AppIdentity;
using LinkGeek.Data;
using Microsoft.EntityFrameworkCore;

namespace LinkGeek.Services;

public class DiscoverUserService
{
    private readonly IContextProvider contextProvider;
    private readonly UserService _userService;

    public DiscoverUserService(IContextProvider contextProvider, UserService userService)
    {
        this.contextProvider = contextProvider;
        this._userService = userService;
    }

    public async Task<List<ApplicationUser>> GetUsers(ApplicationUser currentUser)
    {
        await using var context = contextProvider.GetContext();
        var completeUser = await _userService.GetUserFromUserNameAsync(currentUser.UserName, includeLikedPosts: false);
        if (completeUser == null) return new List<ApplicationUser>();

        var friendIds = completeUser.Friends.Select(f => f.Id).ToList();
        return await context.Users
            .Where(u => u.Id != completeUser.Id)
            .Where(u => friendIds.All(id => id != u.Id))
            .Take(5).ToListAsync();
    }
}
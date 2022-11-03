using LinkGeek.AppIdentity;
using LinkGeek.Data;

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
        await using (var context = contextProvider.GetContext())
        {
            var completeUser = _userService.GetUserFromUserName(currentUser.UserName);
            return context.Users.AsQueryable()
                .Where(u => u.Id != currentUser.Id)
                .Where(u => completeUser != null && !completeUser.Friends.Contains(u))
                .Take(5).ToList();
        }
    }
}
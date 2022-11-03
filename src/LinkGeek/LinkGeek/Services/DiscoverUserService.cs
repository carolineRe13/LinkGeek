using LinkGeek.AppIdentity;
using LinkGeek.Data;

namespace LinkGeek.Services;

public class DiscoverUserService
{
    private readonly IContextProvider contextProvider;

    public DiscoverUserService(IContextProvider contextProvider)
    {
        this.contextProvider = contextProvider;
    }

    public async Task<List<ApplicationUser>> GetUsers(ApplicationUser currentUser)
    {
        List<ApplicationUser> userList = new List<ApplicationUser>();
        await using (var context = contextProvider.GetContext())
        {
            userList = context.Users.AsQueryable()
                .Where(u => u.Id != currentUser.Id)
                .Where(u => null == currentUser.Friends || !currentUser.Friends.Contains(u))
                .Take(5).ToList();
        }

        return userList;
    }
}
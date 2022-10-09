using LinkGeek.AppIdentity;
using LinkGeek.Data;

namespace LinkGeek.Services;

public class DiscoverUserService
{
    public async Task<List<ApplicationUser>> GetUsers(ApplicationUser currentUser)
    {
        List<ApplicationUser> userList = new List<ApplicationUser>();
        await using (var context = new ApplicationDbContext())
        {
            userList = context.Users.AsQueryable()
                .Where(u => u.Id != currentUser.Id)
                .Where(u => null == currentUser.Friends || !currentUser.Friends.Select(f => f.User2).Contains(u))
                .Take(4).ToList();
        }

        return userList;
    }
}
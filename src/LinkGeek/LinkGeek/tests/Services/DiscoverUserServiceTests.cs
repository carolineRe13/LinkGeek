using LinkGeek.AppIdentity;
using LinkGeek.Areas.Friends.Pages;
using LinkGeek.Data;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinkGeek.tests.Services
{
    [TestClass]
    public class DiscoverUserServiceTests
    {
        private TestContextProvider _contextProvider;
        private DiscoverUserService _discoverUserService;
        private UserService _userService;
        
        [TestInitialize]
        public void SetUp()
        {
            // common Arrange 
            var connection = new SqliteConnection("Data Source=DiscoverTests.db");
            connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            this._contextProvider = new TestContextProvider(contextOptions);
            this._contextProvider.GetContext().Database.Migrate();

            this._userService = new UserService(_contextProvider);
            
            this._discoverUserService = new DiscoverUserService(_contextProvider, _userService);
        }

        [TestMethod]
        public void GetUsersTest()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            
            List<ApplicationUser> fiveRandomUsers = new List<ApplicationUser> {
                new ApplicationUser(),
                new ApplicationUser(),
                new ApplicationUser(),
                new ApplicationUser(),
                new ApplicationUser()
            };

            context.AddRange(fiveRandomUsers);
            context.SaveChanges();
            
            // Act
            ICollection<ApplicationUser> playersOfGame = this._discoverUserService.GetUsers(currentUser).Result;
            
            // Assert
            for (int i = 0; i < fiveRandomUsers.Count; i++)
            {
                var selectedUser = fiveRandomUsers[i];
                Assert.IsTrue(playersOfGame.Contains(selectedUser));
            }
        }
        
        [TestMethod]
        public async Task GetUsersTestDoesntResturnFriend()
        {
            // Arrange
            // Create the schema and seed some data
            
            var currentUser = new ApplicationUser();
            var friend = new ApplicationUser();
            currentUser.Friends.Add(friend);
            friend.Friends.Add(currentUser);
            
            var nonFriend1 = new ApplicationUser();
            var nonFriend2 = new ApplicationUser();
            var nonFriend3 = new ApplicationUser();
            var nonFriend4 = new ApplicationUser();
            
            List<ApplicationUser> fiveRandomUsers = new List<ApplicationUser> {
                nonFriend1,
                nonFriend2,
                nonFriend3,
                nonFriend4,
                friend
            };
            
            await using (var context = _contextProvider.GetContext())
            {
                await context.Users.AddRangeAsync(currentUser, friend, nonFriend1, nonFriend2, nonFriend3, nonFriend4);
                var response = await context.SaveChangesAsync();
                Console.WriteLine(response);
            }
            
            // Act
            ICollection<ApplicationUser> selectedUsers = await this._discoverUserService.GetUsers(currentUser);
            
            // Assert
            foreach (var selectedUser in fiveRandomUsers)
            {
                if (selectedUser.Id == friend.Id)
                {
                    Assert.IsFalse(selectedUsers.Any(playerOfGame => playerOfGame.Id == friend.Id));
                }
                else
                {
                    Assert.IsTrue(selectedUsers.Contains(selectedUser));
                }
            }
        }
    }
}

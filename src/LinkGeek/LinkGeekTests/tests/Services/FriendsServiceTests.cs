using LinkGeek.AppIdentity;
using LinkGeek.Data;
using LinkGeek.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinkGeek.tests.Services
{
    [TestClass]
    public class FriendsServiceTests
    {
        private TestContextProvider _contextProvider;
        private DiscoverUserService _gameDbService;
        private FriendService _friendsService;

        [TestInitialize]
        public void SetUp()
        {
            // common Arrange 
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;
            
            this._contextProvider = new TestContextProvider(contextOptions);
            this._contextProvider.GetContext().Database.Migrate();

            this._friendsService = new FriendService(_contextProvider);
        }

        [TestMethod]
        public async Task AddFriendPending()
        {            
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            var otherUser = new ApplicationUser();
            
            context.AddRange(currentUser);
            context.AddRange(otherUser);
            await context.SaveChangesAsync();
            
            // Act
            var response = await _friendsService.AddFriend(currentUser.Id, otherUser.Id);
            // Assert
            
            Assert.AreEqual(response, FriendsResponses.PendingFriends);
        }
        
        [TestMethod]
        public async Task AddFriendFriends()
        {            
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            var otherUser = new ApplicationUser();
            
            context.AddRange(currentUser);
            context.AddRange(otherUser);
            await context.SaveChangesAsync();
            
            // Act
            await _friendsService.AddFriend(currentUser.Id, otherUser.Id);
            var response = await _friendsService.AddFriend(otherUser.Id,currentUser.Id);
            // Assert
            
            Assert.AreEqual(response, FriendsResponses.YouAreNowFriends);
        }
        
        [TestMethod]
        public async Task AddFriendAlreadyFriends()
        {            
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            var otherUser = new ApplicationUser();
            
            context.AddRange(currentUser);
            context.AddRange(otherUser);
            await context.SaveChangesAsync();
            
            // Act
            await _friendsService.AddFriend(currentUser.Id, otherUser.Id);
            await _friendsService.AddFriend(otherUser.Id,currentUser.Id);
            var response = await _friendsService.AddFriend(otherUser.Id,currentUser.Id);
            // Assert
            
            Assert.AreEqual(response, FriendsResponses.AlreadyFriends);
        }
        
        [TestMethod]
        public async Task RemoveFriendSuccessful()
        {            
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            var otherUser = new ApplicationUser();
            
            context.AddRange(currentUser);
            context.AddRange(otherUser);
            await context.SaveChangesAsync();
            
            // Act
            await _friendsService.AddFriend(currentUser.Id, otherUser.Id);
            await _friendsService.AddFriend(otherUser.Id,currentUser.Id);
            var response = await _friendsService.RemoveFriend(otherUser.Id,currentUser.Id);
            
            // Assert
            Assert.AreEqual(response, FriendsResponses.FriendRemoved);
        }
        
        [TestMethod]
        public async Task RemoveFriendAlreadyRemoved()
        {            
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            var otherUser = new ApplicationUser();
            
            context.AddRange(currentUser);
            context.AddRange(otherUser);
            await context.SaveChangesAsync();
            
            // Act
            await _friendsService.AddFriend(currentUser.Id, otherUser.Id);
            await _friendsService.AddFriend(otherUser.Id,currentUser.Id);
            await _friendsService.RemoveFriend(otherUser.Id,currentUser.Id);
            var response = await _friendsService.RemoveFriend(otherUser.Id,currentUser.Id);
            
            // Assert
            Assert.AreEqual(response, FriendsResponses.FriendRemoved);
        }
        
        [TestMethod]
        public async Task DeclineFriend()
        {            
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            var otherUser = new ApplicationUser();
            
            context.AddRange(currentUser);
            context.AddRange(otherUser);
            await context.SaveChangesAsync();
            
            // Act
            await _friendsService.AddFriend(currentUser.Id, otherUser.Id);
            var response = await _friendsService.DeclineFriendRequest(otherUser.Id,currentUser.Id);
            
            // Assert
            Assert.AreEqual(response, FriendsResponses.DeclinedFriendRequest);
        }
        
        [TestMethod]
        public async Task CancelFriendRequest()
        {            
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            var otherUser = new ApplicationUser();
            
            context.AddRange(currentUser);
            context.AddRange(otherUser);
            await context.SaveChangesAsync();
            
            // Act
            await _friendsService.AddFriend(currentUser.Id, otherUser.Id);
            var response = await _friendsService.CancelFriendRequest(currentUser.Id, otherUser.Id);
            
            // Assert
            Assert.AreEqual(response, FriendsResponses.CanceledFriendRequest);
        }
    }
}

using System.Collections.ObjectModel;
using LinkGeek.AppIdentity;
using LinkGeek.Data;
using LinkGeek.Models;
using LinkGeek.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinkGeek.tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private TestContextProvider _contextProvider;
        private UserService _userService;
        
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

            this._userService = new UserService(_contextProvider);
        }

        [TestMethod]
        public async Task AddGameToUSerSuccessful()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            var games = new List<Game>
            {
                new Game("1", "Game 1", new Uri("https://www.google.com"), new List<ApplicationUser>()),
                new Game("2", "Game 2", new Uri("https://www.google.com"), new List<ApplicationUser> {})
            };

            context.AddRange(games);
            context.AddRange(currentUser);
            await context.SaveChangesAsync();
            
            // Act
            var result= await _userService.AddGameToUser(currentUser.Id, "1");
            
            // Assert
            Assert.AreEqual(AddGameToUserResponse.Success, result);
        }
        
        [TestMethod]
        public async Task AddGameToUSerGameNotFound()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            context.AddRange(currentUser);
            await context.SaveChangesAsync();
            
            // Act
            var result= await _userService.AddGameToUser(currentUser.Id, "1");
            
            // Assert
            Assert.AreEqual(AddGameToUserResponse.GameNotFound, result);
        }
        
        [TestMethod]
        public async Task AddGameToUSerGameAlreadyInLibrary()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            var games = new List<Game>
            {
                new Game("1", "Game 1", new Uri("https://www.google.com"), new List<ApplicationUser>()),
                new Game("2", "Game 2", new Uri("https://www.google.com"), new List<ApplicationUser> {currentUser})
            };

            context.AddRange(games);
            context.AddRange(currentUser);
            await context.SaveChangesAsync();
            
            // Act
            var result= await _userService.AddGameToUser(currentUser.Id, "2");
            
            // Assert
            Assert.AreEqual(AddGameToUserResponse.GameAlreadyAdded, result);
        }
        
        [TestMethod]
        public async Task CreatePostSuccessfulWithGame()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();

            var game = new Game("1", "Game 1", new Uri("https://www.google.com"), new List<ApplicationUser>());
            
            var currentUser = new ApplicationUser();
            var games = new List<Game>
            {
                game
            };

            context.AddRange(games);
            context.AddRange(currentUser);
            await context.SaveChangesAsync();
            
            // Act
            var result= await _userService.CreatePost(currentUser, "hey", game, PlayerRoles.None, null);
            
            // Assert
            Assert.AreEqual(CreatePostResponse.Success, result);
        }
        
        [TestMethod]
        public async Task CreatePostSuccessfulNoGame()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();

            context.AddRange(currentUser);
            await context.SaveChangesAsync();
            
            // Act
            var result= await _userService.CreatePost(currentUser, "hey", null, PlayerRoles.None, null);
            
            // Assert
            Assert.AreEqual(CreatePostResponse.Success, result);
        }
        
        [TestMethod]
        public async Task CreatePostSuccessfulNoExistingGame()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            var game = new Game("1", "Game 1", new Uri("https://www.google.com"), new List<ApplicationUser>());

            context.AddRange(currentUser);
            await context.SaveChangesAsync();
            
            // Act
            var result= await _userService.CreatePost(currentUser, "hey", game, PlayerRoles.None, null);
            
            // Assert
            Assert.AreEqual(CreatePostResponse.Success, result);
        }
        
        [TestMethod]
        public async Task RemoveGameFromUserSuccessful()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            var games = new List<Game>
            {
                new Game("1", "Game 1", new Uri("https://www.google.com"), new List<ApplicationUser>()),
                new Game("2", "Game 2", new Uri("https://www.google.com"), new List<ApplicationUser> {currentUser})
            };

            context.AddRange(games);
            context.AddRange(currentUser);
            await context.SaveChangesAsync();
            
            // Act
            var result= await _userService.RemoveGameFromUser(currentUser.Id, "2");
            
            // Assert
            Assert.AreEqual(RemoveGameFromUserResponse.Success, result);
        }
        
        [TestMethod]
        public async Task RemoveGameFromUserNotInLibrary()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            var games = new List<Game>
            {
                new Game("1", "Game 1", new Uri("https://www.google.com"), new List<ApplicationUser>()),
                new Game("2", "Game 2", new Uri("https://www.google.com"), new List<ApplicationUser> {})
            };

            context.AddRange(games);
            context.AddRange(currentUser);
            await context.SaveChangesAsync();
            
            // Act
            var result= await _userService.RemoveGameFromUser(currentUser.Id, "2");
            
            // Assert
            Assert.AreEqual(RemoveGameFromUserResponse.GameAlreadyRemoved, result);
        }
        
        [TestMethod]
        public async Task RemoveGameFromUserNotFound()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            var games = new List<Game>
            {
                new Game("1", "Game 1", new Uri("https://www.google.com"), new List<ApplicationUser>()),
                new Game("2", "Game 2", new Uri("https://www.google.com"), new List<ApplicationUser> {})
            };

            context.AddRange(games);
            context.AddRange(currentUser);
            await context.SaveChangesAsync();
            
            // Act
            var result= await _userService.RemoveGameFromUser(currentUser.Id, "3");
            
            // Assert
            Assert.AreEqual(RemoveGameFromUserResponse.GameNotFound, result);
        }
        
        [TestMethod]
        public async Task UserHasGameNotInLibrary()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            var games = new List<Game>
            {
                new Game("1", "Game 1", new Uri("https://www.google.com"), new List<ApplicationUser>()),
                new Game("2", "Game 2", new Uri("https://www.google.com"), new List<ApplicationUser> {})
            };

            context.AddRange(games);
            context.AddRange(currentUser);
            await context.SaveChangesAsync();
            
            // Act
            var result= await _userService.HasGameInLibrary(currentUser.Id, "1");
            
            // Assert
            Assert.AreEqual(false, result);
        }
        
        [TestMethod]
        public async Task UserHasGameInLibrary()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            var games = new List<Game>
            {
                new Game("1", "Game 1", new Uri("https://www.google.com"), new List<ApplicationUser>()),
                new Game("2", "Game 2", new Uri("https://www.google.com"), new List<ApplicationUser> { currentUser })
            };

            context.AddRange(games);
            context.AddRange(currentUser);
            await context.SaveChangesAsync();
            
            // Act
            var result= await _userService.HasGameInLibrary(currentUser.Id, "2");
            
            // Assert
            Assert.AreEqual(true, result);
        }
        
        [TestMethod]
        public async Task UpdateLocationSuccessful()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            context.AddRange(currentUser);
            await context.SaveChangesAsync();
            
            // Act
            var result= await _userService.UpdateLocation(currentUser.Id, "Oslo");
            
            // Assert
            Assert.AreEqual("Oslo", result);
        }
        
        [TestMethod]
        public async Task UpdateLocationIncorrect()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            context.AddRange(currentUser);
            await context.SaveChangesAsync();
            
            // Act
            var result= await _userService.UpdateLocation(currentUser.Id, "Munich");
            
            // Assert
            Assert.AreNotEqual("Oslo", result);
        }
        
        [TestMethod]
        public async Task UpdateStatusSuccessful()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            context.AddRange(currentUser);
            await context.SaveChangesAsync();
            
            // Act
            var result= await _userService.UpdateStatus(currentUser.Id, "Hello!");
            
            // Assert
            Assert.AreEqual("Hello!", result);
        }
        
        [TestMethod]
        public async Task GetUsersGames()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            var games = new Collection<Game>
            {
                new Game("2", "Game 2", new Uri("https://www.google.com"), new List<ApplicationUser> { currentUser })
            };

            context.AddRange(games);
            context.AddRange(currentUser);
            await context.SaveChangesAsync();
            
            // Act
            var result= await _userService.GetUsersGamesAsync(currentUser.Id);
            
            // Assert
            // Not a good way, missing equals implementation
            Assert.AreEqual(games.ToList()[0], result.ToList()[0]);
        }
        
        [TestMethod]
        public async Task GetUserFromUserName()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var currentUser = new ApplicationUser();
            currentUser.UserName = "Smaraktara";
            
            context.AddRange(currentUser);
            await context.SaveChangesAsync();
            
            // Act
            var result= await _userService.GetUserFromUserNameAsync(currentUser.UserName);
            
            // Assert
            // Not a good way, missing equals implementation
            Assert.AreEqual(currentUser, result);
        }
    }
}

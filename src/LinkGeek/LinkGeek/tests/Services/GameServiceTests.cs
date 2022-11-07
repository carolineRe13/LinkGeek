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
    public class GameServiceTests
    {
        private GameService _gameService;
        private TestContextProvider _contextProvider;
        
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
            
            this._gameService = new GameService(_contextProvider);

        }

        [TestMethod]
        public void FindGame()
        {
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            context.Database.Migrate();

            context.AddRange(
                new Game("1", "Game 1", new Uri("https://www.google.com"), null),
                new Game("2", "Game 2", new Uri("https://www.google.com"), null));
            context.SaveChanges();


            // Act
            var game = context.Game.Find("2");


            // Assert
            Assert.AreEqual("Game 2", game?.Name);
        }

        [TestMethod]
        public void GetGamePlayers()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            context.Database.Migrate();
            
            var user = new ApplicationUser();
            
            List<ApplicationUser> usersPlayingGame = new List<ApplicationUser> {
                user
            };

            var games = new List<Game>
            {
                new Game("1", "Game 1", new Uri("https://www.google.com"), usersPlayingGame),
                new Game("2", "Game 2", new Uri("https://www.google.com"), new List<ApplicationUser> {})
            };
            user.Games = new List<Game>(games);

            context.AddRange(games);
            context.SaveChanges();
            
            // Act
            ICollection<ApplicationUser> playersOfGame = this._gameService.GetGamePlayersAsync("1").Result;
            
            // Assert
            // NOT THE BETS WAY, it needs an equal implementation 
            Assert.AreEqual(usersPlayingGame.ToList()[0].UserName, playersOfGame.ToList()[0].UserName);
        }
    }
}

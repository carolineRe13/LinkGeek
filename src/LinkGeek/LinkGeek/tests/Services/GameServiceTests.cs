using LinkGeek.Data;
using LinkGeek.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinkGeek.tests.Services
{
    [TestClass]
    public class GameServiceTests
    {
        [TestInitialize]
        public void SetUp()
        {
            // common Arrange
        }

        [TestMethod]
        public void testName()
        {
            // Arrange
            // Act
            // 
            
            var _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            var _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_connection)
                .Options;

            // Create the schema and seed some data
            using var context = new ApplicationDbContext(_contextOptions);
            context.Database.Migrate();

            context.AddRange(
                new Game("1", "Game 1", new Uri("https://www.google.com")),
                new Game("2", "Game 2", new Uri("https://www.google.com")));
            context.SaveChanges();

            var game = context.Game.Find("2");
            
            Assert.AreEqual("Game 2", game?.Name);
        }
    }
}

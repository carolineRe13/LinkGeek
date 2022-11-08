using LinkGeek.AppIdentity;
using LinkGeek.Data;
using LinkGeek.Services;
using LinkGeek.Shared;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace LinkGeek.tests.Services
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ChatServiceTests
    {
        private TestContextProvider _contextProvider;
        private ChatService _chatService;

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

            this._chatService = new ChatService(_contextProvider);
        }

        [TestMethod]
        public void GetUserDetailsAsyncTest()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var user = new ApplicationUser();

            context.AddRange(user);
            context.SaveChanges();
            
            // Act
            ApplicationUser userDetails = this._chatService.GetUserDetailsAsync(user.Id).Result;
            
            // Assert
            Assert.AreEqual(userDetails, user);
        }
        
        [TestMethod]
        public void GetUsersAsyncTest()
        {
            // Arrange
            // Create the schema and seed some data
            using var context = _contextProvider.GetContext();
            
            var fromUser = new ApplicationUser();
            var toUser = new ApplicationUser();
            var message = new ChatMessage()
            {
                Id = 123,
                Message = "Hello",
                ToUserId = "12",
                CreatedDate = DateTime.Now,
                FromUserId = "23",
                FromUser = fromUser,
                ToUser = toUser
            };
            
            // Act
            int result = this._chatService.SaveMessageAsync(message, fromUser.Id).Result;
            
            // Assert
            Assert.AreEqual(3, result);
        }
    }
}

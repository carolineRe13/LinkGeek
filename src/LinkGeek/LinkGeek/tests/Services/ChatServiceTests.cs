using LinkGeek.Data;
using LinkGeek.Services;

namespace LinkGeek.tests.Services
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;

    [TestClass]
    public class ChatServiceTests
    {

        private TestContextProvider contextProvider = new TestContextProvider();
        private ChatService chatService;

        [TestInitialize]
        public void SetUp()
        {
            // common Arrange
            this.chatService = new ChatService(this.contextProvider);
        }

        [TestMethod]
        public void GetConversationAsyncTest()
        {
            // Arrange
            // https://nsubstitute.github.io/
            var contextMock = Substitute.For<ApplicationDbContext>();
            // contextMock.ChatMessages.Returns("-");
            contextProvider.Context = contextMock;

            // Act
            var result = chatService.GetConversationAsync;
            // Assert
            Assert.AreEqual(result,"????????");
        }
    }
}

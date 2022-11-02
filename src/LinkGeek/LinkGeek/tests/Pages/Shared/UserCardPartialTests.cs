using LinkGeek.AppIdentity;
using LinkGeek.Pages.Shared;

namespace LinkGeek.tests.Pages.Shared
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UserCardPartialTests
    {
        private ApplicationUser _currentUser;
        private ApplicationUser _otherUser;
        private UserCardPartial partial;

        [TestInitialize]
        public void SetUp()
        {
            // common Arrange
            _currentUser = new ApplicationUser();
            _otherUser = new ApplicationUser();

            partial = new UserCardPartial
            {
                CurrentUser = _currentUser,
                DisplayedUser = _otherUser
            };
        }

        [TestMethod]
        public void AreCurrentlyFriendsTestTrue()
        {
            // Arrange
            _currentUser.Friends.Add(_otherUser);
            _otherUser.Friends.Add(_currentUser);
            // Act
            bool result = partial.AreCurrentlyFriends();
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AreCurrentlyFriendsTestFalse()
        {
            // Arrange
            // Act
            bool result = partial.AreCurrentlyFriends();
            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AreCurrentlyPendingFriendsTrue()
        {
            // Arrange
            _currentUser.Friends.Add(_otherUser);
            // Act
            bool result = partial.AreCurrentlyFriends();
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AreCurrentlyPendingFriendsFalse()
        {
            // Arrange
            _currentUser.Friends.Add(_otherUser);
            _otherUser.Friends.Add(_currentUser);
            // Act
            bool result = partial.AreCurrentlyFriends();
            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAddedTrue()
        {
            // Arrange
            _otherUser.Friends.Add(_currentUser);
            // Act
            bool result = partial.IsAdded();
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAddedFalse()
        {
            // Arrange
            _currentUser.Friends.Add(_otherUser);
            // Act
            bool result = partial.IsAdded();
            // Assert
            Assert.IsFalse(result);
        }
    }
}

using LinkGeek.AppIdentity;
using LinkGeek.Pages.Shared;
using Microsoft.AspNetCore.Components.Authorization;

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
            
            //AuthenticationStateProvider.

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
        public void AreCurrentlyPendingFriendsTrueOnlyCurrentUserAdded()
        {
            // Arrange
            _currentUser.Friends.Add(_otherUser);
            // Act
            bool result = partial.AreCurrentlyFriends();
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AreCurrentlyPendingFriendsTrue()
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
        public void FriendRequestReceivedTrue()
        {
            // Arrange
            _currentUser.ReceivedFriendRequests.Add(_otherUser);
            // Act
            bool result = partial.FriendRequestReceived();
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FriendRequestReceivedFalse()
        {
            // Arrange
            _currentUser.Friends.Add(_otherUser);
            // Act
            bool result = partial.FriendRequestReceived();
            // Assert
            Assert.IsFalse(result);
        }
    }
}

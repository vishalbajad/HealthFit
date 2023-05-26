using NUnit.Framework;
using Data_Layer.InterfaceCollections.Repository;
using Data_Layer.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthFit.Object_Provider.Model;

namespace Data_Layer.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Test]
        public void GetUser_WithValidId_ShouldReturnUser()
        {
            int userId = 1;
            User expectedUser = new User { UserId = userId, FullName = "John" };
            _userRepositoryMock.Setup(repo => repo.GetUser(userId)).Returns(Task.FromResult(expectedUser));

            User actualUser = _userService.GetUser(userId);

            Assert.IsNotNull(actualUser);
            Assert.AreEqual(expectedUser.UserId, actualUser.UserId);
        }

        [Test]
        public void GetUser_WithInvalidId_ShouldReturnNull()
        {
            int userId = 100;
            _userRepositoryMock.Setup(repo => repo.GetUser(userId)).Returns(Task.FromResult<User>(null));

            User actualUser = _userService.GetUser(userId);

            Assert.IsNull(actualUser);
        }

    }
}

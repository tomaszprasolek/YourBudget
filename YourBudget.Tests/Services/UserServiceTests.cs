using System;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using YourBudget.Core.Domain;
using YourBudget.Core.Repositories;
using YourBudget.Infrastructure.Services;

namespace YourBudget.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> userRepository;
        private readonly Mock<IMapper> mapper;

        private readonly Mock<IEncrypter> encrypter;

        private string email = "user12@email.com";

        public UserServiceTests()
        {
            userRepository = new Mock<IUserRepository>();
            mapper = new Mock<IMapper>();
            encrypter = new Mock<IEncrypter>();
        }

        [Fact]
        public async Task register_async_should_invoke_add_async_on_repository()
        {
            // Act
            await Execute();

            // Assert
            userRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async void register_async_throw_exception_when_user_already_exists()
        {
            // Arrange
            userRepository.Setup(ga => ga.GetAsync(email)).ReturnsAsync(new User(email, "user12", "secret12", Guid.NewGuid().ToString("N")));

            // TODO: Czy tak może zostać? Popawić?
            encrypter.Setup(e => e.GetSalt(It.IsAny<string>())).Returns(It.IsAny<string>());
            encrypter.Setup(e => e.GetHash(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<string>());

            // Act
            await Execute();

            Exception exc = null;
            try
            {
                await Execute();
            }
            catch (Exception e)
            {
                exc = e;
            }
            
            // Assert
            Assert.NotNull(exc);
        }

        private async Task Execute()
        {
            var userService = new UserService(userRepository.Object, encrypter.Object, mapper.Object);
            await userService.RegisterAsync(email, "user12", "secret12");
        }
    }
}
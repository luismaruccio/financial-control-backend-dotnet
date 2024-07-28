using FinancialControl.Domain.Entities;
using FinancialControl.Infra.Repositories;
using FinancialControl.Infra.Tests.Repositories.Shared;

namespace FinancialControl.Infra.Tests.Repositories
{
    public class UserRepositoryTests : TestBase
    {
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            _repository = new UserRepository(Context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddUser()
        {
            var user = new User
            {
                Name = "Test User",
                Email = "testuser@example.com",
                PasswordHash = "hashedpassword"
            };

            await _repository.AddAsync(user);
            var users = await _repository.GetAllAsync();

            Assert.Single(users);
            Assert.Equal("Test User", users.First().Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser()
        {
            var user = new User
            {
                Name = "Test User",
                Email = "testuser@example.com",
                PasswordHash = "hashedpassword"
            };

            await _repository.AddAsync(user);

            var retrievedUser = await _repository.GetByIdAsync(user.Id);

            Assert.NotNull(retrievedUser);
            Assert.Equal("Test User", retrievedUser.Name);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            var user1 = new User
            {
                Name = "Test User 1",
                Email = "testuser1@example.com",
                PasswordHash = "hashedpassword1"
            };
            var user2 = new User
            {
                Name = "Test User 2",
                Email = "testuser2@example.com",
                PasswordHash = "hashedpassword2"
            };

            await _repository.AddAsync(user1);
            await _repository.AddAsync(user2);

            var users = await _repository.GetAllAsync();

            Assert.Equal(2, users.Count());
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUser()
        {
            var user = new User
            {
                Name = "Test User",
                Email = "testuser@example.com",
                PasswordHash = "hashedpassword",
                EmailVerified = true
            };

            await _repository.AddAsync(user);

            user.Name = "Teste User Updated";
            await _repository.UpdateAsync(user);
            var updatedUser = await _repository.GetByIdAsync(user.Id);

            Assert.Equal("Teste User Updated", updatedUser!.Name);

        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveUser()
        {
            var user = new User
            {
                Name = "Test User",
                Email = "testuser@example.com",
                PasswordHash = "hashedpassword"
            };

            await _repository.AddAsync(user);

            await _repository.DeleteAsync(user);
            var users = await _repository.GetAllAsync();

            Assert.Empty(users);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldGetUserByEmail()
        {
            var user = new User
            {
                Name = "Test User",
                Email = "testuser@example.com",
                PasswordHash = "hashedpassword"
            };

            await _repository.AddAsync(user);

            var retornedUser = await _repository.GetByEmailAsync(user.Email);

            Assert.IsType<User>(retornedUser);
        }
    }
}

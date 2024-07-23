using FinancialControl.Domain.Entities;
using FinancialControl.Infra.Repositories;
using FinancialControl.Infra.Tests.Utilities;

namespace FinancialControl.Infra.Tests.Repositories
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task AddAsync_ShouldAddUser()
        {
            var context = InMemoryDbContextFactory.Create();
            var repository = new UserRepository(context);
            var user = new User
            {
                Name = "Test User",
                Email = "testuser@example.com",
                PasswordHash = "hashedpassword"
            };

            await repository.AddAsync(user);
            var users = await repository.GetAllAsync();

            Assert.Single(users);
            Assert.Equal("Test User", users.First().Name);

            InMemoryDbContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser()
        {
            var context = InMemoryDbContextFactory.Create();
            var repository = new UserRepository(context);
            var user = new User
            {
                Name = "Test User",
                Email = "testuser@example.com",
                PasswordHash = "hashedpassword"
            };

            await repository.AddAsync(user);

            var retrievedUser = await repository.GetByIdAsync(user.Id);

            Assert.NotNull(retrievedUser);
            Assert.Equal("Test User", retrievedUser.Name);

            InMemoryDbContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            var context = InMemoryDbContextFactory.Create();
            var repository = new UserRepository(context);
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

            await repository.AddAsync(user1);
            await repository.AddAsync(user2);

            var users = await repository.GetAllAsync();

            Assert.Equal(2, users.Count());

            InMemoryDbContextFactory.Destroy(context);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUser()
        {
            var context = InMemoryDbContextFactory.Create();
            var repository = new UserRepository(context);
            var user = new User
            {
                Name = "Test User",
                Email = "testuser@example.com",
                PasswordHash = "hashedpassword",
                EmailVerified = true
            };

            await repository.AddAsync(user);

            user.Name = "Teste User Updated";
            await repository.UpdateAsync(user);
            var updatedUser = await repository.GetByIdAsync(user.Id);

            Assert.Equal("Teste User Updated", updatedUser!.Name);

            InMemoryDbContextFactory.Destroy(context);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveUser()
        {
            var context = InMemoryDbContextFactory.Create();
            var repository = new UserRepository(context);
            var user = new User
            {
                Name = "Test User",
                Email = "testuser@example.com",
                PasswordHash = "hashedpassword"
            };

            await repository.AddAsync(user);

            await repository.DeleteAsync(user);
            var users = await repository.GetAllAsync();

            Assert.Empty(users);

            InMemoryDbContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldGetUserByEmail()
        {
            var context = InMemoryDbContextFactory.Create();
            var repository = new UserRepository(context);
            var user = new User
            {
                Name = "Test User",
                Email = "testuser@example.com",
                PasswordHash = "hashedpassword"
            };

            await repository.AddAsync(user);

            var retornedUser = await repository.GetByEmailAsync(user.Email);

            Assert.IsType<User>(retornedUser);

            InMemoryDbContextFactory.Destroy(context);
        }
    }
}

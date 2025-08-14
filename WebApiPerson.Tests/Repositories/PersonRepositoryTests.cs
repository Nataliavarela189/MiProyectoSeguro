using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using WebApiPerson.Context;
using WebApiPerson.Models;
using WebApiPerson.Repositories;
using System.Linq;

namespace WebApiPerson.Tests.Repositories
{
    public class PersonRepositoryTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAndGetById_ReturnsCorrectPerson()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PersonRepository(context);

            var person = new Person
            {
                Id = 1,
                Name = "Test User",
                Identification = "1234",
                Age = 25,
                Gender = "M",
                IsActive = true
            };

            // Act
            await repository.AddAsync(person);
            var result = await repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test User", result.Name);
        }

        [Fact]
        public async Task UpdatePerson_ChangesAreSaved()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PersonRepository(context);

            var person = new Person
            {
                Id = 2,
                Name = "User Old",
                Identification = "5678",
                Age = 30,
                Gender = "F",
                IsActive = true
            };

            await repository.AddAsync(person);

            // Act
            person.Name = "User New";
            await repository.UpdateAsync(person);

            var updated = await repository.GetByIdAsync(2);

            // Assert
            Assert.Equal("User New", updated.Name);
        }

        [Fact]
        public async Task DeletePerson_RemovesPerson()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PersonRepository(context);

            var person = new Person
            {
                Id = 3,
                Name = "To Delete",
                Identification = "9999",
                Age = 50,
                Gender = "M",
                IsActive = true
            };

            await repository.AddAsync(person);

            // Act
            await repository.DeleteAsync(person);
            var deleted = await repository.GetByIdAsync(3);

            // Assert
            Assert.Null(deleted);
        }

        [Fact]
        public async Task GetAll_ReturnsAllPersons()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PersonRepository(context);

            var person1 = new Person
            {
                Id = 4,
                Name = "User 1",
                Identification = "1111",
                Age = 20,
                Gender = "F",
                IsActive = true
            };
            var person2 = new Person
            {
                Id = 5,
                Name = "User 2",
                Identification = "2222",
                Age = 22,
                Gender = "M",
                IsActive = true
            };

            await repository.AddAsync(person1);
            await repository.AddAsync(person2);

            // Act
            var all = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(all);
            Assert.True(all.Count() >= 2);
        }
    }
}


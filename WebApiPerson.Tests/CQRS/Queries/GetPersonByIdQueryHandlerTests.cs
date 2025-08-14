using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using WebApiPerson.CQRS.Queries.GetPersonById;
using WebApiPerson.Models;
using WebApiPerson.Repositories;

namespace WebApiPerson.Tests.CQRS.Queries
{
    public class GetPersonByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Return_Person_When_Found()
        {
            // Arrange
            var mockRepo = new Mock<IPersonRepository>();

            var person = new Person
            {
                Id = 1,
                Name = "Natalia",
                Identification = "12345678",
                Age = 30,
                Gender = "F",
                IsActive = true,
                Drives = true,
                WearsGlasses = false,
                HasDiabetes = false,
                OtherDiseases = null
            };

            mockRepo.Setup(r => r.GetByIdAsync(person.Id))
                    .ReturnsAsync(person);

            var handler = new GetPersonByIdQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(new GetPersonByIdQuery(person.Id), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(person.Id, result.Id);
            Assert.Equal(person.Name, result.Name);
            mockRepo.Verify(r => r.GetByIdAsync(person.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Null_When_Not_Found()
        {
            // Arrange
            var mockRepo = new Mock<IPersonRepository>();

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync((Person?)null);

            var handler = new GetPersonByIdQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(new GetPersonByIdQuery(99), CancellationToken.None);

            // Assert
            Assert.Null(result);
            mockRepo.Verify(r => r.GetByIdAsync(99), Times.Once);
        }
    }
}

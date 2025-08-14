using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using WebApiPerson.CQRS.Commands.DeletePerson;
using WebApiPerson.Models;
using WebApiPerson.Repositories;

namespace WebApiPerson.Tests.CQRS.Commands
{
    public class DeletePersonCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Delete_Person_And_Return_True_When_Person_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IPersonRepository>();

            var person = new Person
            {
                Id = 1,
                Name = "Test Name",
                Identification = "12345678",
                Age = 30,
                Gender = "F",
                IsActive = true,
                Drives = false,
                WearsGlasses = false,
                HasDiabetes = false,
                OtherDiseases = null
            };

            mockRepo.Setup(r => r.GetByIdAsync(person.Id))
                    .ReturnsAsync(person);

            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<Person>()))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

            var handler = new DeletePersonCommandHandler(mockRepo.Object);
            var command = new DeletePersonCommand(person.Id);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            mockRepo.Verify(r => r.GetByIdAsync(person.Id), Times.Once);
            mockRepo.Verify(r => r.DeleteAsync(It.Is<Person>(p => p == person)), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_False_When_Person_Does_Not_Exist()
        {
            // Arrange
            var mockRepo = new Mock<IPersonRepository>();

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync((Person)null);

            var handler = new DeletePersonCommandHandler(mockRepo.Object);
            var command = new DeletePersonCommand(99);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            mockRepo.Verify(r => r.GetByIdAsync(99), Times.Once);
            mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Person>()), Times.Never);
        }
    }
}

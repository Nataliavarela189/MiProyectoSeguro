using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using WebApiPerson.CQRS.Commands.UpdatePerson;
using WebApiPerson.Models;
using WebApiPerson.Repositories;

namespace WebApiPerson.Tests.CQRS.Commands
{
    public class UpdatePersonCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Update_Person_And_Return_True_When_Person_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IPersonRepository>();

            var existingPerson = new Person
            {
                Id = 1,
                Name = "Old Name",
                Identification = "87654321",
                Age = 25,
                Gender = "F",
                IsActive = true,
                Drives = false,
                WearsGlasses = true,
                HasDiabetes = false,
                OtherDiseases = "None"
            };

            var updatedPerson = new Person
            {
                Id = 1,
                Name = "Updated Name",
                Identification = "87654321",
                Age = 26,
                Gender = "F",
                IsActive = false,
                Drives = true,
                WearsGlasses = false,
                HasDiabetes = true,
                OtherDiseases = "Asthma"
            };

            mockRepo.Setup(r => r.GetByIdAsync(existingPerson.Id))
                    .ReturnsAsync(existingPerson);

            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Person>()))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

            var handler = new UpdatePersonCommandHandler(mockRepo.Object);
            var command = new UpdatePersonCommand(updatedPerson);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);

            mockRepo.Verify(r => r.GetByIdAsync(updatedPerson.Id), Times.Once);

            mockRepo.Verify(r => r.UpdateAsync(It.Is<Person>(p =>
                p.Id == updatedPerson.Id &&
                p.Name == updatedPerson.Name &&
                p.Age == updatedPerson.Age &&
                p.IsActive == updatedPerson.IsActive &&
                p.Drives == updatedPerson.Drives &&
                p.WearsGlasses == updatedPerson.WearsGlasses &&
                p.HasDiabetes == updatedPerson.HasDiabetes &&
                p.OtherDiseases == updatedPerson.OtherDiseases
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_False_When_Person_Does_Not_Exist()
        {
            // Arrange
            var mockRepo = new Mock<IPersonRepository>();

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync((Person)null);

            var handler = new UpdatePersonCommandHandler(mockRepo.Object);

            var command = new UpdatePersonCommand(new Person 
            {
                Id = 99,
                Name = "NombreFalso",
                Identification = "00000000",
                Age = 0,
                Gender = "U",
                IsActive = false,
                Drives = false,
                WearsGlasses = false,
                HasDiabetes = false,
                OtherDiseases = null
            });

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);

            mockRepo.Verify(r => r.GetByIdAsync(99), Times.Once);

            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Person>()), Times.Never);
        }
    }
}

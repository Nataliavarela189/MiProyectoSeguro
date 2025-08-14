using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using WebApiPerson.CQRS.Commands.CreatePerson;
using WebApiPerson.Models;
using WebApiPerson.Repositories;

namespace WebApiPerson.Tests.CQRS.Commands
{
    public class CreatePersonCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Call_AddAsync_And_Return_Person()
        {
            // Arrange
            var mockRepo = new Mock<IPersonRepository>();

            // Setup para AddAsync: no retorna nada, solo se espera que se llame
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Person>()))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

            var handler = new CreatePersonCommandHandler(mockRepo.Object);

            var person = new Person
            {
                Name = "Natalia Varela",
                Identification = "12345678",
                Age = 30,
                Gender = "F",
                IsActive = true,
                Drives = true,
                WearsGlasses = false,
                HasDiabetes = false,
                OtherDiseases = null
            };

            var command = new CreatePersonCommand(person);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.AddAsync(It.Is<Person>(p => p == person)), Times.Once);

            Assert.Equal(person, result);
        }
    }
}

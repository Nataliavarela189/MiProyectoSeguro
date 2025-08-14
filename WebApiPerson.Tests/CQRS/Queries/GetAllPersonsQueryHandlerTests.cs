using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using WebApiPerson.CQRS.Queries.GetAllPersons;
using WebApiPerson.Models;
using WebApiPerson.Repositories;

namespace WebApiPerson.Tests.CQRS.Queries
{
    public class GetAllPersonsQueryHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Return_All_Persons()
        {
            // Arrange
            var mockRepo = new Mock<IPersonRepository>();

            var persons = new List<Person>
            {
                new Person
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
                },
                new Person
                {
                    Id = 2,
                    Name = "Carlos",
                    Identification = "87654321",
                    Age = 40,
                    Gender = "M",
                    IsActive = true,
                    Drives = false,
                    WearsGlasses = true,
                    HasDiabetes = true,
                    OtherDiseases = "Hypertension"
                }
            };

            mockRepo.Setup(r => r.GetAllAsync())
                    .ReturnsAsync(persons);

            var handler = new GetAllPersonsQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(new GetAllPersonsQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(persons, result);
            mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
        }
    }
}

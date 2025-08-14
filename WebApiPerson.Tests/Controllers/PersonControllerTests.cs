using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using MediatR;
using WebApiPerson.Controllers;
using WebApiPerson.Models;
using WebApiPerson.CQRS.Commands.CreatePerson;
using WebApiPerson.CQRS.Commands.DeletePerson;
using WebApiPerson.CQRS.Commands.UpdatePerson;
using WebApiPerson.CQRS.Queries.GetAllPersons;
using WebApiPerson.CQRS.Queries.GetPersonById;

namespace WebApiPerson.Tests.Controllers
{
    public class PersonControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly PersonController _controller;

        public PersonControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new PersonController(_mockMediator.Object);
        }

        [Fact]
        public async Task GetPersons_ReturnsOkWithList()
        {
            // Arrange
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

            _mockMediator.Setup(m => m.Send(It.IsAny<GetAllPersonsQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(persons);

            // Act
            var result = await _controller.GetPersons();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnPersons = Assert.IsAssignableFrom<IEnumerable<Person>>(okResult.Value);
            Assert.Equal(2, ((List<Person>)returnPersons).Count);
        }

        [Fact]
        public async Task GetPerson_ReturnsOk_WhenFound()
        {
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

            _mockMediator.Setup(m => m.Send(It.Is<GetPersonByIdQuery>(q => q.Id == 1), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(person);

            var result = await _controller.GetPerson(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnPerson = Assert.IsType<Person>(okResult.Value);
            Assert.Equal(person.Id, returnPerson.Id);
        }

        [Fact]
        public async Task GetPerson_ReturnsNotFound_WhenNull()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetPersonByIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Person?)null);

            var result = await _controller.GetPerson(99);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostPerson_ReturnsCreatedAtAction()
        {
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

            _mockMediator.Setup(m => m.Send(It.IsAny<CreatePersonCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(person);

            var result = await _controller.PostPerson(person);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnPerson = Assert.IsType<Person>(createdResult.Value);
            Assert.Equal(person.Id, returnPerson.Id);
            Assert.Equal(nameof(_controller.GetPerson), createdResult.ActionName);
        }

        [Fact]
        public async Task PutPerson_ReturnsBadRequest_WhenIdMismatch()
        {
            var person = new Person
            {
                Id = 1,
                Name = "Test",
                Identification = "0000",
                Age = 0,
                Gender = "U",
                IsActive = false,
                Drives = false,
                WearsGlasses = false,
                HasDiabetes = false,
                OtherDiseases = null
            };

            var result = await _controller.PutPerson(2, person);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutPerson_ReturnsNotFound_WhenUpdateFails()
        {
            var person = new Person
            {
                Id = 1,
                Name = "Test",
                Identification = "0000",
                Age = 0,
                Gender = "U",
                IsActive = false,
                Drives = false,
                WearsGlasses = false,
                HasDiabetes = false,
                OtherDiseases = null
            };

            _mockMediator.Setup(m => m.Send(It.IsAny<UpdatePersonCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);

            var result = await _controller.PutPerson(1, person);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutPerson_ReturnsNoContent_WhenUpdateSucceeds()
        {
            var person = new Person
            {
                Id = 1,
                Name = "Test",
                Identification = "0000",
                Age = 0,
                Gender = "U",
                IsActive = false,
                Drives = false,
                WearsGlasses = false,
                HasDiabetes = false,
                OtherDiseases = null
            };

            _mockMediator.Setup(m => m.Send(It.IsAny<UpdatePersonCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var result = await _controller.PutPerson(1, person);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeletePerson_ReturnsNotFound_WhenDeleteFails()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<DeletePersonCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);

            var result = await _controller.DeletePerson(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeletePerson_ReturnsNoContent_WhenDeleteSucceeds()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<DeletePersonCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var result = await _controller.DeletePerson(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}


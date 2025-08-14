using MediatR;
using WebApiPerson.Models;

namespace WebApiPerson.CQRS.Commands.CreatePerson
{
    public record CreatePersonCommand(Person Person) : IRequest<Person>;
}

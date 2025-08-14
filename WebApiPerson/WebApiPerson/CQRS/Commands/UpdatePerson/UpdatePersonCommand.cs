using MediatR;
using WebApiPerson.Models;

namespace WebApiPerson.CQRS.Commands.UpdatePerson
{
    public record UpdatePersonCommand(Person Person) : IRequest<bool>;
}

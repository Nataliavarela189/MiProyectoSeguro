using MediatR;

namespace WebApiPerson.CQRS.Commands.DeletePerson
{
    public record DeletePersonCommand(int Id) : IRequest<bool>;
}

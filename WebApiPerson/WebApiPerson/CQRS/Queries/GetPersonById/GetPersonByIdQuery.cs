using MediatR;
using WebApiPerson.Models;

namespace WebApiPerson.CQRS.Queries.GetPersonById
{
    public record GetPersonByIdQuery(int Id) : IRequest<Person?>;
}

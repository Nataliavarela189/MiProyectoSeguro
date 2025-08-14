using MediatR;
using WebApiPerson.Models;
using System.Collections.Generic;

namespace WebApiPerson.CQRS.Queries.GetAllPersons
{
    public record GetAllPersonsQuery() : IRequest<IEnumerable<Person>>;
}

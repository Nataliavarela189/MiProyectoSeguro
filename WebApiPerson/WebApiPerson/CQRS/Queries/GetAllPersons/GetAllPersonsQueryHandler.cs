using MediatR;
using WebApiPerson.Models;
using WebApiPerson.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiPerson.CQRS.Queries.GetAllPersons
{
    public class GetAllPersonsQueryHandler : IRequestHandler<GetAllPersonsQuery, IEnumerable<Person>>
    {
        private readonly IPersonRepository _personRepository;

        public GetAllPersonsQueryHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<IEnumerable<Person>> Handle(GetAllPersonsQuery request, CancellationToken cancellationToken)
        {
            return await _personRepository.GetAllAsync();
        }
    }
}

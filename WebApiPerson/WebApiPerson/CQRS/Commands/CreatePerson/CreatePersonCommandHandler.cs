using MediatR;
using WebApiPerson.Models;
using WebApiPerson.Repositories;

namespace WebApiPerson.CQRS.Commands.CreatePerson
{
    public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, Person>
    {
        private readonly IPersonRepository _personRepository;

        public CreatePersonCommandHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<Person> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            await _personRepository.AddAsync(request.Person);
            return request.Person;
        }
    }
}


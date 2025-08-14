using MediatR;
using WebApiPerson.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiPerson.CQRS.Commands.DeletePerson
{
    public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, bool>
    {
        private readonly IPersonRepository _personRepository;

        public DeletePersonCommandHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<bool> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            var person = await _personRepository.GetByIdAsync(request.Id);

            if (person == null)
                return false;

            await _personRepository.DeleteAsync(person);

            return true;
        }
    }
}


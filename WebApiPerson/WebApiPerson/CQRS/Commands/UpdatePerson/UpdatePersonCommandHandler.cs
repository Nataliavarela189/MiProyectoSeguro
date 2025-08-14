using MediatR;
using WebApiPerson.Models;
using WebApiPerson.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiPerson.CQRS.Commands.UpdatePerson
{
    public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, bool>
    {
        private readonly IPersonRepository _personRepository;

        public UpdatePersonCommandHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<bool> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            var existingPerson = await _personRepository.GetByIdAsync(request.Person.Id);

            if (existingPerson == null)
                return false;

            // Actualizar propiedades
            existingPerson.Name = request.Person.Name;
            existingPerson.Age = request.Person.Age;
            existingPerson.Identification = request.Person.Identification;
            existingPerson.Gender = request.Person.Gender;
            existingPerson.IsActive = request.Person.IsActive;
            existingPerson.Drives = request.Person.Drives;
            existingPerson.WearsGlasses = request.Person.WearsGlasses;
            existingPerson.HasDiabetes = request.Person.HasDiabetes;
            existingPerson.OtherDiseases = request.Person.OtherDiseases;

            await _personRepository.UpdateAsync(existingPerson);

            return true;
        }
    }
}


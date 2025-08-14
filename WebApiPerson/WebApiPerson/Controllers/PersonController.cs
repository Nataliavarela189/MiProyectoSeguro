using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiPerson.CQRS.Commands.CreatePerson;
using WebApiPerson.CQRS.Commands.DeletePerson;
using WebApiPerson.CQRS.Commands.UpdatePerson;
using WebApiPerson.CQRS.Queries.GetAllPersons;
using WebApiPerson.CQRS.Queries.GetPersonById;
using WebApiPerson.Models;

namespace WebApiPerson.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PersonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Person
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            var persons = await _mediator.Send(new GetAllPersonsQuery());
            return Ok(persons);
        }

        // GET: api/Person/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _mediator.Send(new GetPersonByIdQuery(id));
            if (person == null)
                return NotFound();
            return Ok(person);
        }

        // POST: api/Person
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            var createdPerson = await _mediator.Send(new CreatePersonCommand(person));
            return CreatedAtAction(nameof(GetPerson), new { id = createdPerson.Id }, createdPerson);
        }

        // PUT: api/Person/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            if (id != person.Id)
                return BadRequest();

            var result = await _mediator.Send(new UpdatePersonCommand(person));
            if (!result)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Person/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var result = await _mediator.Send(new DeletePersonCommand(id));
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}


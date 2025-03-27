using Microsoft.AspNetCore.Mvc;
using PhoneDirectory.Domain.Entities;
using PhoneDirectory.Domain.Interfaces.Services;

namespace PhoneDirectory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpPost]
        public async Task<ActionResult<Person>> CreatePerson([FromBody] Person person)
        {
            var result = await _personService.CreatePersonAsync(person);
            return CreatedAtAction(nameof(GetPerson), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Person>> UpdatePerson(Guid id, [FromBody] Person person)
        {
            if (id != person.Id)
                return BadRequest();

            var result = await _personService.UpdatePersonAsync(person);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePerson(Guid id)
        {
            await _personService.DeletePersonAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(Guid id)
        {
            var person = await _personService.GetPersonByIdAsync(id);
            return Ok(person);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetAllPersons()
        {
            var persons = await _personService.GetAllPersonsAsync();
            return Ok(persons);
        }

        [HttpPost("{id}/contacts")]
        public async Task<ActionResult<Person>> AddContact(Guid id, [FromBody] Contact contact)
        {
            var result = await _personService.AddContactAsync(id, contact);
            return Ok(result);
        }

        [HttpDelete("{id}/contacts/{contactId}")]
        public async Task<ActionResult<Person>> RemoveContact(Guid id, Guid contactId)
        {
            var result = await _personService.RemoveContactAsync(id, contactId);
            return Ok(result);
        }

        // Implement other endpoints...
    }
} 
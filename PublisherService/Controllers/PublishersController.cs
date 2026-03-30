using Microsoft.AspNetCore.Mvc;
using PublisherService.Models;
using PublisherService.Repositories;    

namespace PublisherService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherRepository _repository;

        public PublishersController(IPublisherRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var publishers = await _repository.GetAllAsync();
            return Ok(publishers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var publisher = await _repository.GetByIdAsync(id);
            if (publisher is null) return NotFound($"Publisher with ID {id} not found.");
            return Ok(publisher);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Publisher publisher)
        {
            var created = await _repository.CreateAsync(publisher);
            return CreatedAtAction(nameof(GetById), new { id = publisher.Id }, publisher);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Publisher publisher)
        {
            if (id != publisher.Id) 
                return BadRequest("ID in URL does not match ID in body.");

            var exists = await _repository.GetByIdAsync(id);    
            if (exists == null) 
                return NotFound($"Publisher with ID {id} not found.");

            exists.Name = publisher.Name;
            exists.Country = publisher.Country;
            exists.FoundedYear = publisher.FoundedYear;

            try
            {
                await _repository.UpdateAsync(exists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the publisher: {ex.Message}");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _repository.GetByIdAsync(id);
            if (exists == null) 
                return NotFound($"Publisher with ID {id} not found.");

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using VideoGames.DAL.Entities;
using VideoGames.DAL.Repositories;

namespace VideoGames.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeveloperController : ControllerBase
{
    private readonly DeveloperRepository _developerRepository;

    public DeveloperController(DeveloperRepository developerRepository)
    {
        _developerRepository = developerRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeveloperDto>>> GetAll()
    {
        var developers = await _developerRepository.GetAll();
        var dtos = developers.Select(d => new DeveloperDto
        {
            Id = d.Id,
            Name = d.Name
        });

        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DeveloperDto>> GetById(int id)
    {
        var developer = await _developerRepository.GetById(id);
        if (developer == null)
        {
            return NotFound($"Розробника з ID {id} не знайдено.");
        }

        var dto = new DeveloperDto
        {
            Id = developer.Id,
            Name = developer.Name
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<DeveloperDto>> Create([FromBody] DeveloperDto dto)
    {
        var developer = new Developer
        {
            Name = dto.Name!
        };

        await _developerRepository.Create(developer);

        dto.Id = developer.Id;
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DeveloperDto dto)
    {
        var existingDeveloper = await _developerRepository.GetById(id);
        if (existingDeveloper == null)
        {
            return NotFound($"Розробника з ID {id} не знайдено.");
        }

        existingDeveloper.Name = dto.Name!;

        await _developerRepository.Update(existingDeveloper);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existingDeveloper = await _developerRepository.GetById(id);
        if (existingDeveloper == null)
        {
            return NotFound($"Розробника з ID {id} не знайдено.");
        }

        await _developerRepository.Delete(id);
        return NoContent();
    }
}
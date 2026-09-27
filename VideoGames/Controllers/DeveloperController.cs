using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VideoGames.BLL.Dtos.Developer;
using VideoGames.BLL.Services;
using VideoGames.Extensions;

namespace VideoGames.Controllers;

[ApiController]
[Route("api/developer")]
public class DeveloperController : ControllerBase
{
    private readonly DeveloperService _developerService;
    private readonly IValidator<CreateDeveloperDto> _validatorCreate;
    private readonly IValidator<UpdateDeveloperDto> _validatorUpdate;

    public DeveloperController(
        DeveloperService developerService,
        IValidator<CreateDeveloperDto> validatorCreate,
        IValidator<UpdateDeveloperDto> validatorUpdate)
    {
        _developerService = developerService;
        _validatorCreate = validatorCreate;
        _validatorUpdate = validatorUpdate;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
    {
        var response = await _developerService.GetAllAsync(ct);
        return this.GetHttpResponse(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
    {
        var response = await _developerService.GetByIdAsync(id, ct);
        return this.GetHttpResponse(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateDeveloperDto dto,
        CancellationToken ct = default)
    {
        var validationResult = await _validatorCreate.ValidateAsync(dto, ct);

        if (!validationResult.IsValid)
        {
            return this.ValidationResponse(validationResult);
        }

        var response = await _developerService.CreateAsync(dto, ct);

        return this.GetHttpResponse(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update(
        [FromBody] UpdateDeveloperDto dto,
        CancellationToken ct = default)
    {
        var validationResult = await _validatorUpdate.ValidateAsync(dto, ct);

        if (!validationResult.IsValid)
        {
            return this.ValidationResponse(validationResult);
        }

        var response = await _developerService.UpdateAsync(dto, ct);

        return this.GetHttpResponse(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
    {
        var response = await _developerService.DeleteAsync(id, ct);

        return this.GetHttpResponse(response);
    }
}
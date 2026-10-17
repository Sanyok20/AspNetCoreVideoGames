using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VideoGames.BLL.Dtos.Role;
using VideoGames.BLL.Services;
using VideoGames.Extensions;

namespace VideoGames.Controllers
{
    [ApiController]
    [Route("api/role")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;
        private readonly IValidator<CreateRoleDto> _createValidator;
        private readonly IValidator<UpdateRoleDto> _updateValidator;

        public RoleController(RoleService roleService, IValidator<CreateRoleDto> createValidator, IValidator<UpdateRoleDto> updateValidator)
        {
            _roleService = roleService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken ct = default)
        {
            var response = await _roleService.GetAllAsync(ct);
            return this.GetHttpResponse(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken ct = default)
        {
            var response = await _roleService.GetByIdAsync(id, ct);
            return this.GetHttpResponse(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, CancellationToken ct = default)
        {
            var response = await _roleService.DeleteAsync(id, ct);
            return this.GetHttpResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateRoleDto dto, CancellationToken ct = default)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
            {
                return this.ValidationResponse(validation);
            }

            var response = await _roleService.CreateAsync(dto, ct);
            return this.GetHttpResponse(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateRoleDto dto, CancellationToken ct = default)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
            {
                return this.ValidationResponse(validation);
            }

            var response = await _roleService.UpdateAsync(dto, ct);
            return this.GetHttpResponse(response);
        }
    }
}
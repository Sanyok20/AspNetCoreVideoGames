using FluentValidation;
using VideoGames.BLL.Dtos.Role;
using VideoGames.DAL.Repositories;

namespace VideoGames.BLL.Validators.Role
{
    public class CreateRoleValidator : AbstractValidator<CreateRoleDto>
    {
        public CreateRoleValidator(RoleRepository roleRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ім'я ролі не може бути порожнім")
                .MustAsync(async (name, ct) => !await roleRepository.IsExistsAsync(name, ct))
                .WithMessage(x => $"Роль з іменем {x.Name} вже існує");
        }
    }
}
using FluentValidation;
using VideoGames.BLL.Dtos.Role;
using VideoGames.DAL.Repositories;

namespace VideoGames.BLL.Validators.Role
{
    public class UpdateRoleValidator : AbstractValidator<UpdateRoleDto>
    {
        public UpdateRoleValidator(RoleRepository roleRepository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id ролі повинен бути більший за 0");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ім'я ролі не може бути порожнім")
                .MustAsync(async (name, ct) => !await roleRepository.IsExistsAsync(name, ct))
                .WithMessage(x => $"Роль з іменем {x.Name} вже існує");
        }
    }
}
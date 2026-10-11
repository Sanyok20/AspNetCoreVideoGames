using FluentValidation;
using VideoGames.BLL.Dtos.Genre;
using VideoGames.DAL.Repositories;

namespace VideoGames.BLL.Validators.Genre
{
    public class CreateGenreValidator : AbstractValidator<CreateGenreDto>
    {
        public CreateGenreValidator(GenreRepository genreRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва жанру не може бути порожньою")
                .MaximumLength(100).WithMessage("Назва жанру не може перевищувати 100 символів")
                .MustAsync(async (x, ct) => !await genreRepository.IsExistsAsync(x, ct))
                .WithMessage(x => $"Жанр з назвою '{x.Name}' вже існує");
        }
    }
}
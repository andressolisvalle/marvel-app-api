using Application.DTOs;
using FluentValidation;

namespace Api.RequestValidator.Request
{
    public class AddFavoriteComicDtoValidator : AbstractValidator<AddFavoriteComicDto>
    {
        public AddFavoriteComicDtoValidator()
        {
            RuleFor(x => x.ComicId)
                .GreaterThan(0).WithMessage("Debe proporcionar un ID de cómic válido.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("El título del cómic es obligatorio.")
                .MaximumLength(200).WithMessage("El título no puede exceder los 200 caracteres.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("La URL de la imagen es obligatoria.")
                .MaximumLength(500).WithMessage("La URL de la imagen no puede exceder los 500 caracteres.");
        }
    }
}

using Application.DTOs;
using FluentValidation;

namespace Api.RequestValidator.Request
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("Debe ser un email válido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

            RuleFor(x => x.Identification)
                .NotEmpty().WithMessage("La identificación es obligatoria.");
        }
    }
}

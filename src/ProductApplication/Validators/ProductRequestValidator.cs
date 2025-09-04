using FluentValidation;
using ProductApplication.DTOs;

namespace ProductApplication.Validators
{
    public class ProductRequestValidator : AbstractValidator<ProductRequestDto>
    {
        public ProductRequestValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres.");

            RuleFor(x => x.Estoque)
                .GreaterThanOrEqualTo(0).WithMessage("Estoque não pode ser negativo.");

            RuleFor(x => x.Valor)
                .GreaterThanOrEqualTo(0).WithMessage("Valor não pode ser negativo.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using UsuarioApp.Domain.Entities;

namespace UsuariosApp.Domain.Validators
{
    public class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator()
        {
            RuleFor(u => u.Nome)
                .NotEmpty().WithMessage("O nome do usuário é obrigatório.")
                .Length(6,100).WithMessage("O nome deve ter de 6 a 100 caracteres.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("O endereço de email do usuário é obrigatório.")
                .EmailAddress().WithMessage("Informe um endereço de email válido.");

            RuleFor(u => u.Senha)
                .NotEmpty().WithMessage("A senha do usuário é obrigatória.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                    .WithMessage("A senha deve ter pelo menos uma letra minúscula, uma letra maiúscula, um número, um símbolo e pelo menos 8 caracteres.");
        }
    }
}

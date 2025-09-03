using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using UsuarioApp.Domain.Entities;
using UsuariosApp.Domain.Dtos.Requests;
using UsuariosApp.Domain.Dtos.Responses;
using UsuariosApp.Domain.Interfaces.Services;
using UsuariosApp.Domain.Validators;
using UsuariosApp.Domain.Interfaces.Repositories;
using UsuariosApp.Domain.Helpers;

namespace UsuariosApp.Domain.Services
{
    /// <summary>
    /// Classe para implementar os serviços de domínio de usuário
    /// </summary>
    public class UsuarioService (IUsuarioRepository usuarioRepository, IPerfilRepository perfilRepository) : IUsuarioService
    {
        public CriarUsuarioResponse CriarUsuario(CriarUsuarioRequest request)
        {
            var usuario = new Usuario
            {
                Nome = request.Nome ?? string.Empty,
                Email = request.Email ?? string.Empty,
                Senha = request.Senha ?? string.Empty
            };

            var usuarioValidator = new UsuarioValidator();
            var result = usuarioValidator.Validate(usuario);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            if(usuarioRepository.VerifyEmailExists(usuario.Email))
            {
                throw new ApplicationException("O email informado já está em uso.");
            }

            usuario.Senha = CryptoHelper.GetSHA256(usuario.Senha);
            var perfil = perfilRepository.GetByNome("Operador");
            usuario.PerfilId = perfil.Id;
            usuarioRepository.Add(usuario);

            return new CriarUsuarioResponse(
                Id: usuario.Id,
                Nome: usuario.Nome,
                Email: usuario.Email,
                DataHoraCriacao: DateTime.Now,
                Perfil: perfil.Nome
            );
        }
    }
}




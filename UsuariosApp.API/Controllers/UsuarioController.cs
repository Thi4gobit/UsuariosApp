using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsuariosApp.Domain.Dtos.Requests;
using UsuariosApp.Domain.Dtos.Responses;
using UsuariosApp.Domain.DTOs.Requests;
using UsuariosApp.Domain.Interfaces.Services;

namespace UsuariosApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController (IUsuarioService usuarioService) : ControllerBase
    {
        [HttpPost("criar")]
        [ProducesResponseType(typeof(CriarUsuarioResponse), 201)]
        public IActionResult Criar([FromBody] CriarUsuarioRequest request)
        {
            try
            {
                var response = usuarioService.CriarUsuario(request);
                return StatusCode(201, response);
            }
            catch (ValidationException e)
            {
                return StatusCode(400, e.Errors.Select(msg => msg.ErrorMessage));
            }
            catch (ApplicationException e)
            {
                return StatusCode(422, new { e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { e.Message });
            }
        }

        [HttpPost("autenticar")]
        [ProducesResponseType(typeof(AutenticarUsuarioResponse), 200)]
        public IActionResult Autenticar([FromBody] AutenticarUsuarioRequest request)
        {
            try
            {
                var response = usuarioService.AutenticarUsuario(request);
                return StatusCode(200, response);
            }
            catch(ApplicationException e)
            {
                return StatusCode(401, new { e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { e.Message });
            }
        }
    }
}

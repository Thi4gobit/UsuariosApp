using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using UsuariosApp.Domain.Interfaces.Security;
using UsuariosApp.Infra.Security.Settings;

namespace UsuariosApp.Infra.Security.Services
{
    public class JwtBearerSecurity (JwtBearerSettings settings) : IJwtBearerSecurity
    {
        public string GenerateToken(string user, string role)
        {
            // gerar a chave secreta para assinar o TOKEN JWT (assinatura antifalsificação)
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey ?? string.Empty));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // claim (informações do usuário)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, user),
                new Claim("Role", role)
            };

            //Criação do TOKEN JWT
            var token = new JwtSecurityToken(
                issuer: settings.Issuer, // emissor do token
                audience: settings.Audience, // destinatário do token
                claims: claims, // informações do usuário
                notBefore: DateTime.UtcNow, // tempo de início de validade
                expires: DateTime.UtcNow.AddMinutes(settings.ExpirationInMinutes), // tempo de expiração
                signingCredentials: credentials // assinatura do token
            );

            // retornando o TOKEN JWT
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}

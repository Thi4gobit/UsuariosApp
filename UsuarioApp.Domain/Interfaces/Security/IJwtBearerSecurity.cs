using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsuariosApp.Domain.Interfaces.Security
{
    public interface IJwtBearerSecurity
    {
        string GenerateToken(string email, string role);

    }
}

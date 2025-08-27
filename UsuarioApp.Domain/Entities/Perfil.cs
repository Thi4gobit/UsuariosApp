using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsuariosApp.Domain.Entities;

namespace UsuarioApp.Domain.Entities
{
    public class Perfil
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;

        #region Relacionamentos
        public List<Usuario>? Usuarios { get; set; }
        public List<PerfilPermissao>? Permissoes { get; set; }
        #endregion
    }
}

using Projeto_Gabriel.Domain.Entity;
using Projeto_Gabriel.Repository.Generic;

namespace Projeto_Gabriel.Domain.RepositoryInterface
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Usuario ValidacaoCredencial(Usuario usuario);

        Usuario ValidacaoCredencial(string usuarioNome);

        Usuario AtualizarInfoUsuario(Usuario usuario);

        bool RevokeToken(string usuarioNome);
    }
}
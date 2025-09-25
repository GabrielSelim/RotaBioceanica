using Projeto_Gabriel.Application.Dto;

namespace Projeto_Gabriel.Bussines
{
    public interface ILoginBussines
    {
        TokenDbo ValidarCredenciais(UsuarioDbo usuario);

        TokenDbo ValidarCredenciais(TokenDbo token);

        bool RevokeToken(string usuarioNome);
    }
}
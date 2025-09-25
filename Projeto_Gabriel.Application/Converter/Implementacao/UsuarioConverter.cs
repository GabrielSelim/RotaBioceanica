using Projeto_Gabriel.Application.Converter.Contrato;
using Projeto_Gabriel.Application.Dto;
using Projeto_Gabriel.Domain.Entity;

namespace Projeto_Gabriel.Application.Converter.Implementacao
{
    public class UsuarioConverter : IParser<UsuarioDbo, Usuario>, IParser<Usuario, UsuarioDbo>
    {
        public Usuario Parse(UsuarioDbo origem)
        {
            if (origem == null) return null;

            return new Usuario
            {
                NomeUsuario = origem.NomeUsuario,
                Senha = origem.Senha,
                NomeCompleto = origem.NomeCompleto,
                RefreshToken = origem.RefreshToken,
                RefreshTokenTempoExpiracao = origem.RefreshTokenTempoExpiracao
            };
        }

        public UsuarioDbo Parse(Usuario origem)
        {
            if (origem == null) return null;

            return new UsuarioDbo
            {
                NomeUsuario = origem.NomeUsuario,
                Senha = origem.Senha,
                NomeCompleto = origem.NomeCompleto,
                RefreshToken = origem.RefreshToken,
                RefreshTokenTempoExpiracao = origem.RefreshTokenTempoExpiracao
            };
        }

        public List<Usuario> ParseList(List<UsuarioDbo> origem)
        {
            if (origem == null) return null;

            return origem.Select(item => Parse(item)).ToList();
        }

        public List<UsuarioDbo> ParseList(List<Usuario> origem)
        {
            if (origem == null) return null;

            return origem.Select(item => Parse(item)).ToList();
        }
    }
}

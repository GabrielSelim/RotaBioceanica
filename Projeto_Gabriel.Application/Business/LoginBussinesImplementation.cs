using Projeto_Gabriel.Application.Converter.Implementacao;
using Projeto_Gabriel.Application.Dto;
using Projeto_Gabriel.Domain.RepositoryInterface;
using Projeto_Gabriel.Domain.ServiceInterface;
using Projeto_Gabriel.Shared.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Projeto_Gabriel.Bussines.Implementacoes
{
    public class LoginBussinesImplementation : ILoginBussines
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private TokenConfiguration _configuration;
        private IUsuarioRepository _repository;
        private readonly ITokenService _tokenService;
        private readonly UsuarioConverter _converter;

        public LoginBussinesImplementation(TokenConfiguration configuration, IUsuarioRepository repository, ITokenService tokenService)
        {
            _configuration = configuration;
            _repository = repository;
            _tokenService = tokenService;
            _converter = new UsuarioConverter();
        }

        public TokenDbo ValidarCredenciais(UsuarioDbo usuarioDbo)
        {
            var usuarioEntity = _converter.Parse(usuarioDbo);

            if (usuarioEntity == null) return null;

            var usuario = _repository.ValidacaoCredencial(usuarioEntity);

            if (usuario == null) return null;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.NomeUsuario),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.GivenName, usuario.NomeCompleto),
                new Claim(ClaimTypes.Role, usuario.Role)
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            usuario.RefreshToken = refreshToken;
            usuario.RefreshTokenTempoExpiracao = DateTime.Now.AddMinutes(_configuration.DaysToExpiry);

            _repository.AtualizarInfoUsuario(usuario);

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

            return new TokenDbo(
                accessToken,
                refreshToken,
                true,
                createDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                usuario.Role
            );
        }

        public TokenDbo ValidarCredenciais(TokenDbo token)
        {
            var accessToken = token.AccessToken;
            var refreshToken = token.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var nomeUsuario = principal.Identity.Name;

            var usuario = _repository.ValidacaoCredencial(nomeUsuario);
            if (usuario == null || 
                usuario.RefreshToken != refreshToken || 
                usuario.RefreshTokenTempoExpiracao <= DateTime.Now) return null;

            accessToken = _tokenService.GenerateAccessToken(principal.Claims);
            refreshToken = _tokenService.GenerateRefreshToken();

            usuario.RefreshToken = refreshToken;

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

            return new TokenDbo(
                accessToken, 
                refreshToken, 
                true, 
                createDate.ToString(DATE_FORMAT), 
                expirationDate.ToString(DATE_FORMAT), 
                usuario.Role
                );
        }

        public bool RevokeToken(string usuarioNome)
        {
            return _repository.RevokeToken(usuarioNome);
        }
    }
}

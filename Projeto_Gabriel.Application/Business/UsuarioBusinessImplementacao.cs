using Projeto_Gabriel.Application.BusinessInterface;
using Projeto_Gabriel.Application.Converter.Implementacao;
using Projeto_Gabriel.Application.Dto;
using Projeto_Gabriel.Domain.RepositoryInterface;

namespace Projeto_Gabriel.Application.Business
{
    public class UsuarioBusinessImplementacao : IUsuarioBusiness
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly UsuarioConverter _converter;

        public UsuarioBusinessImplementacao(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _converter = new UsuarioConverter();
        }

        public void Criar(CriarUsuarioDbo usuario)
        {
            try
            {
                if (usuario == null)
                    throw new ArgumentException("O usuário não pode ser nulo.");

                if (string.IsNullOrWhiteSpace(usuario.NomeCompleto))
                    throw new ArgumentException("O nome completo não pode ser vazio ou nulo.");

                if (string.IsNullOrWhiteSpace(usuario.NomeUsuario))
                    throw new ArgumentException("O Nome do Usuário não pode ser vazio ou nulo.");

                if (!System.Text.RegularExpressions.Regex.IsMatch(usuario.NomeCompleto, @"^[A-Za-zÀ-ÿ\s]+$"))
                    throw new ArgumentException("O nome completo deve conter apenas letras e espaços.");

                usuario.NomeCompleto = usuario.NomeCompleto.ToUpper();

                var usuarioDbo = new UsuarioDbo
                {
                    NomeCompleto = usuario.NomeCompleto,
                    NomeUsuario = usuario.NomeUsuario,
                    Senha = usuario.Senha
                };

                var usuarioEntity = _converter.Parse(usuarioDbo);

                if (usuarioEntity == null)
                    throw new ArgumentException("Erro ao converter o usuário.");

                _usuarioRepository.Criar(usuarioEntity);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Criar novo Usuário.", ex);
            }
        }
    }
}
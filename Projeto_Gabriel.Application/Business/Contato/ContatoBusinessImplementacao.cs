using Projeto_Gabriel.Application.BusinessInterface.Contato;
using Projeto_Gabriel.Application.Converter.Implementacao.Contatos;
using Projeto_Gabriel.Application.Converter.Implementacao.Contatos.ConverterRetorno;
using Projeto_Gabriel.Application.Dto.Contatos;
using Projeto_Gabriel.Application.Exceptions;
using Projeto_Gabriel.Domain.Entity;
using Projeto_Gabriel.Domain.RepositoryInterface.Contatos;
using System;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Application.Business.Contato
{
    public class ContatoBusinessImplementacao : IContatoBusiness
    {
        private readonly IContatoRepository _contatoRepository;
        private readonly ContatoConverter _contatoConverter;
        private readonly RetornoConveterContato _retornoConveterContato;

        public ContatoBusinessImplementacao(IContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;
            _contatoConverter = new ContatoConverter();
            _retornoConveterContato = new RetornoConveterContato();
        }

        public RetornoContatoDbo Criar(CriarContatoDbo criarContatoDbo, long? usuarioId)
        {
            try
            {
                if (criarContatoDbo == null)
                    throw new ArgumentNullException(nameof(criarContatoDbo), "Contato não pode ser nulo.");

                if (string.IsNullOrWhiteSpace(criarContatoDbo.Nome))
                    throw new ArgumentException("Nome é obrigatório.", nameof(criarContatoDbo.Nome));

                if (string.IsNullOrWhiteSpace(criarContatoDbo.Email))
                    throw new ArgumentException("Email é obrigatório.", nameof(criarContatoDbo.Email));

                if (string.IsNullOrWhiteSpace(criarContatoDbo.Mensagem))
                    throw new ArgumentException("Mensagem é obrigatória.", nameof(criarContatoDbo.Mensagem));

                ContatoDbo contatoDbo = new ContatoDbo
                {
                    Nome = criarContatoDbo.Nome,
                    Email = criarContatoDbo.Email,
                    Mensagem = criarContatoDbo.Mensagem,
                    DataContato = DateTime.Now,
                    UsuarioId = usuarioId
                };

                var contato = _contatoConverter.Parse(contatoDbo);
                var contatoSalvo = _contatoRepository.Criar(contato);
                var contatoSalvoDbo = _contatoConverter.Parse(contatoSalvo);

                return _retornoConveterContato.Parse(contatoSalvoDbo);

            }
            catch (ValidationException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public RetornoContatoDbo ObterPorId(long id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID inválido");

                var contato = _contatoRepository.ObterPorId(id);

                if (contato == null)
                    throw new NotFoundException("Contato não encontrado");

                var contatoDbo = _contatoConverter.Parse(contato);

                return _retornoConveterContato.Parse(contatoDbo);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar cartão por ID.", ex);
            }
        }

        public List<RetornoContatoDbo> ObterTodos()
        {
            try
            {
                var contatos = _contatoRepository.ObterTodos();

                if (contatos == null || contatos.Count == 0)
                    throw new NotFoundException("Nenhum contato encontrado.");

                var parsedContatos = _contatoConverter.ParseList(contatos);

                if (parsedContatos == null || parsedContatos.Count == 0)
                    throw new NotFoundException("Nenhum contato encontrado.");

                return _retornoConveterContato.ParseList(parsedContatos);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar todos os cartões.", ex);
            }
        }
    }
}
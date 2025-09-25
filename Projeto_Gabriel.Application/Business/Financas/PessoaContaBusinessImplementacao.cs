using Projeto_Gabriel.Application.BusinessInterface.Financas;
using Projeto_Gabriel.Application.Converter.Implementacao.Financas;
using Projeto_Gabriel.Application.Converter.Implementacao.Financas.ConverterRetornoFinancas;
using Projeto_Gabriel.Application.Dto.Financas.PessoaContaDbo;
using Projeto_Gabriel.Application.Exceptions;
using Projeto_Gabriel.Domain.RepositoryInterface.Financas;

namespace Projeto_Gabriel.Application.Business.Financas
{
    public class PessoaContaBusinessImplementacao : IPessoaContaBusiness
    {
        private readonly IPessoaContaRepository _pessoaContaRepository;
        private readonly PessoaContaConverter _converter;
        private readonly PessoaContaConverterRetorno _converterRetorno;

        public PessoaContaBusinessImplementacao(IPessoaContaRepository pessoaContaRepository)
        {
            _pessoaContaRepository = pessoaContaRepository;
            _converter = new PessoaContaConverter();
            _converterRetorno = new PessoaContaConverterRetorno();
        }

        public RetornoPessoaContaDbo ObterPorId(long id, long usuarioId)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID inválido");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var pessoaConta = _pessoaContaRepository.ObterPorIdSeguro(id, usuarioId);

                if (pessoaConta == null)
                    throw new NotFoundException("PessoaConta não encontrada");

                var pessoaContaDbo = _converter.Parse(pessoaConta);

                return _converterRetorno.Parse(pessoaContaDbo);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar PessoaConta por ID.", ex);
            }
        }

        public List<RetornoPessoaContaDbo> ObterTodos(long usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var pessoas = _pessoaContaRepository.ObterTodosSeguro(usuarioId);

                if (pessoas == null || pessoas.Count == 0)
                    throw new NotFoundException("Nenhuma PessoaConta encontrada.");

                var parsedPessoasContas = _converter.ParseList(pessoas);

                if (parsedPessoasContas == null || parsedPessoasContas.Count == 0)
                    throw new NotFoundException("Nenhuma PessoaConta encontrada.");

                return _converterRetorno.ParseList(parsedPessoasContas);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar todas as PessoaConta.", ex);
            }
        }

        public RetornoPessoaContaDbo Criar(CriarPessoaContaDbo pessoaConta, long usuarioId)
        {
            try
            {
                PessoaContaDbo pessoaContaDbo = new PessoaContaDbo
                {
                    UsuarioId = usuarioId,
                    NomePessoa = pessoaConta.NomePessoa
                };

                var pessoaContaEntity = _converter.Parse(pessoaContaDbo);

                if (pessoaContaEntity == null)
                    throw new ArgumentException("Erro ao converter PessoaConta.");

                var criado = _pessoaContaRepository.Criar(pessoaContaEntity);

                var pessoaContaDboCriado = _converter.Parse(criado);

                return _converterRetorno.Parse(pessoaContaDboCriado);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar PessoaConta.", ex);
            }
        }

        public List<RetornoPessoaContaDbo> ObterPorNome(string nomePessoa, long usuarioId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomePessoa))
                    throw new ArgumentException("O nome da pessoa não pode ser vazio.");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var pessoas = _pessoaContaRepository.ObterPorNome(nomePessoa, usuarioId);

                if (pessoas == null || pessoas.Count == 0)
                    throw new NotFoundException("Nenhuma PessoaConta encontrada com o nome especificado.");

                var parsedPessoasConta =  _converter.ParseList(pessoas);

                return _converterRetorno.ParseList(parsedPessoasConta);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar PessoaConta por nome.", ex);
            }
        }

        public RetornoPessoaContaDbo AtualizarPessoaConta(AtualizarPessoaContaDbo pessoaConta, long usuarioId)
        {
            try
            {
                if (pessoaConta.Id <= 0)
                    throw new ArgumentException("ID inválido");

                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var pessoaContaExistente = _pessoaContaRepository.ObterPorIdSeguro(pessoaConta.Id, usuarioId);

                if (pessoaContaExistente == null)
                    throw new NotFoundException("Pessoa Conta não encontrada");

                pessoaContaExistente.NomePessoa = pessoaConta.NomePessoa;

                var pessoaContaAtualizado = _pessoaContaRepository.Atualizar(pessoaContaExistente);

                var pessoaContaDboAtualizado = _converter.Parse(pessoaContaAtualizado);

                return _converterRetorno.Parse(pessoaContaDboAtualizado);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar a Pessoa Conta.", ex);
            }
        }

        public void DeletarPessoaConta(long id, long usuarioId)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID inválido");
                if (usuarioId <= 0)
                    throw new ArgumentException("Usuário inválido");

                var pessoaContaExistente = _pessoaContaRepository.ObterPorIdSeguro(id, usuarioId);

                if (pessoaContaExistente == null)
                    throw new NotFoundException("Pessoa Conta não encontrado");

                _pessoaContaRepository.Deletar(pessoaContaExistente.Id);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar o Pessoa Conta.", ex);
            }
        }
    }
}
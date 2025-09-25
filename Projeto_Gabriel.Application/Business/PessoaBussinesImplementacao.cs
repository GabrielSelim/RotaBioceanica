using Projeto_Gabriel.Application.Converter.Implementacao;
using Projeto_Gabriel.Application.Dto;
using Projeto_Gabriel.Application.Exceptions;
using Projeto_Gabriel.Application.Hypermedia.Utils;
using Projeto_Gabriel.Domain.RepositoryInterface;

namespace Projeto_Gabriel.Bussines.Implementacoes
{
    public class PessoaBussinesImplementacao : IPessoaBussines
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly PessoaConverter _converter;

        public PessoaBussinesImplementacao(IPessoaRepository pessoaRepository)
        {
            _pessoaRepository = pessoaRepository;
            _converter = new PessoaConverter();
        }

        public PessoaDbo ObterPorId(long id)
        {
            var pessoa = _pessoaRepository.ObterPorId(id);

            if (pessoa == null)
                throw new Exception("Pessoa não encontrada");

            if (pessoa.Ativo == false)
                throw new Exception("Pessoa desativada");

            return _converter.Parse(pessoa);
        }

        public List<PessoaDbo> ObterTodos()
        {
            var pessoas = _pessoaRepository.ObterTodos();

            if (pessoas == null || pessoas.Count == 0)
                throw new Exception("Nenhuma pessoa encontrada");

            return _converter.ParseList(pessoas);
        }

        public PagedSearchDbo<PessoaDbo> ObterComPaginacao( string direcaoOrdenacao, int tamanhoPagina, int paginaAtual)
        {
            if (tamanhoPagina < 1)
                throw new Exception("Tamanho da página deve ser maior que zero");
            if (paginaAtual < 0)
                throw new Exception("Página atual deve ser maior ou igual a zero");
            if (string.IsNullOrEmpty(direcaoOrdenacao))
                direcaoOrdenacao = "asc";
            if (direcaoOrdenacao != "asc" && direcaoOrdenacao != "desc")
                throw new Exception("Direção de ordenação deve ser 'asc' ou 'desc'");

            var pessoas = _pessoaRepository.ObterComPaginacao(paginaAtual, tamanhoPagina, direcaoOrdenacao);
            var totalResults = _pessoaRepository.GetCount();

            return new PagedSearchDbo<PessoaDbo>
            {
                CurrentPage = paginaAtual,
                PageSize = tamanhoPagina,
                TotalResults = totalResults,
                SortDirections = direcaoOrdenacao,
                List = _converter.ParseList(pessoas)
            };
        }


        public PessoaDbo Criar(PessoaDbo pessoa)
        {
            try
            {
                var pessoaEntity = _converter.Parse(pessoa);
                pessoaEntity = _pessoaRepository.Criar(pessoaEntity);

                if (pessoaEntity == null)
                    throw new ArgumentException("Erro ao criar pessoa");

                return _converter.Parse(pessoaEntity);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: + {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar pessoa", ex);
            }
        }

        public PessoaDbo Atualizar(PessoaDbo pessoa)
        {
            try
            {
                if (pessoa == null || pessoa.Id <= 0)
                    throw new ArgumentException("ID inválido");

                var pessoaEntity = _converter.Parse(pessoa);

                if (pessoaEntity == null)
                    throw new KeyNotFoundException("Pessoa não encontrada");

                pessoaEntity = _pessoaRepository.Atualizar(pessoaEntity);

                if (pessoaEntity == null)
                    throw new ArgumentException("Erro ao atualizar pessoa");

                return _converter.Parse(pessoaEntity);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Erro de argumento ao atualizar pessoa: " + ex.Message, ex);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException("Erro ao atualizar pessoa: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao atualizar pessoa: " + ex.Message, ex);
            }
        }

        public PessoaDbo Desativar(long id)
        {
            try
            {
                var pessoaEntity = _pessoaRepository.Desativar(id);

                if (pessoaEntity == null)
                    throw new KeyNotFoundException("Pessoa não encontrada");

                return _converter.Parse(pessoaEntity);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Erro de argumento ao desativar pessoa: " + ex.Message, ex);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException("Erro ao desativar pessoa: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao desativar pessoa: " + ex.Message, ex);
            }
        }

        public PessoaDbo Ativar(long id)
        {
            try
            {
                var pessoaEntity = _pessoaRepository.Ativar(id);

                if (pessoaEntity == null)
                    throw new KeyNotFoundException("Pessoa não encontrada");

                return _converter.Parse(pessoaEntity);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Erro de argumento ao ativar pessoa: " + ex.Message, ex);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException("Erro ao ativar pessoa: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao ativar pessoa: " + ex.Message, ex);
            }
        }

        public List<PessoaDbo> ObterPessoasPorNome(string nome)
        {
            var pessoasDbo = _pessoaRepository.ObterPessoasPorNome(nome);

            if (pessoasDbo == null || pessoasDbo.Count == 0)
                throw new Exception("Nenhuma pessoa encontrada com esse nome");

            return _converter.ParseList(pessoasDbo);
        }

        public List<PessoaDbo> ObterPessoasPorEndereco(string endereco)
        {
            var pessoasDbo = _pessoaRepository.ObterPessoasPorEndereco(endereco);

            if (pessoasDbo == null || pessoasDbo.Count == 0)
                throw new Exception("Nenhuma pessoa encontrada com esse endereço");

            return _converter.ParseList(pessoasDbo);
        }

        public void Deletar(long id)
        {
            try
            {
                var pessoa = _pessoaRepository.ObterPorId(id);

                if (pessoa == null)
                    throw new ArgumentException("Pessoa não encontrada");

                _pessoaRepository.Deletar(id);
            }
            catch (KeyNotFoundException ex)
            {
                throw new NotFoundException($"Erro ao deletar pessoa: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Erro ao deletar pessoa: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao deletar pessoa: " + ex.Message, ex);
            }
        }
    }
}

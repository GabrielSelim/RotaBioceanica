using Projeto_Gabriel.Application.Dto;
using Projeto_Gabriel.Application.Hypermedia.Utils;

namespace Projeto_Gabriel.Bussines
{
    public interface IPessoaBussines
    {
        PessoaDbo Criar(PessoaDbo pessoa);

        PessoaDbo Atualizar(PessoaDbo pessoa);

        List<PessoaDbo> ObterTodos();

        PagedSearchDbo<PessoaDbo> ObterComPaginacao(string direcaoOrdenacao, int tamanhoPagina, int paginaAtual);

        PessoaDbo ObterPorId(long id);

        PessoaDbo Desativar(long id);

        PessoaDbo Ativar(long id);

        List<PessoaDbo> ObterPessoasPorNome(string nome);

        List<PessoaDbo> ObterPessoasPorEndereco(string endereco);

        void Deletar(long id);
    }
}
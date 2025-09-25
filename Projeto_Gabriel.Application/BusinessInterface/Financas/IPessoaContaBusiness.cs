using Projeto_Gabriel.Application.Dto.Financas.PessoaContaDbo;

namespace Projeto_Gabriel.Application.BusinessInterface.Financas
{
    public interface IPessoaContaBusiness
    {
        RetornoPessoaContaDbo ObterPorId(long id, long usuarioId);

        List<RetornoPessoaContaDbo> ObterTodos(long usuarioId);

        RetornoPessoaContaDbo Criar(CriarPessoaContaDbo pessoaConta, long usuarioId);

        List<RetornoPessoaContaDbo> ObterPorNome(string nomePessoa, long usuarioId);

        RetornoPessoaContaDbo AtualizarPessoaConta(AtualizarPessoaContaDbo pessoaConta, long usuarioId);

        void DeletarPessoaConta(long id, long usuarioId);
    }
}
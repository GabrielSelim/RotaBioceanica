using Projeto_Gabriel.Application.Dto.Contatos;

namespace Projeto_Gabriel.Application.BusinessInterface.Contato
{
    public interface IContatoBusiness
    {
        RetornoContatoDbo Criar(CriarContatoDbo contatoDbo, long? usuarioId);

        RetornoContatoDbo ObterPorId(long id);

        List<RetornoContatoDbo> ObterTodos();
    }
}
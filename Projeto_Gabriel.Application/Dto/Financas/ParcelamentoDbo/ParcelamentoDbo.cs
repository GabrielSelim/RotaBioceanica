using Projeto_Gabriel.Application.Hypermedia.Abstract;
using Projeto_Gabriel.Application.Hypermedia;

namespace Projeto_Gabriel.Application.Dto.Financas.ParcelamentoDbo
{
    public class ParcelamentoDbo
    {
        public long Id { get; set; }
        public long UsuarioId { get; set; }
        public string Descricao { get; set; }
        public decimal ValorTotal { get; set; }
        public int NumeroParcelas { get; set; }
        public DateTime DataPrimeiraParcela { get; set; }
        public int IntervaloParcelas { get; set; }
        public long? CartaoId { get; set; }
        public long? PessoaContaId { get; set; }
        public string Situacao { get; set; }
    }
}
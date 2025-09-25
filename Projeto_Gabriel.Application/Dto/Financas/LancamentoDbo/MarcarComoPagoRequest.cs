namespace Projeto_Gabriel.Application.Dto.Financas.LancamentoDbo
{
    public class MarcarComoPagoRequest
    {
        public long Id { get; set; }
        public decimal ValorPago { get; set; }
        public DateTime DataPagamento { get; set; }
    }
}
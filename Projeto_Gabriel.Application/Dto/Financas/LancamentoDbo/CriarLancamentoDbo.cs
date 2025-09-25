namespace Projeto_Gabriel.Application.Dto.Financas.LancamentoDbo
{
    public class CriarLancamentoDbo
    {
        public DateTime DataLancamento { get; set; }

        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public string TipoLancamento { get; set; }

        public long CategoriaId { get; set; }

        public string Situacao { get; set; }
    }
}
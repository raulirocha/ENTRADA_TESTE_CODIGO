using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    /// <summary>
    /// Representa um perfil de operação fiscal, que agrupa um conjunto de regras.
    /// Define o contexto da transação, como "Venda para Consumidor Final" ou "Devolução de Compra".
    /// Mapeia a tabela 'fiscal_perfil_fiscal'.
    /// </summary>
    [Table("fiscal_perfil")]
    public class PerfilFiscal
    {
        /// <summary>
        /// A chave primária do perfil fiscal.
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// O nome descritivo do perfil (ex: "Venda Dentro do Estado").
        /// </summary>
        [Column("nome")]
        public string? Nome { get; set; }

        /// <summary>
        /// tipo de operação se é uma saída ou uma entrada 
        /// </summary>
        [Column("tipo_operacao")]
        public string? TipoOperacao { get; set; }

        /// <summary>
        /// observação padrão daquele perfil fiscal . pode ser usado para deixar fixo sempre
        /// que fixar uma nota sair tal observação . 
        /// </summary>
        [Column("observacao")]
        public string? Observacao { get; set; }

        /// <summary>
        /// natureza da operação ser é nf / venda / orçamento
        /// </summary>
        [Column("natureza_operacao")]
        public string? NaturezaOperacao { get; set; }

        [Column("natop")]
        public string? NatOp { get; set; }

        [Column("finalidade_nfe")]
        public short FinalidadeNfe { get; set; }


        /// <summary>
        /// Propriedade de navegação para as regras fiscais associadas a este perfil.
        /// Representa a relação "um-para-muitos".
        /// </summary>
        public virtual ICollection<RegraFiscal> Regras { get; set; } = new List<RegraFiscal>();

        /// <summary>
        /// Propriedade de navegação para a tabela de junção que liga PerfilFiscal a SistemaAcao.
        /// Representa a parte "muitos" da relação muitos-para-muitos.
        /// </summary>
        public virtual ICollection<FiscalPerfilRegraAcao> AcoesLink { get; set; } = new List<FiscalPerfilRegraAcao>();




    }
}
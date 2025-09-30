// Em Vendas.Core/FormaPagamento.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    /// <summary>
    /// Representa uma forma de pagamento cadastrada no sistema.
    /// Contém as regras de negócio associadas, como a permissão de parcelamento.
    /// </summary>
    [Table("conf_forma_pagamento")] // Diz ao EF Core o nome exato da tabela
    public class ForPagamento
    {
        [Key] // Diz que 'Id' é a chave primária
        [Column("id")] // Mapeia para a coluna 'id'
        public int Id { get; set; }

        [Column("descricao")] // Mapeia para a coluna 'descricao'
        public string Descricao { get; set; } = string.Empty;

        [Column("aceita_parcelamento")] // Mapeia para a coluna 'aceita_parcelamento'
        public bool AceitaParcelamento { get; set; }

        // Adicionamos esta propriedade que faltava, usada no filtro 'WHERE ativo = true'
        [Column("ativo")]
        public bool Ativo { get; set; }

        [Column("tipo_nf")]
        public string CodigoFiscal { get; set; }
    }
}
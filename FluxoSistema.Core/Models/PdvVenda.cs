using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("pdv_venda")]
    public class PdvVenda
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("dataabertura")]
        public DateTime DataAbertura { get; set; }

        [Column("statusvenda")]
        public string StatusVenda { get; set; }

        [Column("cusuario")]
        public int UsuarioId { get; set; }

        [Column("nusuario")]
        public string NomeUsuario { get; set; }

        [Column("caixaid")]
        public int CaixaId { get; set; }

        [Column("valortotal")]
        public decimal ValorTotal { get; set; }

        [Column("subtotal")]
        public decimal Subtotal { get; set; }

        [Column("descontocabecalho")]
        public decimal DescontoCabecalho { get; set; }

        [Column("datafechamento")]
        public DateTime? DataFechamento { get; set; } // Usamos '?' porque pode ser nula


        [Column("empresaid")]
        public int EmpresaId { get; set; }

        [Column("operacao")]
        public int OperacaoId { get; set; }

        [Column("clienteid")]
        public int ClienteId { get; set; }
    }
}
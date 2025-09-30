using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("venda")]
    public class Venda
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("numerodocumento")]
        public int NumeroDocumento { get; set; }

        [Column("datavenda")]
        public DateTime DataVenda { get; set; }

        [Column("nomecliente")]
        public string? NomeCliente { get; set; }

        [Column("cpf_cnpj")]
        public string? CpfCnpj { get; set; }

        [Column("totalvenda")]
        public decimal TotalVenda { get; set; }

        [Column("desconto")]
        public decimal Desconto { get; set; }

        [Column("valortroco")]
        public decimal ValorTroco { get; set; }

        [Column("statusvenda")]
        public string StatusVenda { get; set; }

        [Column("idcliente")]
        public int IdCliente { get; set; }

        [Column("filial")]
        public int Filial { get; set; }

        [Column("pdvdoc")]
        public int PdvDoc { get; set; }

        [Column("codusuario")]
        public int CodUsuario { get; set; }

        [Column("nusuario")]
        public string? NUsuario { get; set; }

        [Column("subtotal")]
        public decimal Subtotal { get; set; }

        [Column("natureza_operacao")]
        public string NaturezaOperacao { get; set; }

        [Column("status_documento")]
        public string StatusDocumento { get; set; }
    }
}
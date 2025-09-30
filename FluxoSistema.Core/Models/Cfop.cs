// Local: FluxoSistema.Core/Models/Cfop.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("fiscal_cfop")]
    public class Cfop
    {
        [Key]
        [Column("id")] // <-- ADICIONE ESTA LINHA PARA CORRIGIR O ERRO
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        [Column("codigo")] // É uma boa prática mapear todas as colunas
        public string Codigo { get; set; }

        [Required]
        [StringLength(800)]
        [Column("descricao")] // É uma boa prática mapear todas as colunas
        public string Descricao { get; set; }
    }
}
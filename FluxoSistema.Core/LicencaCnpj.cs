using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("licenca_cnpjs")]
    public class LicencaCnpj
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(18)]
        public string Cnpj { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } // Ex: "Ativo", "Cancelado"

        public string Modulos { get; set; } // Pode ser um JSON: "['vendas', 'financeiro']"

        // Chave estrangeira para o cliente da licença
        public Guid LicencaClienteId { get; set; }

        [ForeignKey("LicencaClienteId")]
        public virtual LicencaCliente LicencaCliente { get; set; }
    }
}
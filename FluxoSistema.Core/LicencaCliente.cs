using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("licenca_clientes")]
    public class LicencaCliente
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(200)]
        public string NomeCliente { get; set; }

        [MaxLength(18)]
        public string CpfCnpjCliente { get; set; }

        [Required]
        public DateTime DataExpiracao { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } // Ex: "Ativa", "Cancelada", "Pendente"

        [MaxLength(200)]
        public string? HardwareId { get; set; }

        [MaxLength(50)]
        public string? ChaveAtivacao { get; set; }

        /// <summary>
        /// A assinatura criptográfica completa, em Base64. Esta é a chave para a validação.
        /// </summary>
        public string? AssinaturaCompleta { get; set; }

        /// <summary>
        /// Os dados exatos que foram usados para gerar a assinatura (CNPJ;HardwareID;ExpiraEm).
        /// </summary>
        public string? DadosLicenca { get; set; }

        // Relação: Um cliente de licença pode ter vários CNPJs licenciados
        public virtual ICollection<LicencaCnpj> CnpjsLicenciados { get; set; } = new List<LicencaCnpj>();
    }
}
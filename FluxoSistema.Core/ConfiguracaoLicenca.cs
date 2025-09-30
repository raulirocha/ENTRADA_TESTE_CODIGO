// Em: FluxoSistema.Core/Models/ConfiguracaoLicenca.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core
{
    [Table("configuracao_licenca")]
    public class ConfiguracaoLicenca
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ChavePrivadaXml { get; set; }
    }
}
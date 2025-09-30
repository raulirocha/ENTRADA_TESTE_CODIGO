using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("fiscal_perfil_regras_acao")]
    public class FiscalPerfilRegraAcao
    {


        [Column("id_perfil_fiscal")]
        public int PerfilFiscalId { get; set; }

        [Column("id_acao")]
        public int AcaoId { get; set; }

        // Propriedade de navegação para o EF Core fazer o JOIN
        public virtual SistemaAcao Acao { get; set; }
    }
}
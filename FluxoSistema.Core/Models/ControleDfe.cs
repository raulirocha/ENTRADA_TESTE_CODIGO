// Salve como: FluxoSistema.Core/Models/ControleDfe.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("controledfe")]
    public class ControleDfe
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("cnpjdestinatario")]
        public string? CnpjDestinatario { get; set; }

        [Column("ultimonsu")]
        public string? UltimoNsu { get; set; }

        [Column("proximonsu")]
        public string? ProximoNsu { get; set; }

        [Column("ultimaconsulta")]
        public DateTime? UltimaConsulta { get; set; }

        [Column("statussincronizacao")]
        public string? StatusSincronizacao { get; set; }
    }
}
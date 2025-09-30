using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("cliente")]
    public class Cliente
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        public string? Nome { get; set; }

        [Column("nomefantasia")]
        public string? NomeFantasia { get; set; }

        [Column("cpf_cnpj")]
        public string? CpfCnpj { get; set; }

        [Column("rg_ie")]
        public string? RgIe { get; set; }

        [Column("indicador_consumidor_final")]
        public short? IndicadorConsumidorFinal { get; set; }

        [Column("indicador_ie")]
        public short? IndicadorIe { get; set; }

        [Column("inscmunicipal")]
        public string? InscricaoMunicipal { get; set; }

        [Column("telefone1")]
        public string? Fone1Cliente { get; set; }

        [Column("tipo")]
        public string? Tipo { get; set; }

        [Column("tipopessoa")]
        public string? TipoPessoa { get; set; }

        [Column("datacadastro")]
        public DateTime? DataCadastro { get; set; }



        // --- INÍCIO DA ALTERAÇÃO ---

        // REMOVEMOS os campos de endereço (Logradouro, Numero, etc.)

        // ADICIONAMOS uma "propriedade de navegação".
        // Isto diz ao EF Core que um Cliente pode ter uma lista de Enderecos.
        public virtual ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();

        [NotMapped] public string? Logradouro { get; set; }
        [NotMapped] public string? Numero { get; set; }
        [NotMapped] public string? Bairro { get; set; }
        [NotMapped] public string? Cidade { get; set; }
        [NotMapped] public string? Uf { get; set; }
        [NotMapped] public string? Cep { get; set; }

        [NotMapped] public long CodigoMunicipio { get; set; }

        // --- FIM DA ALTERAÇÃO ---
    }
}
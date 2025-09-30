using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    // Diz ao EF Core que esta classe corresponde à tabela "empresa"
    [Table("empresa")]
    public class Empresa
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("razaosocial")]
        public string? RazaoSocial { get; set; }

        [Column("nomefantasia")]
        public string? NomeFantasia { get; set; }

        [Column("cnpj")]
        public string? Cnpj { get; set; }

        [Column("inscricaoestadual")]
        public string? InscricaoEstadual { get; set; }

        [Column("estado")]
        public string? Uf { get; set; }


        [Column("endereco")]
        public string? Logradouro { get; set; }

        [Column("numero")]
        public string? Numero { get; set; }

        [Column("bairro")]
        public string? Bairro { get; set; }

        [Column("cidade")]
        public string? Cidade { get; set; }

        [Column("cep")]
        public string? Cep { get; set; }

        [Column("telefone")]
        public string? Telefone { get; set; }

        [Column("serienfe")]
        public int? SerieNfe { get; set; }

        [Column("uf_codigo")]
        public int? CodigoUf { get; set; }

        /// <summary>
        /// Define o regime tributário da empresa (ex: 1=Simples Nacional, 2=Lucro Presumido).
        /// </summary>
        [Column("regimetributario")]
        public int IdRegimeTributario { get; set; }

        [Column("certificadoa1path")]
        public string CertificadoA1Path { get; set; }

        [Column("certificadoa1senha")]
        public string CertificadoA1Senha { get; set; }

        [Column("idtoken")]
        public string IdCsc { get; set; }

        [Column("csc")]
        public string Csc { get; set; }

        [Column("codigomunicipio")]
        public int CodigoMunicipio { get; set; }




    }
}
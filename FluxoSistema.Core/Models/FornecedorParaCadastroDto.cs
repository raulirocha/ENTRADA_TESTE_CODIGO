// Em: FluxoSistema.Core/Models/FornecedorParaCadastroDto.cs

namespace FluxoSistema.Core.Models
{
    /// <summary>
    /// Objeto simples (DTO) para transportar os dados de um fornecedor extraídos de um XML
    /// para o formulário de cadastro, sem acoplar a lógica de XML.
    /// </summary>
    public class FornecedorParaCadastroDto
    {
        public string Cnpj { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string InscricaoEstadual { get; set; }
        public string Telefone { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public long CodigoMunicipio { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models

{
    [Table("nota_fiscal")]
    public class NfeHeader
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        // === Grupo B: Identificação da Nota Fiscal eletrônica ===
        [Column("chave_acesso")]
        public string? ChaveAcesso { get; set; }

        [Column("numero_nf")]
        public string? NumeroNf { get; set; }

        [Column("serie")]
        public string? Serie { get; set; }

        [Column("modelo")]
        public string? Modelo { get; set; }

        [Column("cuf")]
        public int? CodigoUf { get; set; }

        [Column("nat_op")]
        public string? NaturezaOperacao { get; set; }

        [Column("dh_emi")]
        public DateTimeOffset? DataEmissao { get; set; }

        [Column("dh_sai_ent")]
        public DateTimeOffset? DataSaidaEntrada { get; set; }

        [Column("tp_nf")]
        public short? TipoOperacao { get; set; } // 0=Entrada; 1=Saída

        [Column("id_dest")]
        public short? IdentificadorDestino { get; set; } // 1=Interna; 2=Interestadual; 3=Exterior

        [Column("cmun_fg")]
        public int? CodigoMunicipioFatoGerador { get; set; }

        [Column("tp_imp")]
        public short? TipoImpressaoDanfe { get; set; }

        [Column("tp_emis")]
        public short? TipoEmissao { get; set; } // normal ou contigencia

        [Column("c_dv")]
        public short? DigitoVerificadorChave { get; set; } // digito verificador gerado automático

        [Column("tp_amb")]
        public short? TipoAmbiente { get; set; } // 1=Produção; 2=Homologação

        [Column("fin_nfe")]
        public short? FinalidadeNfe { get; set; } //normal / devolução / ajuste / complemento

        [Column("ind_final")]
        public short? IndicadorConsumidorFinal { get; set; }  // se o cliente é consumidor final ou comprando para revender

        [Column("ind_pres")]
        public short? IndicadorPresencaComprador { get; set; } // se é presencial / entraga / online / deliver 

        [Column("indicador_ie")]
        public short? IndicadorIE { get; set; } // Emissão de NF-e com aplicativo do contribuinte

        [Column("ver_proc")]
        public string? VersaoProcesso { get; set; }

        // === Grupo E: Identificação do Destinatário da Nota Fiscal eletrônica ===
        [Column("id_cliente")]
        public int? IdCliente { get; set; }

        [Column("dest_nome")]
        public string? DestinatarioNome { get; set; }

        [Column("dest_cnpj_cpf")]
        public string? DestinatarioCpfCnpj { get; set; }

        [Column("dest_ie")]
        public string? DestinatarioIe { get; set; }

        [Column("dest_logradouro")]
        public string? DestinatarioLogradouro { get; set; }

        [Column("dest_numero")]
        public string? DestinatarioNumero { get; set; }

        [Column("dest_bairro")]
        public string? DestinatarioBairro { get; set; }

        [Column("dest_municipio")]
        public string? DestinatarioMunicipio { get; set; }

        [Column("dest_cmun")]
        public int? DestinatarioCodigoMunicipio { get; set; }

        [Column("dest_uf")]
        public string? DestinatarioUf { get; set; }

        [Column("dest_cep")]
        public string? DestinatarioCep { get; set; }

        [Column("dest_pais")]
        public string? DestinatarioPais { get; set; }

        [Column("dest_cpais")]
        public int? DestinatarioCodigoPais { get; set; }

        [Column("dest_fone")]
        public string? DestinatarioFone { get; set; }

        // === Grupo W: Totais da NF-e ===
        [Column("tot_vbc")]
        public decimal? TotalBaseCalculoIcms { get; set; }

        [Column("tot_vicms")]
        public decimal? TotalIcms { get; set; }

        [Column("tot_vicms_deson")]
        public decimal? TotalIcmsDesonerado { get; set; }

        [Column("tot_vfcp")]
        public decimal? TotalFcp { get; set; }

        [Column("tot_vbcst")]
        public decimal? TotalBaseCalculoIcmsSt { get; set; }

        [Column("tot_vst")]
        public decimal? TotalIcmsSt { get; set; }

        [Column("tot_vfcpst")]
        public decimal? TotalFcpSt { get; set; }

        [Column("tot_vfcpst_ret")]
        public decimal? TotalFcpStRetido { get; set; }

        [Column("tot_vprod")]
        public decimal? TotalProdutos { get; set; }

        [Column("tot_vfrete")]
        public decimal? TotalFrete { get; set; }

        [Column("tot_vseg")]
        public decimal? TotalSeguro { get; set; }

        [Column("tot_vdesc")]
        public decimal? TotalDesconto { get; set; }

        [Column("tot_vii")]
        public decimal? TotalImpostoImportacao { get; set; }

        [Column("tot_vipi")]
        public decimal? TotalIpi { get; set; }

        [Column("tot_vipidevol")]
        public decimal? TotalIpiDevolvido { get; set; }

        [Column("tot_vpis")]
        public decimal? TotalPis { get; set; }

        [Column("tot_vcofins")]
        public decimal? TotalCofins { get; set; }

        [Column("tot_voutro")]
        public decimal? TotalOutrasDespesas { get; set; }

        [Column("tot_vnf")]
        public decimal? ValorTotalNota { get; set; }

        [Column("tot_vtottrib")]
        public decimal? TotalTributos { get; set; }

        // === Grupo X: Informações do Transporte da NF-e ===
        [Column("transp_modfrete")]
        public short? ModalidadeFrete { get; set; }

        [Column("transp_transporta_cnpj")]
        public string? TransportadoraCnpj { get; set; }

        // === Grupo Y: Dados da Cobrança ===
        [Column("cobr_nfat")]
        public string? CobrancaNumeroFatura { get; set; }

        [Column("cobr_vorig")]
        public decimal? CobrancaValorOriginal { get; set; }

        [Column("cobr_vdesc")]
        public decimal? CobrancaValorDesconto { get; set; }

        [Column("cobr_vliq")]
        public decimal? CobrancaValorLiquido { get; set; }

        [Column("dup_ndup")]
        public string? DuplicataNumero { get; set; }

        [Column("dup_dvenc")]
        public DateTime? DuplicataDataVencimento { get; set; }

        [Column("dup_vdup")]
        public decimal? DuplicataValor { get; set; }

        // === Grupo YA: Informações de Pagamento ===
        [Column("pag_tpag")]
        public string? PagamentoTipo { get; set; }

        [Column("pag_vpag")]
        public decimal? PagamentoValor { get; set; }

        // === Grupo Z: Informações Adicionais da NF-e ===
        [Column("inf_cpl")]
        public string? InformacoesComplementares { get; set; }

        // === Protocolo de Autorização de Uso ===
        [Column("prot_tpamb")]
        public short? ProtocoloTipoAmbiente { get; set; }

        [Column("prot_veraplic")]
        public string? ProtocoloVersaoAplicativo { get; set; }

        [Column("prot_chnfe")]
        public string? ProtocoloChaveNfe { get; set; }

        [Column("prot_dhrecbto")]
        public DateTime? ProtocoloDataRecebimento { get; set; }

        [Column("prot_nprot")]
        public string? ProtocoloNumero { get; set; }

        [Column("prot_digval")]
        public string? ProtocoloDigestValue { get; set; }

        [Column("prot_cstat")]
        public string? ProtocoloCodigoStatus { get; set; }

        [Column("prot_xmotivo")]
        public string? ProtocoloMotivo { get; set; }

        // === Campos de Controle Interno ===
        [Column("filial")]
        public int? Filial { get; set; }

        [Column("just_cancelamento")]
        public string? JustificativaCancelamento { get; set; }

        [Column("xml_retorno")]
        public string? XmlRetorno { get; set; }

        [Column("qrcode_url")]
        public string? QrCodeUrl { get; set; }



        [Column("dest_im")]

        public string? DestinatarioInscricaoMunicipal { get; set; }

        public virtual ICollection<NfeEvento> Eventos { get; set; } = new List<NfeEvento>();
    }
}
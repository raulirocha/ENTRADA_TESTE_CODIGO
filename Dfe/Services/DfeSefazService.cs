// Salve em: FluxoSistema.Infrastructure/Dfe/Services/DfeSefazService.cs

using System;
using System.IO; // <-- ADICIONADO
using System.IO.Compression; // <-- ADICIONADO
using System.Text; // <-- ADICIONADO
using System.Threading.Tasks;
using System.Xml.Serialization;
using DFe.Classes.Flags;
using DFe.Utils;
using FluxoSistema.Application.Dfe.Repositories;
using FluxoSistema.Application.Dfe.Services;
using FluxoSistema.Application.Fiscal.NFe.Repositories;
using FluxoSistema.Core.Models;
using FluxoSistema.Infrastructure.Fiscal.NFe.Helpers;
using NFe.Classes.Informacoes.Detalhe.Tributacao;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Servicos.DistribuicaoDFe.Schemas;
using NFe.Classes.Servicos.Evento;
using NFe.Classes.Servicos.Tipos;
using NFe.Servicos;



namespace FluxoSistema.Infrastructure.Dfe.Services
{
    public class DfeSefazService : IDfeSefazService
    {
        private readonly IControleDfeRepository _controleDfeRepository;
        private readonly IDfeRepository _dfeRepository;
        private readonly IEmpresaRepository _empresaRepository;

        public DfeSefazService(
            IControleDfeRepository controleDfeRepository,
            IDfeRepository dfeRepository,
            IEmpresaRepository empresaRepository)
        {
            _controleDfeRepository = controleDfeRepository;
            _dfeRepository = dfeRepository;
            _empresaRepository = empresaRepository;
        }



        // No arquivo: FluxoSistema.Infrastructure/Dfe/Services/DfeSefazService.cs

        public async Task<int> ConsultarDocumentosDestinadosAsync(int idEmpresa, string cnpj)
        {
            var empresa = await _empresaRepository.GetByIdAsync(idEmpresa);
            if (empresa == null)
                throw new Exception($"Empresa com ID {idEmpresa} não encontrada.");

            var cfg = ZeusConfigHelper.CriarConfiguracao(empresa, 1, ModeloDocumento.NFe);
            var servico = new ServicosNFe(cfg);

            var controle = await _controleDfeRepository.GetControlePorCnpjAsync(cnpj)
                           ?? new ControleDfe { CnpjDestinatario = cnpj, UltimoNsu = "0" };

            int totalNovosDocumentos = 0;
            string nsuAtual = controle.UltimoNsu;
            bool haMaisDocumentos;

            do
            {
                haMaisDocumentos = false; // Começa assumindo que este é o último lote
                var resposta = servico.NfeDistDFeInteresse(cfg.cUF.ToString(), cnpj, nsuAtual);

                if (resposta.Retorno.cStat == 137) // 137 = Nenhum documento localizado
                {
                    break; // Sai do loop, não há nada a fazer
                }

                if (resposta.Retorno.cStat != 138) // 138 = Documento(s) localizado(s)
                {
                    throw new Exception($"Erro ao consultar SEFAZ ({resposta.Retorno.cStat}): {resposta.Retorno.xMotivo}");
                }

                // Atualiza o NSU para a próxima consulta (se houver)
                nsuAtual = resposta.Retorno.ultNSU.ToString();

                if (resposta.Retorno.loteDistDFeInt != null)
                {
                    foreach (var doc in resposta.Retorno.loteDistDFeInt)
                    {
                        string xmlString = string.Empty;
                        try
                        {
                            xmlString = DescompactarGzipParaString(doc.XmlNfe);

                            if (doc.schema.Contains("procNFe")) // Se for uma nota completa
                            {
                                var nfeProc = FuncoesXml.XmlStringParaClasse<NFe.Classes.nfeProc>(xmlString);
                                if (nfeProc?.NFe?.infNFe == null) continue;

                                var nfe = nfeProc.NFe;
                                var chaveAcesso = nfe.infNFe.Id.Substring(3);
                                var notaExistente = await _dfeRepository.GetNotaPorChaveAcessoAsync(chaveAcesso);

                                if (notaExistente != null)
                                {
                                    notaExistente.DataEmissao = nfe.infNFe.ide.dhEmi.DateTime.ToUniversalTime();
                                    notaExistente.XmlCompleto = xmlString;
                                    notaExistente.Nsu = doc.NSU.ToString();
                                    notaExistente.Itens.Clear();
                                    notaExistente.NaturezaOperacao = nfe.infNFe.ide.natOp.ToString();
                                    notaExistente.ValorIcms = nfe.infNFe.total.ICMSTot.vICMS;
                                    notaExistente.ValorIpi = nfe.infNFe.total.ICMSTot.vIPI;
                                    notaExistente.ValorProdutos = nfe.infNFe.total.ICMSTot.vProd;
                                    notaExistente.ValorBaseIcms = nfe.infNFe.total.ICMSTot.vBC;
                                    foreach (var item in nfe.infNFe.det)
                                    {
                                        notaExistente.Itens.Add(new DfeItemSincronizado
                                        {
                                            CodigoFornecedor = item.prod.cProd,
                                            DescricaoProduto = item.prod.xProd,
                                            Ncm = item.prod.NCM,
                                            Cfop = item.prod.CFOP.ToString(),
                                            Unidade = item.prod.uCom,
                                            Quantidade = item.prod.qCom,
                                            ValorUnitario = item.prod.vUnCom,
                                            ValorTotal = item.prod.vProd,
                                            CodigoBarras = string.IsNullOrEmpty(item.prod.cEAN) ? null : item.prod.cEAN,
                                            Origem = ExtrairOrigemDoIcms(item.imposto.ICMS),
                                            Cest = item.prod.CEST,
                                            Cst = ExtrairCstDoIcms(item.imposto.ICMS)

                                        });
                                    }
                                    await _dfeRepository.AtualizarNotaAsync(notaExistente);
                                    totalNovosDocumentos++;
                                }
                                else
                                {
                                    var novaNota = CriarNovaNotaCompleta(nfe, xmlString, idEmpresa, cnpj);
                                    novaNota.Nsu = doc.NSU.ToString();
                                    await _dfeRepository.SalvarNotaAsync(novaNota);
                                    totalNovosDocumentos++;
                                }
                            }
                            else if (doc.schema.Contains("resNFe")) // Se for um resumo
                            {
                                var resNFe = FuncoesXml.XmlStringParaClasse<resNFe>(xmlString);
                                if (resNFe == null) continue;

                                var notaExistente = await _dfeRepository.GetNotaPorChaveAcessoAsync(resNFe.chNFe);
                                if (notaExistente == null)
                                {
                                    var novaNota = CriarNovaNotaResumo(resNFe, xmlString, idEmpresa, cnpj);
                                    novaNota.Nsu = doc.NSU.ToString();
                                    await _dfeRepository.SalvarNotaAsync(novaNota);
                                }

                                // Sempre conta o documento processado
                                totalNovosDocumentos++;
                            }
                        }
                        catch (Exception ex)
                        {
                            string erroCompleto = $"Falha ao processar documento NSU {doc.NSU}. Erro: {ex.Message}";
                            if (ex.InnerException != null)
                            {
                                erroCompleto += $"\nDetalhes Internos: {ex.InnerException.Message}";
                            }
                            System.Diagnostics.Debug.WriteLine(erroCompleto);
                            System.Diagnostics.Debug.WriteLine("XML com problema:\n" + xmlString);
                            continue; // Pula para o próximo documento em caso de erro
                        }
                    }
                }

                // Verifica se há mais documentos a serem buscados
                if (long.Parse(resposta.Retorno.ultNSU.ToString()) < long.Parse(resposta.Retorno.maxNSU.ToString()))
                {
                    haMaisDocumentos = true;
                }

            } while (haMaisDocumentos);

            // Ao final de todo o processo, salva o último NSU que foi efetivamente consultado.
            controle.UltimoNsu = nsuAtual;
            controle.UltimaConsulta = DateTime.UtcNow;
            await _controleDfeRepository.SalvarControleAsync(controle);

            return totalNovosDocumentos;
        }


        // Métodos auxiliares para organizar o código
        private DfeEntradaSincronizada CriarNovaNotaCompleta(NFe.Classes.NFe nfe, string xml, int idEmpresa, string cnpj)
        {
            var nota = new DfeEntradaSincronizada
            {
                ChaveAcesso = nfe.infNFe.Id.Substring(3),
                NumeroNota = nfe.infNFe.ide.nNF.ToString(),
                Serie = nfe.infNFe.ide.serie.ToString(),
                Modelo = nfe.infNFe.ide.mod.ToString(),
                NaturezaOperacao = nfe.infNFe.ide.natOp.ToString(),
                UfEmitente = nfe.infNFe.ide.cUF.ToString(),
                MunicipioEmitente = nfe.infNFe.emit.enderEmit.xMun.ToString(),
                NomeEmitente = nfe.infNFe.emit.xNome,
                CnpjEmitente = nfe.infNFe.emit.CNPJ,
                DataEmissao = nfe.infNFe.ide.dhEmi.DateTime.ToUniversalTime(),
                ValorTotal = nfe.infNFe.total.ICMSTot.vNF,
                XmlCompleto = xml,
                ManifestoStatus = "Pendente",
                IdEmpresa = idEmpresa,
                CnpjDestinatario = cnpj,
                ValorProdutos = nfe.infNFe.total.ICMSTot.vProd,
                ValorIcms = nfe.infNFe.total.ICMSTot.vICMS
               
            };

            foreach (var item in nfe.infNFe.det)
            {
                nota.Itens.Add(new DfeItemSincronizado
                {
                    CodigoFornecedor = item.prod.cProd,
                    DescricaoProduto = item.prod.xProd,
                    Ncm = item.prod.NCM,
                    Cfop = item.prod.CFOP.ToString(),
                    Unidade = item.prod.uCom,
                    Quantidade = item.prod.qCom,
                    ValorUnitario = item.prod.vUnCom,
                    ValorTotal = item.prod.vProd,
                    CodigoBarras = string.IsNullOrEmpty(item.prod.cEAN) ? null : item.prod.cEAN,
                    Origem = ExtrairOrigemDoIcms(item.imposto.ICMS),
                    Cst = ExtrairCstDoIcms(item.imposto.ICMS),
                    Cest = item.prod.CEST
                    
                });
            }
            return nota;
        }

        private DfeEntradaSincronizada CriarNovaNotaResumo(resNFe resNFe, string xml, int idEmpresa, string cnpj)
        {
            return new DfeEntradaSincronizada
            {
                ChaveAcesso = resNFe.chNFe,
                NumeroNota = resNFe.chNFe.Substring(25, 9),
                Serie = resNFe.chNFe.Substring(22, 3),
                NomeEmitente = resNFe.xNome,
                CnpjEmitente = resNFe.CNPJ,
                DataEmissao = resNFe.dhEmi.ToUniversalTime(),
                ValorTotal = resNFe.vNF,
                XmlCompleto = xml,
                ManifestoStatus = "Pendente",
                IdEmpresa = idEmpresa,
                CnpjDestinatario = cnpj
            };
        }



        public async Task<string> EnviarManifestoAsync(int idEmpresa, string cnpj, string chaveAcesso, TipoEventoManifestacao tipoEvento)
        {
            var empresa = await _empresaRepository.GetByIdAsync(idEmpresa);
            if (empresa == null) throw new Exception($"Empresa com ID {idEmpresa} não encontrada.");

            var cfg = ZeusConfigHelper.CriarConfiguracao(empresa, 1, ModeloDocumento.NFe);
            var servico = new ServicosNFe(cfg);

            // Convertendo a nossa Enum para o tipo esperado pela biblioteca Zeus
            var tipoEventoZeus = (NFeTipoEvento)tipoEvento;

            // **USANDO O MÉTODO CORRETO QUE VOCÊ ENCONTROU**
            var resposta = servico.RecepcaoEventoManifestacaoDestinatario(1, 1, chaveAcesso, tipoEventoZeus, cnpj);

            // Acessando o retorno de forma correta
            var retornoEvento = resposta.Retorno.retEvento.FirstOrDefault();
            if (retornoEvento == null || retornoEvento.infEvento.cStat != 135) // 135: Evento registrado
            {
                var motivo = retornoEvento?.infEvento.xMotivo ?? resposta.Retorno.xMotivo;
                var cstat = retornoEvento?.infEvento.cStat ?? resposta.Retorno.cStat;
                throw new Exception($"Erro da SEFAZ ({cstat}): {motivo}");
            }

            string novoStatus = tipoEvento switch
            {
                TipoEventoManifestacao.ConfirmacaoDaOperacao => "Confirmada",
                TipoEventoManifestacao.DesconhecimentoDaOperacao => "Desconhecida",
                TipoEventoManifestacao.OperacaoNaoRealizada => "Operacao nao Realizada",
                TipoEventoManifestacao.CienciaDaEmissao => "Ciencia",
                _ => "Pendente"
            };

            await _dfeRepository.AtualizarStatusManifestoAsync(chaveAcesso, novoStatus);
            return novoStatus;
        }




        // ===================================================================
        // **MÉTODO DE DESCOMPACTAÇÃO ADICIONADO DIRETAMENTE AQUI NA CLASSE**
        // ===================================================================
        private string DescompactarGzipParaString(byte[] dadosCompactados)
        {
            if (dadosCompactados == null || dadosCompactados.Length == 0)
                return string.Empty;

            using (var memoryStream = new MemoryStream(dadosCompactados))
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    using (var streamReader = new StreamReader(gzipStream, Encoding.UTF8))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }




        /// <summary>
        /// Obtém o valor de um atributo [XmlEnum] de um membro de um enum.
        /// Esta função é necessária porque a versão atual da biblioteca DFe.Utils
        /// não possui um método direto para esta operação.
        /// </summary>
        /// <param name="valorEnum">O valor do enum a ser inspecionado.</param>
        /// <returns>A string contida no atributo [XmlEnum], ou o nome do enum se o atributo não for encontrado.</returns>
        private string? ObterValorDoXmlEnum(Enum valorEnum)
        {
            if (valorEnum == null) return null;
            var tipoEnum = valorEnum.GetType();
            var nomeMembro = Enum.GetName(tipoEnum, valorEnum);
            if (nomeMembro == null) return null;

            var membroInfo = tipoEnum.GetField(nomeMembro);
            var atributo = (XmlEnumAttribute?)membroInfo?.GetCustomAttributes(typeof(XmlEnumAttribute), false).FirstOrDefault();

            // Retorna o valor do atributo (ex: "60"), ou o nome do próprio enum se o atributo não existir.
            return atributo?.Name ?? nomeMembro;
        }


        // ===================================================================
        // MÉTODO DE EXTRAIR ORIGEM ATUALIZADO
        // ===================================================================
        private string? ExtrairOrigemDoIcms(ICMS icms)
        {
            if (icms?.TipoICMS == null) return null;

            switch (icms.TipoICMS)
            {
                case ICMS00 i: return ObterValorDoXmlEnum(i.orig);
                case ICMS10 i: return ObterValorDoXmlEnum(i.orig);
                case ICMS20 i: return ObterValorDoXmlEnum(i.orig);
                case ICMS30 i: return ObterValorDoXmlEnum(i.orig);
                case ICMS40: return "4"; // ICMS40/41/50 têm origem fixa, mas não o campo 'orig'. Retornamos o valor esperado.
                case ICMS51 i: return ObterValorDoXmlEnum(i.orig);
                case ICMS60 i: return ObterValorDoXmlEnum(i.orig);
                case ICMS70 i: return ObterValorDoXmlEnum(i.orig);
                case ICMS90 i: return ObterValorDoXmlEnum(i.orig);
                case ICMSSN101 i: return ObterValorDoXmlEnum(i.orig);
                case ICMSSN102: return null;
                case ICMSSN201 i: return ObterValorDoXmlEnum(i.orig);
                case ICMSSN202 i: return ObterValorDoXmlEnum(i.orig);
                case ICMSSN900 i: return ObterValorDoXmlEnum(i.orig);
                default: return null;
            }
        }


        // ===================================================================
        // MÉTODO DE EXTRAIR CST ATUALIZADO
        // ===================================================================
        private string? ExtrairCstDoIcms(ICMS icms)
        {
            if (icms?.TipoICMS == null) return null;

            // A CORREÇÃO é usar a nossa nova função 'ObterValorDoXmlEnum'.
            switch (icms.TipoICMS)
            {
                // Casos de Regime Normal (CST)
                case ICMS00 i: return ObterValorDoXmlEnum(i.CST);
                case ICMS10 i: return ObterValorDoXmlEnum(i.CST);
                case ICMS20 i: return ObterValorDoXmlEnum(i.CST);
                case ICMS30 i: return ObterValorDoXmlEnum(i.CST);
                case ICMS40 i: return ObterValorDoXmlEnum(i.CST);
                case ICMS51 i: return ObterValorDoXmlEnum(i.CST);
                case ICMS60 i: return ObterValorDoXmlEnum(i.CST);
                case ICMS70 i: return ObterValorDoXmlEnum(i.CST);
                case ICMS90 i: return ObterValorDoXmlEnum(i.CST);

                // Casos do Simples Nacional (CSOSN)
                case ICMSSN101 i: return ObterValorDoXmlEnum(i.CSOSN);
                case ICMSSN102 i: return ObterValorDoXmlEnum(i.CSOSN);
                case ICMSSN201 i: return ObterValorDoXmlEnum(i.CSOSN);
                case ICMSSN202 i: return ObterValorDoXmlEnum(i.CSOSN);
                case ICMSSN500 i: return ObterValorDoXmlEnum(i.CSOSN);
                case ICMSSN900 i: return ObterValorDoXmlEnum(i.CSOSN);

                default: return null;
            }
        }
    }
}
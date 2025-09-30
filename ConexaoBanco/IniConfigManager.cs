using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FluxoSistema.Infrastructure.ConexaoBanco
{
    public static class IniConfigManager
    {
        private static readonly string pastaRaiz = @"C:\ROCHA_SISTEMA";
        private static readonly string caminhoArquivo = Path.Combine(pastaRaiz, "config.ini");
        private static readonly string pastaModulosPadrao = Path.Combine(pastaRaiz, "MODULOS");

        // Propriedades para armazenar o que foi lido
        public static string Servidor { get; private set; }
        public static string Banco { get; private set; }
        public static string Usuario { get; private set; }
        public static string Senha { get; private set; }
        public static string PastaModulos { get; private set; }


        /// <summary>
        /// Monta e retorna a connection string para Npgsql
        /// </summary>
        public static string ConnectionString =>
            $"Host={Servidor};Database={Banco};Username={Usuario};Password={Senha};";


        public static void LoadConfig()
        {

            CriarEstruturaInicial();


            if (!File.Exists(caminhoArquivo))
                throw new FileNotFoundException("Arquivo de configuração não encontrado.", caminhoArquivo);

            string[] linhas = File.ReadAllLines(caminhoArquivo);
            foreach (string linha in linhas)
            {
                if (linha.StartsWith("Servidor="))
                    Servidor = linha.Substring(linha.IndexOf('=') + 1);
                else if (linha.StartsWith("Banco="))
                    Banco = linha.Substring(linha.IndexOf('=') + 1);
                else if (linha.StartsWith("Usuario="))
                    Usuario = linha.Substring(linha.IndexOf('=') + 1);
                else if (linha.StartsWith("Senha="))
                {
                    // Corrigido para capturar toda a string, inclusive os '=' de padding
                    string senhaCriptografada = linha.Substring(linha.IndexOf('=') + 1);
                    Senha = CriptografiaSimples.Descriptografar(senhaCriptografada);
                }
                else if (linha.StartsWith("PastaModulos="))
                    PastaModulos = linha.Substring(linha.IndexOf('=') + 1);
            }
        }


        private static void CriarEstruturaInicial()
        {
            // Criar a pasta base
            if (!Directory.Exists(pastaRaiz))
                Directory.CreateDirectory(pastaRaiz);

            // Criar a pasta de módulos
            if (!Directory.Exists(pastaModulosPadrao))
                Directory.CreateDirectory(pastaModulosPadrao);


        }

        public static void SaveConfig(string servidor, string banco, string usuario, string senhaPura, string pastaModulos)
        {
            // Criptografar a senha antes de salvar
            string senhaCriptografada = CriptografiaSimples.Criptografar(senhaPura);

            string[] conteudo = {
                "[Database]",
                $"Servidor={servidor}",
                $"Banco={banco}",
                $"Usuario={usuario}",
                $"Senha={senhaCriptografada}",
                "",
                "[Modulos]",
                $"PastaModulos={pastaModulos}"
            };

            File.WriteAllLines(caminhoArquivo, conteudo);

            // Atualiza as propriedades em memória
            Servidor = servidor;
            Banco = banco;
            Usuario = usuario;
            Senha = senhaPura;
            PastaModulos = pastaModulos;
        }

        private static class CriptografiaSimples
        {
            private static readonly byte[] chave = Encoding.UTF8.GetBytes("ChaveExemploAES1");
            private static readonly byte[] iv = Encoding.UTF8.GetBytes("VetordeInicial16");  // 16 caracteres exatos

            public static string Criptografar(string texto)
            {
                if (string.IsNullOrEmpty(texto)) return texto;

                using (Aes aes = Aes.Create())
                {
                    aes.Key = chave;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (MemoryStream ms = new MemoryStream())
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] inputBytes = Encoding.UTF8.GetBytes(texto);
                        cs.Write(inputBytes, 0, inputBytes.Length);
                        cs.FlushFinalBlock();

                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }

            public static string Descriptografar(string textoCriptografado)
            {
                if (string.IsNullOrEmpty(textoCriptografado)) return textoCriptografado;

                using (Aes aes = Aes.Create())
                {
                    aes.Key = chave;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    byte[] cipherBytes = Convert.FromBase64String(textoCriptografado);

                    using (MemoryStream ms = new MemoryStream())
                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.FlushFinalBlock();

                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
        }
    }
}

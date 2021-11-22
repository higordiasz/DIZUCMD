using DIZUCMD.Controllers.Instagrams;
using DIZUCMD.Models.Retornos;
using DIZUCMD.WebInstagram;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DIZUCMD.Models.Instagrams
{
    class Instagram
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("block")]
        public bool Block { get; set; }

        [JsonProperty("challenge")]
        public bool Challeng { get; set; }

        [JsonProperty("incorrect")]
        public bool Incorrect { get; set; }

        [JsonProperty("seguir")]
        public int Seguir { get; set; }

        [JsonProperty("curtir")]
        public int Curtir { get; set; }
    }

    class InstaController
    {
        public InstagramWeb Web { get; set; }
        public Instagram Insta { get; set; }
        public string ContaID { get; set; }
        public string PictureURL { get; set; }
        public bool isLogged { get; set; }
        public int Seguir { get; set; }
        public int Curtir { get; set; }
        public int Total { get; set; }
    }

    class ActionsList
    {
        public InstagramWeb Web { get; set; }
        public string Username { get; set; }
        public string ContaID { get; set; }
        public int Index { get; set; }
    }

    class InstaList {
        public string Username { get; set; }
        public string Url { get; set; }
        public string Seguir { get; set; }
        public string Curtir { get; set; }
        public string Color { get; set; }
    }

    class Sender
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }

    class CreateInsta
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }

    class AlterPassInsta
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("newpassword")]
        public string Password { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }

    class Receiver
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("erro")]
        public string Erro { get; set; }

        [JsonProperty("data")]
        public List<Instagram> Contas { get; set; }
    }

    class InstagramName
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }

    static class ExtendInstagram
    {

        private static string chave = "DJEHTR3USI5827CHSGTY274OPCJDE598";
        private static string vetorInicializacao = "SKEUJ492NVH4I8A9";

        private static Rijndael CriarInstanciaRijndael()
        {
            if (!(chave != null &&
                  (chave.Length == 16 ||
                   chave.Length == 24 ||
                   chave.Length == 32)))
            {
                throw new Exception(
                    "A chave de criptografia deve possuir " +
                    "16, 24 ou 32 caracteres.");
            }

            if (vetorInicializacao == null ||
                vetorInicializacao.Length != 16)
            {
                throw new Exception(
                    "O vetor de inicialização deve possuir " +
                    "16 caracteres.");
            }

            Rijndael algoritmo = Rijndael.Create();
            algoritmo.Key =
                Encoding.ASCII.GetBytes(chave);
            algoritmo.IV =
                Encoding.ASCII.GetBytes(vetorInicializacao);

            return algoritmo;
        }

        private static string Encriptar(string textoNormal)
        {
            if (String.IsNullOrWhiteSpace(textoNormal))
            {
                return textoNormal;
            }
            using (Rijndael algoritmo = CriarInstanciaRijndael())
            {
                ICryptoTransform encryptor =
                    algoritmo.CreateEncryptor(
                        algoritmo.Key, algoritmo.IV);

                using (MemoryStream streamResultado =
                       new MemoryStream())
                {
                    using (CryptoStream csStream = new CryptoStream(
                        streamResultado, encryptor,
                        CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer =
                            new StreamWriter(csStream))
                        {
                            writer.Write(textoNormal);
                        }
                    }

                    return ArrayBytesToHexString(
                        streamResultado.ToArray());
                }
            }
        }

        private static string ArrayBytesToHexString(byte[] conteudo)
        {
            string[] arrayHex = Array.ConvertAll(
                conteudo, b => b.ToString("X2"));
            return string.Concat(arrayHex);
        }

        private static string Decriptar(string textoEncriptado)
        {
            if (String.IsNullOrWhiteSpace(textoEncriptado))
            {
                return textoEncriptado;
            }

            if (textoEncriptado.Length % 2 != 0)
            {
                return textoEncriptado;
            }


            using (Rijndael algoritmo = CriarInstanciaRijndael())
            {
                ICryptoTransform decryptor =
                    algoritmo.CreateDecryptor(
                        algoritmo.Key, algoritmo.IV);

                string textoDecriptografado = null;
                using (MemoryStream streamTextoEncriptado =
                    new MemoryStream(
                        HexStringToArrayBytes(textoEncriptado)))
                {
                    using (CryptoStream csStream = new CryptoStream(
                        streamTextoEncriptado, decryptor,
                        CryptoStreamMode.Read))
                    {
                        using (StreamReader reader =
                            new StreamReader(csStream))
                        {
                            textoDecriptografado =
                                reader.ReadToEnd();
                        }
                    }
                }

                return textoDecriptografado;
            }
        }

        private static byte[] HexStringToArrayBytes(string conteudo)
        {
            int qtdeBytesEncriptados =
                conteudo.Length / 2;
            byte[] arrayConteudoEncriptado =
                new byte[qtdeBytesEncriptados];
            for (int i = 0; i < qtdeBytesEncriptados; i++)
            {
                arrayConteudoEncriptado[i] = Convert.ToByte(
                    conteudo.Substring(i * 2, 2), 16);
            }

            return arrayConteudoEncriptado;
        }

        /// <summary>
        /// Cadastrar um novo Instagram no banco de dados
        /// </summary>
        /// <param name="Insta">Instagram para salvar</param>
        /// <returns>Retorno da API</returns>
        public static Retorno SaveInstagram (this Instagram Insta)
        {
            Retorno Ret = new Retorno
            {
                Mensagem = "Não foi possivel cadastrar a conta do instagram.",
                Status = 0
            };
            if (String.IsNullOrEmpty(Insta.Username))
            {
                Ret.Status = 0;
                Ret.Mensagem = "Informe o usernamo da conta";
                return Ret;
            }
            if (String.IsNullOrEmpty(Insta.Password))
            {
                Ret.Status = 0;
                Ret.Mensagem = "Informe a senha da conta do instagram";
                return Ret;
            }
            Insta.Password = Encriptar(Insta.Password);
            var res = InstagramController.CreateInstagram(Insta);
            if (res.Status == 1)
            {
                Ret.Status = 1;
                Ret.Mensagem = "Sucesso ao salvar a conta";
                return Ret;
            } else
            {
                Ret.Status = res.Status;
                Ret.Mensagem = res.Erro;
                return Ret;
            }
        }

        /// <summary>
        /// Alterar conta do instagram
        /// </summary>
        /// <param name="username">Conta para alterar</param>
        /// <param name="pass">Nova senha para alterar</param>
        /// <returns>Retorno da API</returns>
        public static Receiver AlterPassword (string username, string pass)
        {
            var senha = Encriptar(pass);
            var res = InstagramController.AlterarInstagram(username, senha);
            return res;
        }

        /// <summary>
        /// Buscar instagram pelo username
        /// </summary>
        /// <param name="Insta">Instagram</param>
        /// <returns></returns>
        public static Instagram GetInstaByUsername (string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                return null;
            }
            var res = InstagramController.GetInstagramByUsername(username);
            if (res.Status == 1)
            {
                return res.Contas[0];
            } else
            {
                return null;
            }
        }

        /// <summary>
        /// Remover conta do instagram
        /// </summary>
        /// <param name="Insta">Conta para remover</param>
        /// <returns>Retorno da API</returns>
        public static Retorno RemoveInsta (string Insta)
        {
            Retorno Ret = new Retorno
            {
                Mensagem = "Não foi possivel remover a conta do instagram.",
                Status = 0
            };
            if (String.IsNullOrEmpty(Insta))
            {
                Ret.Status = 0;
                Ret.Mensagem = "Informe o usernamo da conta";
                return Ret;
            }
            var res = InstagramController.RemoveInstagramByUsername(Insta);
            if (res.Status == 1)
            {
                Ret.Status = 1;
                Ret.Mensagem = "Sucesso ao remover a conta";
                return Ret;
            }
            else
            {
                Ret.Status = res.Status;
                Ret.Mensagem = res.Erro;
                return Ret;
            }
        }

        /// <summary>
        /// Remove a criptografia das senhas do instagram.
        /// </summary>
        /// <param name="lista">Lista de contas para remover a criptografia</param>
        /// <returns>Retorna uma lista com as contas sem a criptografia</returns>
        public static Instagram ReverterSenha(this Instagram i)
        {
            i.Password = Decriptar(i.Password);
            return i;
        }

        /// <summary>
        /// Remove a criptografia das senhas do instagram.
        /// </summary>
        /// <param name="lista">Lista de contas para remover a criptografia</param>
        /// <returns>Retorna uma lista com as contas sem a criptografia</returns>
        public static List<Instagram> ReverterSenha (this List<Instagram> lista)
        {
            var aux = new List<Instagram>();
            foreach (Instagram i in lista)
            {
                i.Password = Decriptar(i.Password);
                aux.Add(i);
            }
            return aux;
        } 

        /// <summary>
        /// Função para adicionar tarefa de Seguir perfil.
        /// </summary>
        /// <param name="Insta">Conta para adicionar</param>
        public static void AdicionarSeguir (this Instagram Insta)
        {
            if (String.IsNullOrEmpty(Insta.Username)) return;
            InstagramController.AddSeguir(Insta);
            return;
        }

        /// <summary>
        /// Função para adicionar tarefa de curtir publicação.
        /// </summary>
        /// <param name="Insta">Conta para adicionar</param>
        public static void AdicionarCurtir(this Instagram Insta)
        {
            if (String.IsNullOrEmpty(Insta.Username)) return;
            InstagramController.AddCurtir(Insta);
            return;
        }

        /// <summary>
        /// Função para adicionar Block.
        /// </summary>
        /// <param name="Insta">Conta do Instagram</param>
        public static void AdicionarBlock (this Instagram Insta)
        {
            if (String.IsNullOrEmpty(Insta.Username)) return;
            InstagramController.AddBlock(Insta);
            return;
        }

        /// <summary>
        /// Função para remover Block.
        /// </summary>
        /// <param name="Insta">Conta do Instagram</param>
        public static void RemoverBlock(this Instagram Insta)
        {
            if (String.IsNullOrEmpty(Insta.Username)) return;
            InstagramController.RemoveBlock(Insta);
            return;
        }

        /// <summary>
        /// Função para adicionar Challenge.
        /// </summary>
        /// <param name="Insta">Conta do Instagram</param>
        public static void AdicionarChallenge(this Instagram Insta)
        {
            if (String.IsNullOrEmpty(Insta.Username)) return;
            InstagramController.AddChallenge(Insta);
            return;
        }

        /// <summary>
        /// Função para remvoer Challenge.
        /// </summary>
        /// <param name="Insta">Conta do Instagram</param>
        public static void RemoverChallenge(this Instagram Insta)
        {
            if (String.IsNullOrEmpty(Insta.Username)) return;
            InstagramController.RemoveChallenge(Insta);
            return;
        }

        /// <summary>
        /// Função para adicionar block de Incorrect.
        /// </summary>
        /// <param name="Insta">Conta do Instagram</param>
        public static void AdicionarIncorrect(this Instagram Insta)
        {
            if (String.IsNullOrEmpty(Insta.Username)) return;
            InstagramController.AddIncorrect(Insta);
            return;
        }

        /// <summary>
        /// Função para remover block de Incorrect.
        /// </summary>
        /// <param name="Insta">Conta do Instagram</param>
        public static void RemoverIncorrect(this Instagram Insta)
        {
            if (String.IsNullOrEmpty(Insta.Username)) return;
            InstagramController.RemoveIncorrect(Insta);
            return;
        }
    }

}

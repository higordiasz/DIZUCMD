using Newtonsoft.Json;
using DIZUCMD.Models.Grupos;
using System;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace DIZUCMD.Controllers.Grupos
{
    static class GrupoController
    {
        private static string BaseURL = "https://arka-site.herokuapp.com/api/";

        ///<summary>
        ///Buscar um grupo no banco de dados pelo Nome do mesmo.
        ///</summary>
        ///<param name="Nome">Nome do grupo que deseja carregar</param>
        public static Receiver GetGrupoByName(string Nome)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel localizar o grupo com o nome '{Nome}'.",
                Grupos = null
            };
            try
            {
                if (String.IsNullOrEmpty(Variaveis.Token))
                {
                    Receiver.Status = 0;
                    Receiver.Erro = "Não foi possivel localiozar o Token da conta.";
                    return Receiver;
                }
                if (String.IsNullOrEmpty(BaseURL))
                {
                    Receiver.Status = 0;
                    Receiver.Erro = "Impossivel se conectar com o servidor.";
                    return Receiver;
                }
                GroupName Sender = new GroupName
                {
                    Nome = Nome,
                    Token = Variaveis.Token
                };
                using (var cliente = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                    string adress = BaseURL + "getgroup";
                    var serializedSender = JsonConvert.SerializeObject(Sender);
                    var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                    var request = cliente.PostAsync(adress, content).Result;
                    if (request.IsSuccessStatusCode)
                    {
                        var aux = request.Content.ReadAsStringAsync().Result;
                        Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                        return Receiver;
                    }
                    Receiver.Erro = $"Não foi possivel localizar o grupo com o nome '{Nome}'.";
                    Receiver.Status = 0;
                    return Receiver;
                }
            }
            catch (Exception err)
            {
                Receiver.Status = 0;
                Receiver.Erro = $"Erro ao realizar a requisição: '{err.Message}'.";
                return Receiver;
            }
        }

        ///<summary>
        ///Buscar todos os grupo do banco de dados.
        ///</summary>
        public static Receiver GetAllGroup()
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel localizar os Globais da sua conta.",
                Grupos = null
            };
            try
            {
                if (String.IsNullOrEmpty(Variaveis.Token))
                {
                    Receiver.Status = 0;
                    Receiver.Erro = "Não foi possivel localiozar o Token da conta.";
                    return Receiver;
                }
                if (String.IsNullOrEmpty(BaseURL))
                {
                    Receiver.Status = 0;
                    Receiver.Erro = "Impossivel se conectar com o servidor.";
                    return Receiver;
                }
                GroupName Sender = new GroupName
                {
                    Nome = "",
                    Token = Variaveis.Token
                };
                using (var cliente = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                    string adress = BaseURL + "getallgroups";
                    var serializedSender = JsonConvert.SerializeObject(Sender);
                    var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                    var request = cliente.PostAsync(adress, content).Result;
                    if (request.IsSuccessStatusCode)
                    {
                        var aux = request.Content.ReadAsStringAsync().Result;
                        Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                        return Receiver;
                    }
                    Receiver.Erro = $"Não foi possivel localizar os Globais da sua conta.";
                    Receiver.Status = 0;
                    return Receiver;
                }
            }
            catch (Exception err)
            {
                Receiver.Status = 0;
                Receiver.Erro = $"Erro ao realizar a requisição: '{err.Message}'.";
                return Receiver;
            }
        }

        ///<summary>
        ///Remover um grupo do banco de dados.
        ///</summary>
        public static Receiver RemoveGroupByName(string Nome)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel localizar o global com o nome '{Nome}'.",
                Grupos = null
            };
            try
            {
                if (String.IsNullOrEmpty(Variaveis.Token))
                {
                    Receiver.Status = 0;
                    Receiver.Erro = "Não foi possivel localiozar o Token da conta.";
                    return Receiver;
                }
                if (String.IsNullOrEmpty(BaseURL))
                {
                    Receiver.Status = 0;
                    Receiver.Erro = "Impossivel se conectar com o servidor.";
                    return Receiver;
                }
                GroupName Sender = new GroupName
                {
                    Nome = Nome,
                    Token = Variaveis.Token
                };
                using (var cliente = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                    string adress = BaseURL + "removegroup";
                    var serializedSender = JsonConvert.SerializeObject(Sender);
                    var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                    var request = cliente.PostAsync(adress, content).Result;
                    if (request.IsSuccessStatusCode)
                    {
                        var aux = request.Content.ReadAsStringAsync().Result;
                        Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                        return Receiver;
                    }
                    Receiver.Erro = $"Não foi possivel localizar o global com o nome '{Nome}'.";
                    Receiver.Status = 0;
                    return Receiver;
                }
            }
            catch (Exception err)
            {
                Receiver.Status = 0;
                Receiver.Erro = $"Erro ao realizar a requisição: '{err.Message}'.";
                return Receiver;
            }
        }

        ///<summary>
        ///Cadastrar novo grupo no banco de dados.
        ///</summary>
        public static Receiver CreateGroup(SenderGroup G)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel cadastrar o global com o nome '{G.Nome}'.",
                Grupos = null
            };
            try
            {
                if (String.IsNullOrEmpty(Variaveis.Token))
                {
                    Receiver.Status = 0;
                    Receiver.Erro = "Não foi possivel localiozar o Token da conta.";
                    return Receiver;
                }
                if (String.IsNullOrEmpty(BaseURL))
                {
                    Receiver.Status = 0;
                    Receiver.Erro = "Impossivel se conectar com o servidor.";
                    return Receiver;
                }
                using (var cliente = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                    string adress = BaseURL + "creategroup";
                    var serializedSender = JsonConvert.SerializeObject(G);
                    var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                    var request = cliente.PostAsync(adress, content).Result;
                    if (request.IsSuccessStatusCode)
                    {
                        var aux = request.Content.ReadAsStringAsync().Result;
                        Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                        return Receiver;
                    }
                    Receiver.Erro = $"Não foi possivel cadastrar o global com o nome '{G.Nome}'.";
                    Receiver.Status = 0;
                    return Receiver;
                }
            }
            catch (Exception err)
            {
                Receiver.Status = 0;
                Receiver.Erro = $"Erro ao realizar a requisição: '{err.Message}'.";
                return Receiver;
            }
        }

        ///<summary>
        ///Alterar um grupo no banco de dados.
        ///</summary>
        public static Receiver AlterGroup(SenderGroup G)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel alterar o global com o nome '{G.Nome}'.",
                Grupos = null
            };
            try
            {
                if (String.IsNullOrEmpty(Variaveis.Token))
                {
                    Receiver.Status = 0;
                    Receiver.Erro = "Não foi possivel localiozar o Token da conta.";
                    return Receiver;
                }
                if (String.IsNullOrEmpty(BaseURL))
                {
                    Receiver.Status = 0;
                    Receiver.Erro = "Impossivel se conectar com o servidor.";
                    return Receiver;
                }
                using (var cliente = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                    string adress = BaseURL + "altergroup";
                    var serializedSender = JsonConvert.SerializeObject(G);
                    var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                    var request = cliente.PostAsync(adress, content).Result;
                    if (request.IsSuccessStatusCode)
                    {
                        var aux = request.Content.ReadAsStringAsync().Result;
                        Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                        return Receiver;
                    }
                    Receiver.Erro = $"Não foi possivel alterar o global com o nome '{G.Nome}'.";
                    Receiver.Status = 0;
                    return Receiver;
                }
            }
            catch (Exception err)
            {
                Receiver.Status = 0;
                Receiver.Erro = $"Erro ao realizar a requisição: '{err.Message}'.";
                return Receiver;
            }
        }
    }
}

using Newtonsoft.Json;
using DIZUCMD.Models.Globals;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace DIZUCMD.Controllers.Globals
{
    static class GlobalController
    {
        private static string BaseURL = "https://arka-site.herokuapp.com/api/";

        ///<summary>
        ///Buscar um global no banco de dados pelo Nome do mesmo.
        ///</summary>
        ///<param name="Nome">Nome do global que deseja carregar</param>
        public static Receiver GetGlobalByName(string Nome)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel localizar o global com o nome '{Nome}'.",
                Globais = null
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
                GlobalName Sender = new GlobalName
                {
                    Nome = Nome,
                    Token = Variaveis.Token
                };
                using (var cliente = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                    string adress = BaseURL + "getglobal";
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
            } catch (Exception err)
            {
                Receiver.Status = 0;
                Receiver.Erro = $"Erro ao realizar a requisição: '{err.Message}'.";
                return null;
            }
        }

        ///<summary>
        ///Buscar todos os globais do banco de dados.
        ///</summary>
        public static Receiver GetAllGlobal()
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel localizar os Globais da sua conta.",
                Globais = null
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
                GlobalName Sender = new GlobalName
                {
                    Nome = "",
                    Token = Variaveis.Token
                };
                using (var cliente = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                    string adress = BaseURL + "getallglobals";
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
                return null;
            }
        }

        ///<summary>
        ///Remover um Global do banco de dados.
        ///</summary>
        public static Receiver RemoveGlobalByName(string Nome)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel localizar o global com o nome '{Nome}'.",
                Globais = null
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
                GlobalName Sender = new GlobalName
                {
                    Nome = Nome,
                    Token = Variaveis.Token
                };
                using (var cliente = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                    string adress = BaseURL + "removeglobal";
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
                return null;
            }
        }

        ///<summary>
        ///Cadastrar novo Global no banco de dados.
        ///</summary>
        public static Receiver CreateGlobal (GlobalSender G)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel cadastrar o global com o nome '{G.Nome}'.",
                Globais = null
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
                    string adress = BaseURL + "createglobal";
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
                return null;
            }
        }

        ///<summary>
        ///Alterar um Global no banco de dados.
        ///</summary>
        public static Receiver AlterGlobal(GlobalSender G)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel alterar o global com o nome '{G.Nome}'.",
                Globais = null
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
                    string adress = BaseURL + "alterglobal";
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

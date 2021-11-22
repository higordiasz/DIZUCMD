using Newtonsoft.Json;
using DIZUCMD.Models.Users;
using System;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;

namespace DIZUCMD.Controllers.Users
{
    static class UserController
    {
        private static string BaseURL = "https://arka-site.herokuapp.com/api/";
        public static string Token { get; set; }

        static public Receiver CheckToken (string token)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Impossivel se conectar com o servidor.",
                Conta = null
            };
            try
            {
                if (String.IsNullOrEmpty(BaseURL))
                {
                    Receiver.Status = 0;
                    Receiver.Erro = "Impossivel se conectar com o servidor.";
                    return Receiver;
                }
                if (String.IsNullOrEmpty(token))
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
                    string adress = BaseURL + "dizu/checktoken";
                    var content = new StringContent($"{{\"token\":\"{token}\"}}", Encoding.UTF8, "application/json");
                    var request = cliente.PostAsync(adress, content).Result;
                    if (request.IsSuccessStatusCode)
                    {
                        var aux = request.Content.ReadAsStringAsync().Result;
                        Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                        return Receiver;
                    }
                    Receiver.Status = 0;
                    return Receiver;
                }
            } catch (Exception err)
            {
                Receiver.Status = 0;
                Receiver.Erro = $"Erro ao realizar a requisição: '{err.Message}'.";
                return Receiver;
            }
        }

        static public Receiver LoginSystem (Models.Users.Login Usuario)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel localizar o usuario com o email '{Usuario.Email}'.",
                Conta = null
            };
            try
            {
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
                    string adress = BaseURL + "login";
                    var serializedSender = JsonConvert.SerializeObject(Usuario);
                    var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                    var request = cliente.PostAsync(adress, content).Result;
                    if (request.IsSuccessStatusCode)
                    {
                        var aux = request.Content.ReadAsStringAsync().Result;
                        Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                        return Receiver;
                    }
                    Receiver.Erro = $"Não foi possivel localizar o usuario com o email '{Usuario.Email}'.";
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

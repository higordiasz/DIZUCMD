using Newtonsoft.Json;
using DIZUCMD.Models.Instagrams;
using DIZUCMD.Models.Retornos;
using System;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;

namespace DIZUCMD.Controllers.Instagrams
{
    static class InstagramController
    {
        private static string BaseURL = "https://arkabot.com.br/api/dizu/";

        /// <summary>
        /// Cadastrar uma conta do instagram no banco de dados
        /// </summary>
        /// <param name="Insta">Conta do Instagram para cadastro</param>
        /// <returns>Retorno da API</returns>
        static public Receiver CreateInstagram (Instagram Insta)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel cadastrar o instagram com o nome '{Insta.Username}'.",
                Contas = null
            };
            try
            {
                using var cliente = new HttpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                string adress = BaseURL + "addinsta";
                CreateInsta sender = new CreateInsta
                {
                    Password = Insta.Password,
                    Token = Variaveis.Token,
                    Username = Insta.Username
                };
                var serializedSender = JsonConvert.SerializeObject(sender);
                var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                var request = cliente.PostAsync(adress, content).Result;
                if (request.IsSuccessStatusCode)
                {
                    var aux = request.Content.ReadAsStringAsync().Result;
                    Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                    return Receiver;
                }
                Receiver.Erro = $"Não foi possivel cadastrar o instagram com o nome '{Insta.Username}'.";
                Receiver.Status = 0;
                return Receiver;
            }
            catch (Exception err)
            {
                Receiver.Status = 0;
                Receiver.Erro = $"Erro ao realizar a requisição: '{err.Message}'.";
                return Receiver;
            }
        } 

        /// <summary>
        /// Remover uma conta do instagram pelo Username da conta
        /// </summary>
        /// <param name="Username">Username da conta</param>
        /// <returns>Retorno da API</returns>
        static public Receiver RemoveInstagramByUsername (string Username)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel localizar o instagram com o nome '{Username}'.",
                Contas = null
            };
            try
            {
                InstagramName Sender = new InstagramName
                {
                    Username = Username,
                    Token = Variaveis.Token
                };
                using var cliente = new HttpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                string adress = BaseURL + "removeinsta";
                var serializedSender = JsonConvert.SerializeObject(Sender);
                var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                var request = cliente.PostAsync(adress, content).Result;
                if (request.IsSuccessStatusCode)
                {
                    var aux = request.Content.ReadAsStringAsync().Result;
                    Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                    return Receiver;
                }
                Receiver.Erro = $"Não foi possivel remover o instagram com o nome '{Username}'.";
                Receiver.Status = 0;
                return Receiver;
            }
            catch (Exception err)
            {
                Receiver.Status = 0;
                Receiver.Erro = $"Erro ao realizar a requisição: '{err.Message}'.";
                return Receiver;
            }
        }

        /// <summary>
        /// Alterar uma conta do instagram no banco de dados
        /// </summary>
        /// <param name="Insta">Canta para alterar</param>
        /// <returns>Retorno da API</returns>
        static public Receiver AlterarInstagram (string Username, string Password)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel alterar o instagram com o nome '{Username}'.",
                Contas = null
            };
            try
            {
                using var cliente = new HttpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                string adress = BaseURL + "alterinsta";
                AlterPassInsta Sender = new AlterPassInsta
                {
                    Password = Password,
                    Token = Variaveis.Token,
                    Username = Username
                };
                var serializedSender = JsonConvert.SerializeObject(Sender);
                var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                var request = cliente.PostAsync(adress, content).Result;
                if (request.IsSuccessStatusCode)
                {
                    var aux = request.Content.ReadAsStringAsync().Result;
                    Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                    return Receiver;
                }
                Receiver.Erro = $"Não foi possivel alterar o instagram com o nome '{Username}'.";
                Receiver.Status = 0;
                return Receiver;
            }
            catch (Exception err)
            {
                Receiver.Status = 0;
                Receiver.Erro = $"Erro ao realizar a requisição: '{err.Message}'.";
                return Receiver;
            }
        }

        /// <summary>
        /// Buscar uma conta do Instagram pelo Username
        /// </summary>
        /// <param name="username">Username da conta</param>
        /// <returns>Retorno da API</returns>
        static public Receiver GetInstagramByUsername (string username)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel localizar o instagram com o nome '{username}'.",
                Contas = null
            };
            try
            {
                InstagramName Sender = new InstagramName
                {
                    Username = username,
                    Token = Variaveis.Token
                };
                using var cliente = new HttpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                string adress = BaseURL + "getinstagram";
                var serializedSender = JsonConvert.SerializeObject(Sender);
                var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                var request = cliente.PostAsync(adress, content).Result;
                if (request.IsSuccessStatusCode)
                {
                    var aux = request.Content.ReadAsStringAsync().Result;
                    Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                    if (Receiver.Contas.Count > 0)
                    {
                        Receiver.Contas[0] = Receiver.Contas[0].ReverterSenha();
                    }
                    return Receiver;
                }
                Receiver.Erro = $"Não foi possivel localizar o instagram com o nome '{username}'.";
                Receiver.Status = 0;
                return Receiver;
            }
            catch (Exception err)
            {
                Receiver.Status = 0;
                Receiver.Erro = $"Erro ao realizar a requisição: '{err.Message}'.";
                return Receiver;
            }
        }

        /// <summary>
        /// Buscar todas as contas no banco de dados
        /// </summary>
        /// <returns>Retorno da API</returns>
        static public Receiver GetAllInstagram ()
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel localizar o instagram com o nome.",
                Contas = null
            };
            try
            {
                InstagramName Sender = new InstagramName
                {
                    Username = "",
                    Token = Variaveis.Token
                };
                using var cliente = new HttpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                string adress = BaseURL + "getallinstagram";
                var serializedSender = JsonConvert.SerializeObject(Sender);
                var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                var request = cliente.PostAsync(adress, content).Result;
                if (request.IsSuccessStatusCode)
                {
                    var aux = request.Content.ReadAsStringAsync().Result;
                    Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                    if (Receiver.Contas.Count > 0)
                    {
                        Receiver.Contas = Receiver.Contas.ReverterSenha();
                    }
                    return Receiver;
                }
                Receiver.Erro = $"Não foi possivel localizar o instagram com o nome.";
                Receiver.Status = 0;
                return Receiver;
            }
            catch (Exception err)
            {
                Receiver.Status = 0;
                Receiver.Erro = $"Erro ao realizar a requisição: '{err.Message}'.";
                return Receiver;
            }
        }

        /// <summary>
        /// Adicionar Block na conta do Instagram
        /// </summary>
        /// <param name="Insta">Conta para adicionar</param>
        /// <returns>Retorno da API</returns>
        static public Receiver AddBlock (Instagram Insta)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel localizar o instagram com o nome '{Insta.Username}'.",
                Contas = null
            };
            try
            {
                using var cliente = new HttpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                string adress = BaseURL + "addblock";
                Sender s = new Sender
                {
                    Token = Variaveis.Token,
                    Username = Insta.Username
                };
                var serializedSender = JsonConvert.SerializeObject(s);
                var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                var request = cliente.PostAsync(adress, content).Result;
                if (request.IsSuccessStatusCode)
                {
                    var aux = request.Content.ReadAsStringAsync().Result;
                    Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                    return Receiver;
                }
                Receiver.Erro = $"Não foi possivel localizar o instagram com o nome '{Insta.Username}'.";
                Receiver.Status = 0;
                return Receiver;
            }
            catch (Exception err)
            {
                Receiver.Status = 0;
                Receiver.Erro = $"Erro ao realizar a requisição: '{err.Message}'.";
                return Receiver;
            }
        }

        /// <summary>
        /// Remover bloqueio da conta do instagram
        /// </summary>
        /// <param name="Insta">Conta para remover</param>
        /// <returns>Retorno da API</returns>
        static public Receiver RemoveBlock (Instagram Insta)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel localizar o instagram com o nome '{Insta.Username}'.",
                Contas = null
            };
            try
            {
                using var cliente = new HttpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                string adress = BaseURL + "removeblock";
                Sender s = new Sender
                {
                    Token = Variaveis.Token,
                    Username = Insta.Username
                };
                var serializedSender = JsonConvert.SerializeObject(s);
                var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                var request = cliente.PostAsync(adress, content).Result;
                if (request.IsSuccessStatusCode)
                {
                    var aux = request.Content.ReadAsStringAsync().Result;
                    Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                    return Receiver;
                }
                Receiver.Erro = $"Não foi possivel localizar o instagram com o nome '{Insta.Username}'.";
                Receiver.Status = 0;
                return Receiver;
            }
            catch (Exception err)
            {
                Receiver.Status = 0;
                Receiver.Erro = $"Erro ao realizar a requisição: '{err.Message}'.";
                return Receiver;
            }
        }

        /// <summary>
        /// Adicionar Challenge na conta do instagram
        /// </summary>
        /// <param name="Insta">Conta para adicionar</param>
        /// <returns>Retorno da API</returns>
        static public Receiver AddChallenge (Instagram Insta)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel localizar o instagram com o nome '{Insta.Username}'.",
                Contas = null
            };
            try
            {
                using var cliente = new HttpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                string adress = BaseURL + "addchallenge";
                Sender s = new Sender
                {
                    Token = Variaveis.Token,
                    Username = Insta.Username
                };
                var serializedSender = JsonConvert.SerializeObject(s);
                var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                var request = cliente.PostAsync(adress, content).Result;
                if (request.IsSuccessStatusCode)
                {
                    var aux = request.Content.ReadAsStringAsync().Result;
                    Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                    return Receiver;
                }
                Receiver.Erro = $"Não foi possivel localizar o instagram com o nome '{Insta.Username}'.";
                Receiver.Status = 0;
                return Receiver;
            }
            catch (Exception err)
            {
                Receiver.Status = 0;
                Receiver.Erro = $"Erro ao realizar a requisição: '{err.Message}'.";
                return Receiver;
            }
        }

        /// <summary>
        /// Remover Challenge da conta do instagram
        /// </summary>
        /// <param name="Insta">Conta para remover</param>
        /// <returns>Retorno da API</returns>
        static public Receiver RemoveChallenge (Instagram Insta)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel localizar o instagram com o nome '{Insta.Username}'.",
                Contas = null
            };
            try
            {
                using var cliente = new HttpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                string adress = BaseURL + "removechallenge";
                Sender s = new Sender
                {
                    Token = Variaveis.Token,
                    Username = Insta.Username
                };
                var serializedSender = JsonConvert.SerializeObject(s);
                var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                var request = cliente.PostAsync(adress, content).Result;
                if (request.IsSuccessStatusCode)
                {
                    var aux = request.Content.ReadAsStringAsync().Result;
                    Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                    return Receiver;
                }
                Receiver.Erro = $"Não foi possivel localizar o instagram com o nome '{Insta.Username}'.";
                Receiver.Status = 0;
                return Receiver;
            }
            catch (Exception err)
            {
                Receiver.Status = 0;
                Receiver.Erro = $"Erro ao realizar a requisição: '{err.Message}'.";
                return Receiver;
            }
        }

        /// <summary>
        /// Adicionar Incorrect na conta do instagram
        /// </summary>
        /// <param name="Insta">Conta para adicionar</param>
        /// <returns>Retorno da API</returns>
        static public Receiver AddIncorrect (Instagram Insta)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel localizar o instagram com o nome '{Insta.Username}'.",
                Contas = null
            };
            try
            {
                using var cliente = new HttpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                string adress = BaseURL + "addincorrect";
                Sender s = new Sender
                {
                    Token = Variaveis.Token,
                    Username = Insta.Username
                };
                var serializedSender = JsonConvert.SerializeObject(s);
                var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                var request = cliente.PostAsync(adress, content).Result;
                if (request.IsSuccessStatusCode)
                {
                    var aux = request.Content.ReadAsStringAsync().Result;
                    Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                    return Receiver;
                }
                Receiver.Erro = $"Não foi possivel localizar o instagram com o nome '{Insta.Username}'.";
                Receiver.Status = 0;
                return Receiver;
            }
            catch (Exception err)
            {
                Receiver.Status = 0;
                Receiver.Erro = $"Erro ao realizar a requisição: '{err.Message}'.";
                return Receiver;
            }
        }

        /// <summary>
        /// Remover Incorrect da conta do instagram
        /// </summary>
        /// <param name="Insta">Conta para remover</param>
        /// <returns>Retorno da API</returns>
        static public Receiver RemoveIncorrect (Instagram Insta)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel localizar o instagram com o nome '{Insta.Username}'.",
                Contas = null
            };
            try
            {
                using var cliente = new HttpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                string adress = BaseURL + "removeincorrect";
                Sender s = new Sender
                {
                    Token = Variaveis.Token,
                    Username = Insta.Username
                };
                var serializedSender = JsonConvert.SerializeObject(s);
                var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                var request = cliente.PostAsync(adress, content).Result;
                if (request.IsSuccessStatusCode)
                {
                    var aux = request.Content.ReadAsStringAsync().Result;
                    Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                    return Receiver;
                }
                Receiver.Erro = $"Não foi possivel localizar o instagram com o nome '{Insta.Username}'.";
                Receiver.Status = 0;
                return Receiver;
            }
            catch (Exception err)
            {
                Receiver.Status = 0;
                Receiver.Erro = $"Erro ao realizar a requisição: '{err.Message}'.";
                return Receiver;
            }
        }

        /// <summary>
        /// Adicionar tarefa de curtir na conta do instagram
        /// </summary>
        /// <param name="Insta">Conta para adicionar</param>
        /// <returns>Retorno da API</returns>
        static public Receiver AddCurtir (Instagram Insta)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel localizar o instagram com o nome '{Insta.Username}'.",
                Contas = null
            };
            try
            {
                using var cliente = new HttpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                string adress = BaseURL + "addcurtir";
                Sender s = new Sender
                {
                    Token = Variaveis.Token,
                    Username = Insta.Username
                };
                var serializedSender = JsonConvert.SerializeObject(s);
                var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                var request = cliente.PostAsync(adress, content).Result;
                if (request.IsSuccessStatusCode)
                {
                    var aux = request.Content.ReadAsStringAsync().Result;
                    Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                    return Receiver;
                }
                Receiver.Erro = $"Não foi possivel localizar o instagram com o nome '{Insta.Username}'.";
                Receiver.Status = 0;
                return Receiver;
            }
            catch (Exception err)
            {
                Receiver.Status = 0;
                Receiver.Erro = $"Erro ao realizar a requisição: '{err.Message}'.";
                return Receiver;
            }
        }

        /// <summary>
        /// Adicionar tarefa de seguir na conta do instagram
        /// </summary>
        /// <param name="Insta">Conta para adicionar</param>
        /// <returns>Retorno da API</returns>
        static public Receiver AddSeguir (Instagram Insta)
        {
            var Receiver = new Receiver
            {
                Status = 0,
                Erro = $"Não foi possivel localizar o instagram com o nome '{Insta.Username}'.",
                Contas = null
            };
            try
            {
                using var cliente = new HttpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                string adress = BaseURL + "addseguir";
                Sender s = new Sender
                {
                    Token = Variaveis.Token,
                    Username = Insta.Username
                };
                var serializedSender = JsonConvert.SerializeObject(s);
                var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                var request = cliente.PostAsync(adress, content).Result;
                if (request.IsSuccessStatusCode)
                {
                    var aux = request.Content.ReadAsStringAsync().Result;
                    Receiver = JsonConvert.DeserializeObject<Receiver>(aux);
                    return Receiver;
                }
                Receiver.Erro = $"Não foi possivel localizar o instagram com o nome '{Insta.Username}'.";
                Receiver.Status = 0;
                return Receiver;
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

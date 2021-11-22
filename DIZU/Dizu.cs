using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DIZUCMD.DIZU
{
    class Dizu
    {
        private string Token { get; set; }
        private string UserAgent = "Arka Bot Dizu";
        private HttpClient Client { get; set; }
        private HttpClientHandler Handler { get; set; }
        private CookieContainer Cookies { get; set; }
        private Uri BasicUrl { get; set; }
        public List<ContasDizu> Contas { get; set; }
        public bool LoadComplet { get; set; }
        public Dizu (string UserToken)
        {
            this.Contas = new List<ContasDizu>();
            this.BasicUrl = new Uri("https://painel.dizu.com.br/");
            this.Cookies = new CookieContainer();
            this.Token = UserToken;
            this.Handler = new HttpClientHandler
            {
                UseDefaultCredentials = false,
                UseCookies = true,
                CookieContainer = this.Cookies
            };
            this.Client = new HttpClient(this.Handler);
            this.Client.DefaultRequestHeaders.Add("UserAgent", this.UserAgent);
            var request = new HttpRequestMessage(HttpMethod.Get, this.BasicUrl + $"perfis?json=true&crsftoken={this.Token}");
            var res = this.Client.SendAsync(request).Result;
            if (res.IsSuccessStatusCode)
            {
                var serializado = res.Content.ReadAsStringAsync().Result;
                try
                {
                    dynamic contas = JsonConvert.DeserializeObject(serializado);
                    if (contas.Count > 0)
                    {
                        for (int i = 0; i < contas.Count; i++)
                        {
                            if (contas[i].status == 1)
                            {
                                ContasDizu aux = new ContasDizu
                                {
                                    PKID = contas[i].pkid.ToString(),
                                    Username = contas[i].perfil
                                };
                                this.Contas.Add(aux);
                            }
                            if (this.Contas.Count > 0)
                            {
                                this.LoadComplet = true;
                            }
                            else
                            {
                                this.LoadComplet = false;
                            }
                        }
                    }
                    else
                    {
                        this.LoadComplet = false;
                    }
                } catch
                {
                    this.LoadComplet = false;
                }
            } else
            {
                this.LoadComplet = false;
            }
        }

        /// <summary>
        /// Buscar tarefa pelo PKID da conta
        /// </summary>
        /// <param name="PKID">PKID da conta</param>
        /// <returns>-1 = Erro / 1 = Seguir / 2 = Curtir / 3 = Não localizada / 4 = Não cadastrada</returns>
        public async Task<DizuRetorno> GetTask (string PKID)
        {
            DizuRetorno Ret = new DizuRetorno
            {
                Json = null,
                Response = "",
                Status = 0
            };
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, this.BasicUrl + $"listar_pedido?crsftoken={this.Token}&json=true&conta_id={PKID}");
                var response = await this.Client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var serializado = await response.Content.ReadAsStringAsync();
                    if (serializado.IndexOf("\"id\"") > -1)
                    {
                        dynamic json = JsonConvert.DeserializeObject(serializado);
                        try
                        {
                            string acao = json.acao;
                            if (String.IsNullOrEmpty(acao))
                            {
                                acao = json.link.ToString().IndexOf("/p/") > -1 ? "Curtir" : "Seguir";
                            }
                            if (acao.IndexOf("Seguir") > -1)
                            {
                                Ret.Status = 1;
                                Ret.Response = "Tipo Seguir";
                                Ret.Json = json;
                                return Ret;
                            }
                            else
                            {
                                if (acao.ToString().IndexOf("Curtir") > -1)
                                {
                                    Ret.Status = 2;
                                    Ret.Response = "Tipo Curtir";
                                    Ret.Json = json;
                                    return Ret;
                                }
                                else
                                {
                                    Ret.Status = 4;
                                    Ret.Response = "Tipo de tarefa ainda não configurado";
                                    return Ret;
                                }
                            }
                        }
                        catch
                        {
                            Ret.Status = 3;
                            Ret.Response = "Tarefa não localizada";
                            return Ret;
                        }
                    } else
                    {
                        Ret.Response = "Tarefa não encontrada";
                        Ret.Status = 0;
                        return Ret;
                    }
                } else
                {
                    Ret.Response = "Erro ao buscar a tarefa";
                    Ret.Status = -1;
                    return Ret;
                }
            } catch (Exception err)
            {
                Ret.Status = -1;
                Ret.Response = err.Message;
            }
            return Ret;
        }

        /// <summary>
        /// Puchar dados do perfil da Dizu
        /// </summary>
        /// <returns>Retorno 0 caso token errado/ 1 Certo / -1 Erro</returns>
        public async Task<DizuRetorno> GetSaldo ()
        {
            DizuRetorno Ret = new DizuRetorno
            {
                Json = null,
                Response = "",
                Status = 0
            };
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, this.BasicUrl + $"?json=true&crsftoken={this.Token}");
                var response = await this.Client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var serializado = await response.Content.ReadAsStringAsync();
                    if (serializado.IndexOf("\"id\":") > -1)
                    {
                        dynamic json = JsonConvert.DeserializeObject(serializado);
                        Ret.Status = 1;
                        Ret.Response = "Dados puchados";
                        Ret.Json = json;
                        return Ret;
                    } else
                    {
                        Ret.Response = "Token invalido";
                        Ret.Status = 0;
                        return Ret;
                    }
                }
                else
                {
                    Ret.Response = "Erro ao buscar o perfil";
                    Ret.Status = -1;
                    return Ret;
                }
            }
            catch (Exception err)
            {
                Ret.Status = -1;
                Ret.Response = err.Message;
            }
            return Ret;
        }
    
        /// <summary>
        /// Confirmar tarefa
        /// </summary>
        /// <param name="pkid">PKID da conta</param>
        /// <param name="taskid">ID da tarefa</param>
        /// <returns>Retorna: 1 confirmada / -1 erro/ 0 não confirmada</returns>
        public async Task<DizuRetorno> ConfirmTask (string pkid, string taskid)
        {
            DizuRetorno Ret = new DizuRetorno
            {
                Json = null,
                Response = "",
                Status = 0
            };
            try
            {
                SendConfirmDizu send = new SendConfirmDizu
                {
                    PKID = pkid,
                    Realizado = 1,
                    TaskID = taskid
                };
                var seralized = JsonConvert.SerializeObject(send);
                var content = new StringContent(seralized, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Post, this.BasicUrl + $"confirmar_pedido/?crsftoken={this.Token}") { Content = content };
                var response = await this.Client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var serializado = await response.Content.ReadAsStringAsync();
                    if (serializado.IndexOf("\"pedido_success\"") > -1)
                    {
                        dynamic json = JsonConvert.DeserializeObject(serializado);
                        Ret.Status = 1;
                        Ret.Response = "Tarefa confirmada";
                        Ret.Json = json;
                        return Ret;
                    }
                    else
                    {
                        Ret.Response = "Erro ao confirmar a tarefa";
                        Ret.Status = 0;
                        return Ret;
                    }
                }
                else
                {
                    Ret.Response = "Erro ao confirmar a tarefa";
                    Ret.Status = -1;
                    return Ret;
                }
            }
            catch (Exception err)
            {
                Ret.Status = -1;
                Ret.Response = err.Message;
            }
            return Ret;
        }

        /// <summary>
        /// Pular tarefa
        /// </summary>
        /// <param name="pkid">PKID da conta</param>
        /// <param name="taskid">ID da tarefa</param>
        /// <returns>Retorna: 1 pulada / -1 erro/ 0 não pulada</returns>
        public async Task<DizuRetorno> PularTask(string pkid, string taskid)
        {
            DizuRetorno Ret = new DizuRetorno
            {
                Json = null,
                Response = "",
                Status = 0
            };
            try
            {
                SendConfirmDizu send = new SendConfirmDizu
                {
                    PKID = pkid,
                    Realizado = 3,
                    TaskID = taskid
                };
                var seralized = JsonConvert.SerializeObject(send);
                var content = new StringContent(seralized, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Post, this.BasicUrl + $"confirmar_pedido/?crsftoken={this.Token}") { Content = content };
                var response = await this.Client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var serializado = await response.Content.ReadAsStringAsync();
                    if (serializado.IndexOf("\"pedido_success\"") > -1)
                    {
                        dynamic json = JsonConvert.DeserializeObject(serializado);
                        Ret.Status = 1;
                        Ret.Response = "Tarefa pulada";
                        Ret.Json = json;
                        return Ret;
                    }
                    else
                    {
                        Ret.Response = "Erro ao pular a tarefa";
                        Ret.Status = 0;
                        return Ret;
                    }
                }
                else
                {
                    Ret.Response = "Erro ao pular a tarefa";
                    Ret.Status = -1;
                    return Ret;
                }
            }
            catch (Exception err)
            {
                Ret.Status = -1;
                Ret.Response = err.Message;
            }
            return Ret;
        }

        /// <summary>
        /// Verificar se a conta está cadastrada na Dizu
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>True or False</returns>
        public bool CheckAccount(string username)
        {
            string a = username.ToLower();
            if (this.Contas.Count > 0)
            {
                if (this.Contas.Exists(i => i.Username.ToLower() == a))
                {
                    return true;
                } else
                {
                    return false;
                }
            } else
            {
                return false;
            }
        }

        /// <summary>
        /// Pegar a PKID da conta
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>PKID da conta</returns>
        public string GetPKID(string username)
        {
            var a = username.ToLower();
            if (this.Contas.Count > 0)
            {
                if (this.Contas.Exists(i => i.Username.ToLower() == a))
                {
                    var index = this.Contas.Find(i => i.Username.ToLower() == a);
                    return index.PKID;
                } else
                {
                    return "";
                }
            } else
            {
                return "";
            }
        }
    
        /// <summary>
        /// Cadastrar a conta do instagra na dizu, IMPORTANT: Validar se possui 30 seguidor, foto de perfil e se te mais de 7 publicações
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pkid"></param>
        /// <returns></returns>
        public async Task<bool> CadastrarConta(string username, dynamic pkid)
        {
            try
            {
                SendCadInstaDizu send = new SendCadInstaDizu
                {
                    PKID = pkid,
                    Username = username.StartsWith("@") ? username : "@" + username
                };
                var seralized = JsonConvert.SerializeObject(send);
                var content = new StringContent(seralized, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Post, this.BasicUrl + $"cadastrar_conta?crsftoken={this.Token}") { Content = content };
                var response = await this.Client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var serializado = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject(serializado);
                    try
                    {
                        if (json.success)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    } catch
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    
    }

    class SendConfirmDizu
    {
        [JsonProperty("conta_id")]
        public string PKID { get; set; }
        [JsonProperty("tarefa_id")]
        public string TaskID { get; set; }
        [JsonProperty("realizado")]
        public int Realizado { get; set; }
    }

    class SendCadInstaDizu
    {
        [JsonProperty("conta")]
        public string Username { get; set; }
        [JsonProperty("site")]
        public int Site = 1;
        [JsonProperty("user_pkid")]
        public dynamic PKID { get; set; }
    }

    class DizuRetorno
    {
        public int Status { get; set; }
        public string Response { get; set; }
        public dynamic Json { get; set; }
    }

    class ContasDizu
    {
        public string Username { get; set; }
        public string PKID { get; set; }
    }
}

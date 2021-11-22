using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DIZUCMD.WebInstagram
{
    public class InstagramWeb
    {
        public CookieContainer Cookies { get; set; }
        public Dictionary<string, string> ListCookie { get; set; }
        public Uri BasicUrl { get; set; }
        public string CSRFTOKEN { get; set; }
        public HttpClientHandler Handler { get; set; }
        public HttpClient Client { get; set; }
        public long UserID { get; set; }
        public string UserAgent { get; set; }
        public UserDate User { get; set; }
        public dynamic _shareddata { get; set; }
        public string X_IG_WWW_CLAIM { get; set; }
        public string Challenge_URL { get; set; }

        public InstagramWeb(UserDate User, bool PrivateUserAgent = false, string TypeUserAgentString = "", string PrivateUserAgentString = "")
        {
            try
            {
                this.Challenge_URL = "challenge/";
                this.User = User;
                this.BasicUrl = new Uri("https://www.instagram.com");
                this.Cookies = new CookieContainer();
                this.Cookies.Add(this.BasicUrl, new Cookie("ig_cb", "1"));
                this.X_IG_WWW_CLAIM = "";
                this.Handler = new HttpClientHandler
                {
                    CookieContainer = this.Cookies,
                    UseCookies = true,
                    UseDefaultCredentials = false
                };
                this.Client = new HttpClient(this.Handler)
                {
                    BaseAddress = this.BasicUrl,
                    Timeout = TimeSpan.FromSeconds(35)
                };
                this.Client.DefaultRequestHeaders.Add("Accept-Language", "en-US");
                this.Client.DefaultRequestHeaders.Add("X-Instagram-AJAX", "1");
                if (PrivateUserAgent)
                    this.UserAgent = TypeUserAgentString == "mobile" ? GetUserAgent("mobile") : TypeUserAgentString == "navegador" ? GetUserAgent("navegador") : TypeUserAgentString == "txt" ? PrivateUserAgentString : GetUserAgent(User.Username);
                else
                    this.UserAgent = GetUserAgent(User.Username) ?? "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.8; rv:21.0) Gecko/20100101 Firefox/21.0";
                if (String.IsNullOrEmpty(this.UserAgent))
                    this.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.8; rv:21.0) Gecko/20100101 Firefox/21.0";
                this.Client.DefaultRequestHeaders.Add("User-Agent", this.UserAgent);
                this.Client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                this.Client.DefaultRequestHeaders.Add("Referer", this.BasicUrl.ToString());
                HttpResponseMessage response = this.Client.GetAsync(this.BasicUrl).Result;
                var aux = this.Handler.CookieContainer.GetCookies(this.BasicUrl);
                for (int i = 0; i < aux.Count; i++)
                {
                    if (aux[i].Name == "csrftoken")
                    {
                        this.CSRFTOKEN = aux[i].Value;
                    }
                }
                this.Client.DefaultRequestHeaders.Add("X-CSRFToken", this.CSRFTOKEN);
            }
            catch
            {
            }
        }

        private string GetUserAgent(string username)
        {
            using (var cliente = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                string adress = "https://arkabot.com.br/api/useragent";
                SendUsername sender = new SendUsername { Username = username };
                var serializedSender = JsonConvert.SerializeObject(sender);
                var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                var request = cliente.PostAsync(adress, content).Result;
                if (request.IsSuccessStatusCode)
                {
                    var aux = request.Content.ReadAsStringAsync().Result;
                    UserAgentRetorno aux2 = JsonConvert.DeserializeObject<UserAgentRetorno>(aux);
                    return String.IsNullOrEmpty(aux2.Mensagem) ? null : aux2.Mensagem;
                }
                return null;
            }
        }

        public string GetGis(string path)
        {
            using (var cliente = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                string adress = $"https://arkabot.com.br/api/getgis?path={path}";
                var serializedSender = JsonConvert.SerializeObject(this._shareddata);
                var content = new StringContent(serializedSender, Encoding.UTF8, "application/json");
                var request = cliente.PostAsync(adress, content).Result;
                if (request.IsSuccessStatusCode)
                {
                    var aux = request.Content.ReadAsStringAsync().Result;
                    return aux;
                }
                return "";
            }
        }
    }

    public class InstaResponse
    {
        public int Satus { get; set; }
        public string Response { get; set; }
    }

    public class InstaResponseJson
    {
        public int Status { get; set; }
        public string Response { get; set; }
        public dynamic Json { get; set; }
    }

    public class FriendshipRelation
    {
        public bool Is_Complet { get; set; }
        public bool Is_Private { get; set; }
        public bool Is_Followed { get; set; }
        public bool Is_Following { get; set; }
        public string PK { get; set; }
        public string Response { get; set; }
        public int Status { get; set; }
    }

    public class MediaRelation
    {
        public bool Is_Complet { get; set; }
        public bool Is_Liked { get; set; }
        public string MediaID { get; set; }
        public string MediaShortcode { get; set; }
        public string OwnerUsername { get; set; }
        public string OwnerPK { get; set; }
        public bool Is_FollowingOwner { get; set; }
        public bool Is_PrivateOwner { get; set; }
        public string Response { get; set; }
        public int Status { get; set; }
    }

    public static class ExtendInstagram
    {
        public static InstaResponse DoLogin(this InstagramWeb Insta, int atual = 1)
        {
            InstaResponse Ret = new InstaResponse
            {
                Response = "",
                Satus = 0
            };
            try
            {
                HttpResponseMessage response = Insta.Client.GetAsync(Insta.BasicUrl).Result;
                var sharedDate = WebHelper.GetJson(response.Content.ReadAsStringAsync().Result);
                var csrf_token = Insta.CSRFTOKEN;
                if (sharedDate != null)
                {
                    var index = sharedDate.IndexOf("csrf_token\":\"");
                    if (index > -1)
                    {
                        csrf_token = sharedDate.Substring(index + 13, 32);
                    }
                }
                Insta.CSRFTOKEN = csrf_token;
                Insta.Client.DefaultRequestHeaders.Remove("X-CSRFToken");
                Insta.Client.DefaultRequestHeaders.Add("X-CSRFToken", csrf_token);
                Insta.Client.DefaultRequestHeaders.Remove("X-IG-WWW-Claim");
                Insta.Client.DefaultRequestHeaders.Add("X-IG-WWW-Claim", "0");
                Insta.Client.DefaultRequestHeaders.Remove("Sec-Fetch-Mode");
                Insta.Client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
                Insta.Client.DefaultRequestHeaders.Remove("Referer");
                Insta.Client.DefaultRequestHeaders.Add("Referer", "https://www.instagram.com/");
                Insta.Client.DefaultRequestHeaders.Remove("X-ASBD-ID");
                Insta.Client.DefaultRequestHeaders.Add("X-ASBD-ID", "198387");
                Insta.Client.DefaultRequestHeaders.Remove("X-IG-App-ID");
                Insta.Client.DefaultRequestHeaders.Add("X-IG-App-ID", "936619743392459");
                var username = Insta.User.Username;
                var enc_password = GetEncryptedPassword(Insta.User.Password);
                var dict = new Dictionary<string, string>();
                dict.Add("username", username);
                dict.Add("enc_password", enc_password);
                var req = new HttpRequestMessage(HttpMethod.Post, Insta.BasicUrl + "/accounts/login/ajax/") { Content = new FormUrlEncodedContent(dict) };
                var res = Insta.Client.SendAsync(req).Result;
                if (res.IsSuccessStatusCode)
                {
                    IEnumerable<string> headerValues = res.Headers.GetValues("x-ig-set-www-claim");
                    Insta.X_IG_WWW_CLAIM = headerValues.FirstOrDefault();
                    Insta.Client.DefaultRequestHeaders.Remove("X-IG-WWW-Claim");
                    Insta.Client.DefaultRequestHeaders.Add("X-IG-WWW-Claim", Insta.X_IG_WWW_CLAIM);
                    var aux = Insta.Handler.CookieContainer.GetCookies(Insta.BasicUrl);
                    if (Insta.ListCookie == null)
                        Insta.ListCookie = new Dictionary<string, string>();
                    for (int i = 0; i < aux.Count; i++)
                    {
                        Insta.ListCookie.Add(aux[i].Name, aux[i].Value);
                        if (aux[i].Name == "csrftoken")
                        {
                            Insta.CSRFTOKEN = aux[i].Value;
                        }
                    }
                    csrf_token = Insta.CSRFTOKEN;
                    Insta.Client.DefaultRequestHeaders.Remove("X-CSRFToken");
                    Insta.Client.DefaultRequestHeaders.Add("X-CSRFToken", csrf_token);
                    if (res.Content.ReadAsStringAsync().Result.IndexOf("\"authenticated\":true") > -1)
                    {
                        Ret.Satus = 1;
                        Ret.Response = "Login realizado com sucesso";
                        Insta.Client.DefaultRequestHeaders.Add("X-IG-WWW-Claim", Insta.X_IG_WWW_CLAIM);
                        var aux3 = res.Content.ReadAsStringAsync().Result.Split('"');
                        Insta.UserID = long.Parse(aux3[5]);
                        var reqq = new HttpRequestMessage(HttpMethod.Get, Insta.BasicUrl);
                        var init = Insta.Client.SendAsync(reqq).Result;
                        if (init.IsSuccessStatusCode)
                        {
                            if (WebHelper.CanReadJson(init.Content.ReadAsStringAsync().Result))
                            {
                                var json = WebHelper.GetJson(init.Content.ReadAsStringAsync().Result);
                                Insta._shareddata = JsonConvert.DeserializeObject(json);
                                csrf_token = Insta._shareddata.config.csrf_token;
                                Insta.Client.DefaultRequestHeaders.Remove("X-CSRFToken");
                                Insta.Client.DefaultRequestHeaders.Add("X-CSRFToken", csrf_token);
                                Insta.CSRFTOKEN = csrf_token;
                            }
                        }
                        return Ret;
                    }
                    Ret.Satus = 0;
                    Ret.Response = "Não foi possivel realizar o login";
                }
                else
                {
                    if (res.Content.ReadAsStringAsync().Result.IndexOf("\"checkpoint_required\"") > -1)
                    {
                        var serializado = res.Content.ReadAsStringAsync().Result;
                        dynamic dataJson = JsonConvert.DeserializeObject(serializado);
                        Insta.Challenge_URL = dataJson.checkpoint_url;
                        Ret.Satus = -2;
                        Ret.Response = "Conta com bloqueio ao logar";
                        return Ret;
                    }
                    else
                    {
                        return Insta.DoLogin(atual++);
                    }
                }
            }
            catch
            {
            }
            return Ret;
        }

        public async static Task<InstaResponseJson> GetUserProfileByUsernameAsync(this InstagramWeb Insta, string username, int atual = 0)
        {
            InstaResponseJson Ret = new InstaResponseJson
            {
                Response = "",
                Status = 0,
                Json = null
            };
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Get, Insta.BasicUrl + $"{username.ToLower()}/?__a=1");
                var perfil = await Insta.Client.SendAsync(req);
                if (perfil.IsSuccessStatusCode)
                {
                    if ((await perfil.Content.ReadAsStringAsync()).IndexOf("\"biography\"") > -1)
                    {
                        var serializado = perfil.Content.ReadAsStringAsync().Result;
                        dynamic dataJson = JsonConvert.DeserializeObject(serializado);
                        Ret.Response = dataJson.graphql.user.id;
                        Ret.Status = 1;
                        Ret.Json = dataJson.graphql.user;
                    }
                    else
                    {
                        Ret.Response = await perfil.Content.ReadAsStringAsync();
                        Ret.Status = 0;
                    }
                }
                else
                {
                    var aux = await perfil.Content.ReadAsStringAsync();
                    if (aux.IndexOf("Go back to Instagram.") > -1)
                    {
                        await Task.Delay(1246);
                        if (atual < 3)
                            return await Insta.GetUserProfileByUsernameAsync(username, atual++);
                        Ret.Status = 0;
                        Ret.Response = "Usuario não encontrado";
                        return Ret;
                    }
                    else
                    {
                        if (aux.IndexOf("\"checkpoint_required\"") > -1)
                        {
                            Ret.Status = -2;
                            Ret.Response = aux;
                            dynamic challenge = JsonConvert.DeserializeObject(aux);
                            Insta.Challenge_URL = challenge.checkpoint_url;
                            return Ret;
                        }
                        else
                        {
                            if (aux.IndexOf("\"feedback_required\"") > -1)
                            {
                                Ret.Status = -3;
                                Ret.Response = aux;
                                return Ret;
                            }
                            else
                            {
                                Ret.Status = -4;
                                Ret.Response = aux;
                                return Ret;
                            }
                        }
                    }
                }
            }
            catch
            { }
            return Ret;
        }

        public async static Task<InstaResponseJson> GetChallengeRequestByChallengeUrlAsync(this InstagramWeb Insta, int atual = 1)
        {
            InstaResponseJson Ret = new InstaResponseJson
            {
                Response = "Erro ao buscar challenge",
                Status = -1,
                Json = null
            };
            try
            {
                string url = "";
                if (Insta.Challenge_URL.IndexOf("instagram.com") > -1)
                {
                    url = $"{Insta.Challenge_URL}";
                }
                else
                {
                    if (Insta.Challenge_URL.StartsWith("/"))
                        url = $"https://instagram.com{Insta.Challenge_URL}";
                    else
                        url = $"https://instagram.com/{Insta.Challenge_URL}";
                }
                if (Insta.Challenge_URL.IndexOf("?") > -1)
                    url += "&__a=1";
                else
                    url += "?__a=1";
                var req = new HttpRequestMessage(HttpMethod.Get, url);
                var result = await Insta.Client.SendAsync(req);
                if (result.IsSuccessStatusCode)
                {
                    var serializado = await result.Content.ReadAsStringAsync();
                    dynamic dataJson = JsonConvert.DeserializeObject(serializado);
                    try
                    {
                        Ret.Response = dataJson.challengeType;
                        Ret.Status = 1;
                        Ret.Json = dataJson;
                        return Ret;
                    }
                    catch
                    {
                        Ret.Response = "Erro ao puxar o challenge: " + serializado;
                        Ret.Status = 2;
                        return Ret;
                    }
                }
                else
                {
                    var aux = await result.Content.ReadAsStringAsync();
                    if (aux.IndexOf("Go back to Instagram.") > -1 && atual < 3)
                    {
                        await Task.Delay(1246);
                        return await Insta.GetChallengeRequestByChallengeUrlAsync(atual++);
                    }
                    else
                    { }
                }
                return Ret;
            }
            catch (Exception err)
            {
                Ret.Response = err.Message;
                Ret.Status = -1;
            }
            return Ret;
        }

        public async static Task<InstaResponseJson> GetChallengeRequestAsync(this InstagramWeb Insta, int atual = 1)
        {
            InstaResponseJson Ret = new InstaResponseJson
            {
                Response = "Erro ao buscar Challenge",
                Status = -1,
                Json = null
            };
            try
            {
                string url = "";
                if (Insta.Challenge_URL.IndexOf("instagram.com") > -1)
                {
                    url = $"{Insta.Challenge_URL}";
                }
                else
                {
                    if (Insta.Challenge_URL.StartsWith("/"))
                        url = $"https://instagram.com{Insta.Challenge_URL}";
                    else
                        url = $"https://instagram.com/{Insta.Challenge_URL}";
                }
                if (Insta.Challenge_URL.IndexOf("?") > -1)
                    url += "&__a=1";
                else
                    url += "?__a=1";
                var req = new HttpRequestMessage(HttpMethod.Get, url);
                var result = await Insta.Client.SendAsync(req);
                if (result.IsSuccessStatusCode)
                {
                    var serializado = await result.Content.ReadAsStringAsync();
                    dynamic dataJson = JsonConvert.DeserializeObject(serializado);
                    try
                    {
                        Ret.Response = dataJson.challengeType;
                        Ret.Status = 1;
                        Ret.Json = dataJson;
                        return Ret;
                    }
                    catch
                    {
                        Ret.Response = "Erro ao puxar o challenge: " + serializado;
                        Ret.Status = 2;
                        return Ret;
                    }
                }
                else
                {
                    var aux = await result.Content.ReadAsStringAsync();
                    if (aux.IndexOf("Go back to Instagram.") > -1 && atual < 3)
                    {
                        await Task.Delay(1246);
                        return await Insta.GetChallengeRequestAsync(atual++);
                    }
                    else
                    { }
                }
                return Ret;
            }
            catch (Exception err)
            {
                Ret.Response = err.Message;
                Ret.Status = -1;
            }
            return Ret;
        }

        public async static Task<InstaResponse> ReplyChallengeByChoiceAsync(this InstagramWeb Insta, string Choice, int atual = 1)
        {
            InstaResponse Ret = new InstaResponse
            {
                Response = "Erro ao responder ao challenge",
                Satus = 2
            };
            try
            {
                var dict = new Dictionary<string, string>();
                dict.Add("choice", Choice);
                var req = new HttpRequestMessage(HttpMethod.Post, Insta.BasicUrl + $"challenge/") { Content = new FormUrlEncodedContent(dict) };
                var result = await Insta.Client.SendAsync(req);
                if (result.IsSuccessStatusCode)
                {
                    var serializado = await result.Content.ReadAsStringAsync();
                    dynamic dataJson = JsonConvert.DeserializeObject(serializado);
                    try
                    {
                        if (dataJson.status == "ok")
                        {
                            Ret.Response = "Sucesso";
                            Ret.Satus = 1;
                            return Ret;
                        }
                        else
                        {
                            Ret.Response = "Erro ao responder o challenge: " + serializado;
                            Ret.Satus = 2;
                            return Ret;
                        }
                    }
                    catch
                    {
                        Ret.Response = "Erro ao responder o challenge: " + serializado;
                        Ret.Satus = 2;
                        return Ret;
                    }
                }
                else
                {
                    var aux = await result.Content.ReadAsStringAsync();
                    if (aux.IndexOf("Go back to Instagram.") > -1)
                    {
                        Ret.Response = "Erro ao responder o challenge";
                        Ret.Satus = 2;
                        return Ret;
                    }
                    else
                    { }
                }
            }
            catch (Exception err)
            {
                Ret.Response = err.Message;
                Ret.Satus = -1;
            }
            return Ret;
        }

        public async static Task<FriendshipRelation> GetFriendshipRelationByUsernameAsync(this InstagramWeb Insta, string username, int atual = 0)
        {
            FriendshipRelation Ret = new FriendshipRelation
            {
                Is_Complet = false,
                Is_Followed = false,
                Is_Following = false,
                Is_Private = false,
                PK = "",
                Response = "",
                Status = 0
            };
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Get, Insta.BasicUrl + $"{username.ToLower()}/?__a=1");
                var perfil = await Insta.Client.SendAsync(req);
                if (perfil.IsSuccessStatusCode)
                {
                    if ((await perfil.Content.ReadAsStringAsync()).IndexOf("\"biography\"") > -1)
                    {
                        var serializado = await perfil.Content.ReadAsStringAsync();
                        dynamic dataJson = JsonConvert.DeserializeObject(serializado);
                        var user = dataJson.graphql.user;
                        Ret.Is_Following = user.followed_by_viewer;
                        Ret.Is_Followed = user.follows_viewer;
                        Ret.Is_Private = user.is_private;
                        Ret.PK = user.id;
                        Ret.Is_Complet = true;
                        Ret.Status = 1;
                        Ret.Response = "Sucesso";
                        return Ret;
                    }
                    else
                    {
                        var serializado = await perfil.Content.ReadAsStringAsync();
                        Ret.Is_Complet = false;
                        Ret.Response = serializado;
                        Ret.Status = -4;
                        return Ret;
                    }
                }
                else
                {
                    var aux = await perfil.Content.ReadAsStringAsync();
                    if (aux.IndexOf("Go back to Instagram.") > -1)
                    {
                        await Task.Delay(1246);
                        if (atual < 3)
                            return await Insta.GetFriendshipRelationByUsernameAsync(username, atual++);
                        Ret.Is_Complet = false;
                        Ret.Response = "Perfil não encontrado";
                        Ret.Status = 0;
                        return Ret;
                    }
                    else
                    {
                        if (aux.IndexOf("\"checkpoint_required\"") > -1)
                        {
                            Ret.Is_Complet = false;
                            Ret.Response = aux;
                            Ret.Status = -2;
                            dynamic challenge = JsonConvert.DeserializeObject(aux);
                            Insta.Challenge_URL = challenge.checkpoint_url;
                            return Ret;
                        }
                        else
                        {
                            if (aux.IndexOf("\"feedback_required\"") > -1)
                            {
                                Ret.Is_Complet = false;
                                Ret.Response = aux;
                                Ret.Status = -3;
                                return Ret;
                            }
                            else
                            {
                                Ret.Is_Complet = false;
                                Ret.Response = aux;
                                Ret.Status = -4;
                                return Ret;
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Ret.Response = err.Message;
                Ret.Is_Complet = false;
                Ret.Status = -4;
            }
            return Ret;
        }

        public async static Task<InstaResponse> GetUserBySearchBarAsync(this InstagramWeb Insta, string username, int atual = 0)
        {
            // https://www.instagram.com/web/search/topsearch/?context=blended&query={username}&include_reel=false
            InstaResponse Ret = new InstaResponse
            {
                Response = "",
                Satus = 0
            };
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Get, Insta.BasicUrl + $"/web/search/topsearch/?context=blended&query={username.ToLower()}&include_reel=false");
                var result = await Insta.Client.SendAsync(req);
                if (result.IsSuccessStatusCode)
                {
                    var serializado = await result.Content.ReadAsStringAsync();
                    dynamic userJson = JsonConvert.DeserializeObject(serializado);
                    try
                    {
                        for (int i = 0; i < userJson.users.Count; i++)
                        {
                            if (userJson.users[i].user.username == username.ToLower())
                            {
                                Ret.Response = userJson.users[0].user.pk;
                                Ret.Satus = 1;
                                return Ret;
                            }
                        }
                        Ret.Response = "Usuario não localizado";
                        Ret.Satus = 2;
                        return Ret;
                    }
                    catch
                    {
                        Ret.Response = "Usuario não localizado";
                        Ret.Satus = 2;
                        return Ret;
                    }
                }
                else
                {
                    var aux = await result.Content.ReadAsStringAsync();
                    if (aux.IndexOf("Go back to Instagram.") > -1 && atual < 4)
                    {
                        await Task.Delay(1246);
                        return await Insta.GetUserBySearchBarAsync(username, atual++);
                    }
                    else
                    {
                        if (aux.IndexOf("\"checkpoint_required\"") > -1)
                        {
                            Ret.Satus = -2;
                            Ret.Response = aux;
                            dynamic challenge = JsonConvert.DeserializeObject(aux);
                            Insta.Challenge_URL = challenge.checkpoint_url;
                            return Ret;
                        }
                        else
                        {
                            if (aux.IndexOf("\"feedback_required\"") > -1)
                            {
                                Ret.Satus = -3;
                                Ret.Response = aux;
                                return Ret;
                            }
                            else
                            {
                                Ret.Response = "Usuario não localizado";
                                Ret.Satus = 2;
                                return Ret;
                            }
                        }
                    }
                }
            }
            catch
            {
                Ret.Satus = -1;
                Ret.Response = "Erro na requisição";
                return Ret;
            }
        }

        public async static Task<InstaResponse> GetUserIdByUsernameAsync(this InstagramWeb Insta, string username, int atual = 0)
        {
            InstaResponse Ret = new InstaResponse
            {
                Response = "",
                Satus = 0
            };
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Get, Insta.BasicUrl + $"{username.ToLower()}/?__a=1");
                var perfil = await Insta.Client.SendAsync(req);
                if (perfil.IsSuccessStatusCode)
                {
                    if ((await perfil.Content.ReadAsStringAsync()).IndexOf("\"biography\"") > -1)
                    {
                        var serializado = perfil.Content.ReadAsStringAsync().Result;
                        dynamic dataJson = JsonConvert.DeserializeObject(serializado);
                        Ret.Response = dataJson.graphql.user.id;
                        Ret.Satus = 1;
                    }
                    else
                    {
                        Ret.Response = await perfil.Content.ReadAsStringAsync();
                        Ret.Satus = 0;
                    }
                }
                else
                {
                    var aux = await perfil.Content.ReadAsStringAsync();
                    if (aux.IndexOf("Go back to Instagram.") > -1)
                    {
                        await Task.Delay(1246);
                        if (atual < 3)
                            return await Insta.GetUserIdByUsernameAsync(username, atual++);
                        Ret.Satus = 0;
                        Ret.Response = "Usuario não encontrado";
                        return Ret;
                    }
                    else
                    {
                        if (aux.IndexOf("\"checkpoint_required\"") > -1)
                        {
                            Ret.Satus = -2;
                            Ret.Response = aux;
                            dynamic challenge = JsonConvert.DeserializeObject(aux);
                            Insta.Challenge_URL = challenge.checkpoint_url;
                            return Ret;
                        }
                        else
                        {
                            if (aux.IndexOf("\"feedback_required\"") > -1)
                            {
                                Ret.Satus = -3;
                                Ret.Response = aux;
                                return Ret;
                            }
                            else
                            {
                                Ret.Satus = -4;
                                Ret.Response = aux;
                                return Ret;
                            }
                        }
                    }
                }
            }
            catch
            { }
            return Ret;
        }

        public async static Task<InstaResponse> FollowUserByIdAsync(this InstagramWeb Insta, string PK, int atual = 1)
        {
            // https://www.instagram.com/web/friendships/45259518580/follow/
            InstaResponse Ret = new InstaResponse
            {
                Response = "",
                Satus = 0
            };
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Post, Insta.BasicUrl + $"web/friendships/{PK}/follow/");
                var result = await Insta.Client.SendAsync(req);
                if (result.IsSuccessStatusCode)
                {
                    var serializado = await result.Content.ReadAsStringAsync();
                    dynamic userJson = JsonConvert.DeserializeObject(serializado);
                    try
                    {
                        if (userJson.status == "ok")
                        {
                            Ret.Response = "Sucesso";
                            Ret.Satus = 1;
                            return Ret;
                        }
                        Ret.Response = serializado;
                        Ret.Satus = 2;
                        return Ret;
                    }
                    catch
                    {
                        Ret.Response = "Usuario não localizado";
                        Ret.Satus = 2;
                        return Ret;
                    }
                }
                else
                {
                    var aux = await result.Content.ReadAsStringAsync();
                    if (aux.IndexOf("Go back to Instagram.") > -1 && atual < 3)
                    {
                        await Task.Delay(1246);
                        return await Insta.FollowUserByIdAsync(PK, atual++);
                    }
                    else
                    {
                        if (aux.IndexOf("\"checkpoint_required\"") > -1)
                        {
                            Ret.Satus = -2;
                            Ret.Response = aux;
                            dynamic challenge = JsonConvert.DeserializeObject(aux);
                            Insta.Challenge_URL = challenge.checkpoint_url;
                            return Ret;
                        }
                        else
                        {
                            if (aux.IndexOf("\"feedback_required\"") > -1)
                            {
                                Ret.Satus = -3;
                                Ret.Response = aux;
                                return Ret;
                            }
                            else
                            {
                                Ret.Response = "Usuario não localizado";
                                Ret.Satus = 2;
                                return Ret;
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Ret.Satus = -4;
                Ret.Response = err.Message;
                return Ret;
            }
        }

        public async static Task<InstaResponse> UnfollowUserByIdAsync(this InstagramWeb Insta, string PK, int atual = 1)
        {
            // https://www.instagram.com/web/friendships/632713992/unfollow/
            InstaResponse Ret = new InstaResponse
            {
                Response = "",
                Satus = 0
            };
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Post, Insta.BasicUrl + $"web/friendships/{PK}/unfollow/");
                var result = await Insta.Client.SendAsync(req);
                if (result.IsSuccessStatusCode)
                {
                    var serializado = await result.Content.ReadAsStringAsync();
                    dynamic userJson = JsonConvert.DeserializeObject(serializado);
                    try
                    {
                        if (userJson.status == "ok")
                        {
                            Ret.Response = "Sucesso";
                            Ret.Satus = 1;
                            return Ret;
                        }
                        Ret.Response = serializado;
                        Ret.Satus = 2;
                        return Ret;
                    }
                    catch
                    {
                        Ret.Response = "Usuario não localizado";
                        Ret.Satus = 2;
                        return Ret;
                    }
                }
                else
                {
                    var aux = await result.Content.ReadAsStringAsync();
                    if (aux.IndexOf("Go back to Instagram.") > -1 && atual < 4)
                    {
                        await Task.Delay(1246);
                        return await Insta.UnfollowUserByIdAsync(PK, atual++);
                    }
                    else
                    {
                        if (aux.IndexOf("\"checkpoint_required\"") > -1)
                        {
                            Ret.Satus = -2;
                            Ret.Response = aux;
                            dynamic challenge = JsonConvert.DeserializeObject(aux);
                            Insta.Challenge_URL = challenge.checkpoint_url;
                            return Ret;
                        }
                        else
                        {
                            if (aux.IndexOf("\"feedback_required\"") > -1)
                            {
                                Ret.Satus = -3;
                                Ret.Response = aux;
                                return Ret;
                            }
                            else
                            {
                                Ret.Response = "Usuario não localizado";
                                Ret.Satus = 2;
                                return Ret;
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Ret.Satus = -4;
                Ret.Response = err.Message;
                return Ret;
            }
        }

        public async static Task<InstaResponseJson> GetSuspiciousLoginAsync(this InstagramWeb Insta, int atual = 1)
        {
            // https://www.instagram.com/session/login_activity/?__a=1
            InstaResponseJson Ret = new InstaResponseJson
            {
                Response = "",
                Status = 0,
                Json = null
            };
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Get, Insta.BasicUrl + "session/login_activity/?__a=1");
                var result = await Insta.Client.SendAsync(req);
                if (result.IsSuccessStatusCode)
                {
                    var serializado = await result.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(serializado);
                    try
                    {
                        if (data.data.suspicious_logins != null)
                        {
                            Ret.Response = "Sucesso";
                            Ret.Status = 1;
                            Ret.Json = data.data.suspicious_logins;
                            return Ret;
                        }
                        Ret.Response = serializado;
                        Ret.Status = 2;
                        return Ret;
                    }
                    catch
                    {
                        Ret.Response = "Não foi possivel pegar os dados de login";
                        Ret.Status = 2;
                        return Ret;
                    }
                }
                else
                {
                    var aux = await result.Content.ReadAsStringAsync();
                    if (aux.IndexOf("Go back to Instagram.") > -1 && atual < 4)
                    {
                        await Task.Delay(1246);
                        return await Insta.GetSuspiciousLoginAsync(atual++);
                    }
                    else
                    {
                        if (aux.IndexOf("\"checkpoint_required\"") > -1)
                        {
                            Ret.Status = -2;
                            Ret.Response = aux;
                            dynamic challenge = JsonConvert.DeserializeObject(aux);
                            Insta.Challenge_URL = challenge.checkpoint_url;
                            return Ret;
                        }
                        else
                        {
                            if (aux.IndexOf("\"feedback_required\"") > -1)
                            {
                                Ret.Status = -3;
                                Ret.Response = aux;
                                return Ret;
                            }
                            else
                            {
                                Ret.Response = "Não foi possivel pegar os dados de login";
                                Ret.Status = 2;
                                return Ret;
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Ret.Status = -4;
                Ret.Response = err.Message;
                return Ret;
            }
        }

        public async static Task<InstaResponseJson> GetMyProfileAsync(this InstagramWeb Insta, int atual = 1)
        {
            // https://www.instagram.com/accounts/edit/?__a=1
            InstaResponseJson Ret = new InstaResponseJson
            {
                Response = "",
                Status = 0,
                Json = null
            };
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Get, Insta.BasicUrl + "accounts/edit/?__a=1");
                var result = await Insta.Client.SendAsync(req);
                if (result.IsSuccessStatusCode)
                {
                    var serializado = await result.Content.ReadAsStringAsync();
                    dynamic userJson = JsonConvert.DeserializeObject(serializado);
                    try
                    {
                        if (userJson.form_data.username == Insta.User.Username.ToLower())
                        {
                            Ret.Response = "Sucesso";
                            Ret.Status = 1;
                            Ret.Json = userJson.form_data;
                            return Ret;
                        }
                        Ret.Response = serializado;
                        Ret.Status = 2;
                        return Ret;
                    }
                    catch
                    {
                        Ret.Response = "Erro ao pegar dados do perfil";
                        Ret.Status = 2;
                        return Ret;
                    }
                }
                else
                {
                    var aux = await result.Content.ReadAsStringAsync();
                    if (aux.IndexOf("Go back to Instagram.") > -1 && atual < 4)
                    {
                        await Task.Delay(1246);
                        return await Insta.GetMyProfileAsync(atual++);
                    }
                    else
                    {
                        if (aux.IndexOf("\"checkpoint_required\"") > -1)
                        {
                            Ret.Status = -2;
                            Ret.Response = aux;
                            dynamic challenge = JsonConvert.DeserializeObject(aux);
                            Insta.Challenge_URL = challenge.checkpoint_url;
                            return Ret;
                        }
                        else
                        {
                            if (aux.IndexOf("\"feedback_required\"") > -1)
                            {
                                Ret.Status = -3;
                                Ret.Response = aux;
                                return Ret;
                            }
                            else
                            {
                                Ret.Response = "Erro ao pegar dados do perfil";
                                Ret.Status = 2;
                                return Ret;
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Ret.Status = -1;
                Ret.Response = err.Message;
                return Ret;
            }
        }

        public async static Task<InstaResponse> UpdateProfileAsync(this InstagramWeb Insta, string name = "", string email = "", string phone = "", int gender = 2, string bio = "", string url = "", bool similar = false, int atual = 1)
        {
            //https://www.instagram.com/accounts/edit/
            InstaResponse Ret = new InstaResponse
            {
                Response = "",
                Satus = 0
            };
            var atual_profile = await Insta.GetMyProfileAsync();
            if (atual_profile.Status == 1)
            {
                try
                {
                    var dict = new Dictionary<string, string>();
                    dict.Add("first_name", name != "" ? name : atual_profile.Json.first_name);
                    dict.Add("email", email != "" ? email : atual_profile.Json.email);
                    dict.Add("username", Insta.User.Username.ToLower());
                    dict.Add("phone_number", phone != "" ? phone : atual_profile.Json.phone_number);
                    dict.Add("gender", gender.ToString());
                    dict.Add("biography", bio != "" ? bio : atual_profile.Json.biography);
                    dict.Add("external_url", url != "" ? url : atual_profile.Json.external_url);
                    dict.Add("chaining_enabled", similar.ToString());
                    var req = new HttpRequestMessage(HttpMethod.Post, Insta.BasicUrl + "accounts/edit/") { Content = new FormUrlEncodedContent(dict) };
                    var result = await Insta.Client.SendAsync(req);
                    if (result.IsSuccessStatusCode)
                    {
                        var serializado = await result.Content.ReadAsStringAsync();
                        dynamic dataJson = JsonConvert.DeserializeObject(serializado);
                        if (dataJson.status == "ok")
                        {
                            Ret.Response = "Sucesso";
                            Ret.Satus = 1;
                            return Ret;
                        }
                        else
                        {
                            Ret.Response = dataJson.status;
                            Ret.Satus = 0;
                            return Ret;
                        }
                    }
                    else
                    {
                        var aux = await result.Content.ReadAsStringAsync();
                        if (aux.IndexOf("Go back to Instagram.") > -1 && atual < 4)
                        {
                            await Task.Delay(1246);
                            return await Insta.UpdateProfileAsync(name, email, phone, gender, bio, url, similar, atual++);
                        }
                        else
                        {
                            if (aux.IndexOf("\"checkpoint_required\"") > -1)
                            {
                                Ret.Satus = -2;
                                Ret.Response = aux;
                                dynamic challenge = JsonConvert.DeserializeObject(aux);
                                Insta.Challenge_URL = challenge.checkpoint_url;
                                return Ret;
                            }
                            else
                            {
                                if (aux.IndexOf("\"feedback_required\"") > -1)
                                {
                                    Ret.Satus = -3;
                                    Ret.Response = aux;
                                    return Ret;
                                }
                                else
                                {
                                    Ret.Satus = -4;
                                    Ret.Response = aux;
                                    return Ret;
                                }
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    Ret.Response = err.Message;
                    Ret.Satus = -1;
                    return Ret;
                }
            }
            else
            {
                Ret.Response = "Não foi possivel recuperar o perfil atual";
                return Ret;
            }
        }

        public async static Task<InstaResponse> AllowSuspiciosLoginByIdAsync(this InstagramWeb Insta, string ID, int atual = 1)
        {
            //https://www.instagram.com/session/login_activity/avow_login/
            InstaResponse Ret = new InstaResponse
            {
                Response = "",
                Satus = 0
            };
            try
            {
                var dict = new Dictionary<string, string>();
                dict.Add("login_id", ID);
                var req = new HttpRequestMessage(HttpMethod.Post, Insta.BasicUrl + "session/login_activity/avow_login/") { Content = new FormUrlEncodedContent(dict) };
                var result = await Insta.Client.SendAsync(req);
                if (result.IsSuccessStatusCode)
                {
                    Ret.Response = "Sucesso";
                    Ret.Satus = 1;
                    return Ret;
                }
                else
                {
                    var aux = await result.Content.ReadAsStringAsync();
                    if (aux.IndexOf("Go back to Instagram.") > -1 && atual < 4)
                    {
                        await Task.Delay(1246);
                        return await Insta.AllowSuspiciosLoginByIdAsync(ID, atual++);
                    }
                    else
                    {
                        if (aux.IndexOf("\"checkpoint_required\"") > -1)
                        {
                            Ret.Satus = -2;
                            Ret.Response = aux;
                            dynamic challenge = JsonConvert.DeserializeObject(aux);
                            Insta.Challenge_URL = challenge.checkpoint_url;
                            return Ret;
                        }
                        else
                        {
                            if (aux.IndexOf("\"feedback_required\"") > -1)
                            {
                                Ret.Satus = -3;
                                Ret.Response = aux;
                                return Ret;
                            }
                            else
                            {
                                Ret.Satus = -4;
                                Ret.Response = aux;
                                return Ret;
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Ret.Satus = -1;
                Ret.Response = err.Message;
                return Ret;
            }
        }

        public async static Task<MediaRelation> GetMediaRelationByShortcodeAsync(this InstagramWeb Insta, string Shortcode, int atual = 0)
        {
            //https://www.instagram.com/p/CWBOFNKL_zz/?__a=1
            MediaRelation Ret = new MediaRelation
            {
                Is_Complet = false,
                Is_PrivateOwner = false,
                Is_FollowingOwner = false,
                Is_Liked = false,
                MediaID = "",
                MediaShortcode = Shortcode,
                OwnerPK = "",
                OwnerUsername = "",
                Response = ""
            };
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Get, Insta.BasicUrl + $"p/{Shortcode}/?__a=1");
                var res = await Insta.Client.SendAsync(req);
                if (res.IsSuccessStatusCode)
                {
                    if ((await res.Content.ReadAsStringAsync()).IndexOf("\"shortcode_media\"") > -1)
                    {
                        var serializado = await res.Content.ReadAsStringAsync();
                        dynamic dataJson = JsonConvert.DeserializeObject(serializado);
                        var data = dataJson.graphql.shortcode_media;
                        Ret.Is_PrivateOwner = data.owner.is_private;
                        Ret.Is_FollowingOwner = data.owner.followed_by_viewer;
                        Ret.Is_Liked = data.viewer_has_liked;
                        Ret.OwnerPK = data.owner.id;
                        Ret.OwnerUsername = data.owner.username;
                        Ret.MediaID = data.id;
                        Ret.Is_Complet = true;
                        Ret.Response = "Sucesso";
                        Ret.Status = 1;
                        return Ret;
                    }
                    else
                    {
                        var serializado = await res.Content.ReadAsStringAsync();
                        Ret.Is_Complet = false;
                        Ret.Response = serializado;
                        return Ret;
                    }
                }
                else
                {
                    var aux = await res.Content.ReadAsStringAsync();
                    if (aux.IndexOf("Go back to Instagram.") > -1 && atual < 3)
                    {
                        await Task.Delay(1246);
                        return await Insta.GetMediaRelationByShortcodeAsync(Shortcode, atual++);
                    }
                    else
                    {
                        if (aux.IndexOf("\"checkpoint_required\"") > -1)
                        {
                            Ret.Status = -2;
                            Ret.Is_Complet = false;
                            Ret.Response = aux;
                            dynamic challenge = JsonConvert.DeserializeObject(aux);
                            Insta.Challenge_URL = challenge.checkpoint_url;
                            return Ret;
                        }
                        else
                        {
                            if (aux.IndexOf("\"feedback_required\"") > -1)
                            {
                                Ret.Status = -3;
                                Ret.Is_Complet = false;
                                Ret.Response = aux;
                                return Ret;
                            }
                            else
                            {
                                Ret.Status = -4;
                                Ret.Is_Complet = false;
                                Ret.Response = aux;
                                return Ret;
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Ret.Status = -1;
                Ret.Response = err.Message;
                Ret.Is_Complet = false;
                return Ret;
            }
        }

        public async static Task<InstaResponse> LikeMediaByIdAsync(this InstagramWeb Insta, string MediaID, int atual = 1)
        {
            //https://www.instagram.com/web/likes/2702503181777108211/like/
            InstaResponse Ret = new InstaResponse
            {
                Response = "",
                Satus = 0
            };
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Post, Insta.BasicUrl + $"web/likes/{MediaID}/like/");
                var result = await Insta.Client.SendAsync(req);
                if (result.IsSuccessStatusCode)
                {
                    var serializado = await result.Content.ReadAsStringAsync();
                    dynamic dataJson = JsonConvert.DeserializeObject(serializado);
                    try
                    {
                        if (dataJson.status == "ok")
                        {
                            Ret.Response = "Sucesso";
                            Ret.Satus = 1;
                            return Ret;
                        }
                        Ret.Response = serializado;
                        Ret.Satus = 2;
                        return Ret;
                    }
                    catch
                    {
                        Ret.Response = "Erro ao curtir a publicação: " + serializado;
                        Ret.Satus = 2;
                        return Ret;
                    }
                }
                else
                {
                    var aux = await result.Content.ReadAsStringAsync();
                    if (aux.IndexOf("Go back to Instagram.") > -1 && atual < 4)
                    {
                        await Task.Delay(1246);
                        return await Insta.LikeMediaByIdAsync(MediaID, atual++);
                    }
                    else
                    {
                        if (aux.IndexOf("\"checkpoint_required\"") > -1)
                        {
                            Ret.Satus = -2;
                            Ret.Response = "Conta com bloqueio";
                            dynamic challenge = JsonConvert.DeserializeObject(aux);
                            Insta.Challenge_URL = challenge.checkpoint_url;
                            return Ret;
                        }
                        else
                        {
                            if (aux.IndexOf("\"checkpoint_required\"") > -1)
                            {
                                Ret.Satus = -2;
                                Ret.Response = aux;
                                dynamic challenge = JsonConvert.DeserializeObject(aux);
                                Insta.Challenge_URL = challenge.checkpoint_url;
                                return Ret;
                            }
                            else
                            {
                                if (aux.IndexOf("\"feedback_required\"") > -1)
                                {
                                    Ret.Satus = -3;
                                    Ret.Response = aux;
                                    return Ret;
                                }
                                else
                                {
                                    Ret.Satus = -4;
                                    Ret.Response = aux;
                                    return Ret;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Ret.Response = err.Message;
                Ret.Satus = -1;
            }
            return Ret;
        }

        public async static Task<InstaResponse> UnlikeMediaByIdAsync(this InstagramWeb Insta, string MediaID, int atual = 1)
        {
            //https://www.instagram.com/web/likes/2702503181777108211/unlike/
            InstaResponse Ret = new InstaResponse
            {
                Response = "",
                Satus = 0
            };
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Post, Insta.BasicUrl + $"web/likes/{MediaID}/unlike/");
                var result = await Insta.Client.SendAsync(req);
                if (result.IsSuccessStatusCode)
                {
                    var serializado = await result.Content.ReadAsStringAsync();
                    dynamic dataJson = JsonConvert.DeserializeObject(serializado);
                    try
                    {
                        if (dataJson.status == "ok")
                        {
                            Ret.Response = "Sucesso";
                            Ret.Satus = 1;
                            return Ret;
                        }
                        Ret.Response = serializado;
                        Ret.Satus = 2;
                        return Ret;
                    }
                    catch
                    {
                        Ret.Response = "Erro ao deixar de curtir a publicação: " + serializado;
                        Ret.Satus = 2;
                        return Ret;
                    }
                }
                else
                {
                    var aux = await result.Content.ReadAsStringAsync();
                    if (aux.IndexOf("Go back to Instagram.") > -1 && atual < 4)
                    {
                        await Task.Delay(1246);
                        return await Insta.UnlikeMediaByIdAsync(MediaID, atual++);
                    }
                    else
                    {
                        if (aux.IndexOf("\"checkpoint_required\"") > -1)
                        {
                            Ret.Satus = -2;
                            Ret.Response = aux;
                            dynamic challenge = JsonConvert.DeserializeObject(aux);
                            Insta.Challenge_URL = challenge.checkpoint_url;
                            return Ret;
                        }
                        else
                        {
                            if (aux.IndexOf("\"feedback_required\"") > -1)
                            {
                                Ret.Satus = -3;
                                Ret.Response = aux;
                                return Ret;
                            }
                            else
                            {
                                Ret.Satus = -4;
                                Ret.Response = aux;
                                return Ret;
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Ret.Response = err.Message;
                Ret.Satus = -1;
            }
            return Ret;
        }

        public async static Task<InstaResponseJson> CommentMediaByIdAsync(this InstagramWeb Insta, string MediaID, string Comentario, int atual = 1)
        {
            //https://www.instagram.com/web/comments/2702524355445609875/add/
            InstaResponseJson Ret = new InstaResponseJson
            {
                Response = "",
                Status = 0,
                Json = null
            };
            try
            {
                var dict = new Dictionary<string, string>();
                dict.Add("comment_text", Comentario);
                dict.Add("replied_to_comment_id", "");
                var req = new HttpRequestMessage(HttpMethod.Post, Insta.BasicUrl + $"web/comments/{MediaID}/add/") { Content = new FormUrlEncodedContent(dict) };
                var result = await Insta.Client.SendAsync(req);
                if (result.IsSuccessStatusCode)
                {
                    var serializado = await result.Content.ReadAsStringAsync();
                    dynamic dataJson = JsonConvert.DeserializeObject(serializado);
                    try
                    {
                        if (dataJson.status == "ok")
                        {
                            Ret.Response = "Sucesso";
                            Ret.Status = 1;
                            Ret.Json = dataJson;
                            return Ret;
                        }
                        Ret.Response = serializado;
                        Ret.Status = 2;
                        return Ret;
                    }
                    catch
                    {
                        Ret.Response = "Erro ao comentar a publicação: " + serializado;
                        Ret.Status = 2;
                        return Ret;
                    }
                }
                else
                {
                    var aux = await result.Content.ReadAsStringAsync();
                    if (aux.IndexOf("Go back to Instagram.") > -1 && atual < 4)
                    {
                        await Task.Delay(1246);
                        return await Insta.CommentMediaByIdAsync(MediaID, Comentario, atual++);
                    }
                    else
                    {
                        if (aux.IndexOf("\"checkpoint_required\"") > -1)
                        {
                            Ret.Status = -2;
                            Ret.Response = aux;
                            dynamic challenge = JsonConvert.DeserializeObject(aux);
                            Insta.Challenge_URL = challenge.checkpoint_url;
                            return Ret;
                        }
                        else
                        {
                            if (aux.IndexOf("\"feedback_required\"") > -1)
                            {
                                Ret.Status = -3;
                                Ret.Response = aux;
                                return Ret;
                            }
                            else
                            {
                                Ret.Status = -4;
                                Ret.Response = aux;
                                return Ret;
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Ret.Response = err.Message;
                Ret.Status = -1;
            }
            return Ret;
        }

        private static string GetEncryptedPassword(string password, long? providedTime = null)
        {
            long time = providedTime ?? DateTime.UtcNow.ToUnixTime();
            return $"#PWD_INSTAGRAM:0:{time}:{password}";
        }
    }
}
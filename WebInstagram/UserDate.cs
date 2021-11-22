using Newtonsoft.Json;

namespace DIZUCMD.WebInstagram
{
    public class UserDate
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("enc_password")]
        public string Password { get; set; }
    }

    public class SendUsername
    {
        [JsonProperty("username")]
        public string Username { get; set; }
    }

    public class UserAgentRetorno
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("erro")]
        public string Mensagem { get; set; }
    }
}

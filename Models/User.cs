using DIZUCMD.Controllers.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DIZUCMD.Models.Users
{
    class User
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Senha { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }
    }

    class Login
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Senha { get; set; }
    }

    class Receiver
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("erro")]
        public string Erro { get; set; }

        [JsonProperty("data")]
        public List<User> Conta { get; set; }
    }

    static class ExtendUsers
    {
        static public Receiver Login (this Login l)
        {
            Receiver ret = new Receiver
            {
                Conta = null,
                Erro = "Informe os dados corretos para prosseguir.",
                Status = 0
            };
            if (String.IsNullOrEmpty(l.Email)) return ret;
            if (String.IsNullOrEmpty(l.Senha)) return ret;
            ret = UserController.LoginSystem(l);
            return ret;
        }
    }
}
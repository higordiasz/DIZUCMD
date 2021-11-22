using System;
using System.Collections.Generic;
using System.Text;

namespace DIZUCMD.Models.ActionModels
{
    public class ActionModel
    {
        public int Status { get; set; }
        public string Username { get; set; }
        public string Type { get; set; }
        public string Response { get; set; }
        public string ChallengeResponse { get; set; }
        public string Numero { get; set; }
        public string Email { get; set; }
        public string Codigo { get; set; }
    }

    public class TarefaModel
    {
        public string Tarefa { get; set; }
        public string Valor { get; set; }
    }

    public class LoginInstagram
    {
        public int Status { get; set; }
        public string Response { get; set; }
        public Boolean Logado { get; set; }
    }

    public class Result
    {
        public int Status { get; set; }
        public string Response { get; set; }
    }
}

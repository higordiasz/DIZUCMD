using Newtonsoft.Json;
using System.Collections.Generic;
using DIZUCMD.Controllers.Globals;
using DIZUCMD.Models.Retornos;

namespace DIZUCMD.Models.Globals
{
    class Global
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("delay1")]
        public int Delay1 { get; set; }

        [JsonProperty("delay2")]
        public int Delay2 { get; set; }

        [JsonProperty("quantidade")]
        public int Quantidade { get; set; }

        [JsonProperty("tcontas")]
        public int Timer_contas { get; set; }

        [JsonProperty("meta")]
        public int Meta { get; set; }

        [JsonProperty("tmeta")]
        public int Timer_Meta { get; set; }

        [JsonProperty("tblock")]
        public int Timer_Block { get; set; }

        [JsonProperty("cgrupo")]
        public int CadastroGrupo { get; set; }

        [JsonProperty("anonimo")]
        public bool Anonimo { get; set; }

        [JsonProperty("trocar")]
        public bool Trocar { get; set; }

        [JsonProperty("perfil")]
        public bool Perfil { get; set; }

        [JsonProperty("barra")]
        public bool Barra { get; set; }
    }

    class GlobalSender
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("delay1")]
        public int Delay1 { get; set; }

        [JsonProperty("delay2")]
        public int Delay2 { get; set; }

        [JsonProperty("quantidade")]
        public int Quantidade { get; set; }

        [JsonProperty("tcontas")]
        public int Timer_contas { get; set; }

        [JsonProperty("meta")]
        public int Meta { get; set; }

        [JsonProperty("tmeta")]
        public int Timer_Meta { get; set; }

        [JsonProperty("tblock")]
        public int Timer_Block { get; set; }

        [JsonProperty("cgrupo")]
        public int CadastroGrupo { get; set; }

        [JsonProperty("anonimo")]
        public bool Anonimo { get; set; }

        [JsonProperty("trocar")]
        public bool Trocar { get; set; }

        [JsonProperty("perfil")]
        public bool Perfil { get; set; }

        [JsonProperty("barra")]
        public bool Barra { get; set; }
    }

    class GlobalName
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }

    class BiddingGlobalList
    {
        public bool IsChecked { get; set; }
        public string Nome { get; set; }
        public int QtdGrupos { get; set; }
    }

    class Receiver
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("erro")]
        public string Erro { get; set; }

        [JsonProperty("data")]
        public List<Global> Globais { get; set; }
    }

    static class ExtendsGlobal
    {
        /// <summary>
        /// Cadastrar o grupo
        /// </summary>
        /// <param name="G">Grupo a cadastrar</param>
        /// <returns>0 - Erro, 1 = Certo, 2 - Licença Vencida</returns>
        static public Retorno Cadastrar (this Global G)
        {
            Retorno Ret = new Retorno
            {
                Mensagem = "Não foi possivel cadastrar o global",
                Status = 0
            };
            if (string.IsNullOrEmpty(G.Nome))
            {
                Ret.Status = 0;
                Ret.Mensagem = "Nome do Global não informado";
                return Ret;
            };
            var sender = new GlobalSender
            {
                Anonimo = G.Anonimo,
                Barra = G.Barra,
                CadastroGrupo = G.CadastroGrupo,
                Delay1 = G.Delay1,
                Delay2 = G.Delay2,
                Meta = G.Meta,
                Nome = G.Nome,
                Perfil = G.Perfil,
                Quantidade = G.Quantidade,
                Timer_Block = G.Timer_Block,
                Timer_contas = G.Timer_contas,
                Timer_Meta = G.Timer_Meta,
                Trocar = G.Trocar,
                Token = Variaveis.Token
            };
            var apiRet = GlobalController.CreateGlobal(sender);
            if (apiRet.Status == 1)
            {
                Ret.Status = 1;
                Ret.Mensagem = "Global cadastrado com sucesso";
                return Ret;
            }
            Ret.Status = apiRet.Status;
            Ret.Mensagem = apiRet.Erro;
            return Ret;
        }

        /// <summary>
        /// Alterar o grupo.
        /// </summary>
        /// <param name="G">Grupo que sera alterado.</param>
        /// <returns>0 - Erro, 1 = Certo, 2 - Licença Vencida</returns>
        static public Retorno Alterar(this Global G)
        {
            Retorno Ret = new Retorno
            {
                Mensagem = "Não foi possivel alterar o global",
                Status = 0
            };
            if (string.IsNullOrEmpty(G.Nome))
            {
                Ret.Status = 0;
                Ret.Mensagem = "Nome do Global não informado";
                return Ret;
            };
            var sender = new GlobalSender
            {
                Anonimo = G.Anonimo,
                Barra = G.Barra,
                CadastroGrupo = G.CadastroGrupo,
                Delay1 = G.Delay1,
                Delay2 = G.Delay2,
                Meta = G.Meta,
                Nome = G.Nome,
                Perfil = G.Perfil,
                Quantidade = G.Quantidade,
                Timer_Block = G.Timer_Block,
                Timer_contas = G.Timer_contas,
                Timer_Meta = G.Timer_Meta,
                Trocar = G.Trocar,
                Token = Variaveis.Token
            };
            var apiRet = GlobalController.AlterGlobal(sender);
            if (apiRet.Status == 1)
            {
                Ret.Status = 1;
                Ret.Mensagem = "Global alterado com sucesso";
                return Ret;
            }
            Ret.Status = apiRet.Status;
            Ret.Mensagem = apiRet.Erro;
            return Ret;
        }

        /// <summary>
        /// Remover o Global
        /// </summary>
        /// <param name="G">Global a ser removido</param>
        /// <returns>0 - Erro, 1 = Certo, 2 - Licença Vencida</returns>
        static public Retorno Remover (this Global G)
        {
            Retorno Ret = new Retorno
            {
                Mensagem = "Não foi possivel reemover o global",
                Status = 0
            };
            if (string.IsNullOrEmpty(G.Nome))
            {
                Ret.Status = 0;
                Ret.Mensagem = "Nome do Global não informado";
                return Ret;
            };
            var apiRet = GlobalController.RemoveGlobalByName(G.Nome);
            if (apiRet.Status == 1)
            {
                Ret.Status = 1;
                Ret.Mensagem = "Global removido com sucesso";
                return Ret;
            }
            Ret.Status = apiRet.Status;
            Ret.Mensagem = apiRet.Erro;
            return Ret;
        }

        /// <summary>
        /// Buscar um Global pelo nome.
        /// </summary>
        /// <param name="nome">Nome do Global para carregar</param>
        /// <returns>Global ou Null</returns>
        static public Global GetGlobalByname(string nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                return null;
            };
            var apiRet = GlobalController.GetGlobalByName(nome);
            if (apiRet.Status == 1)
            {
                return apiRet.Globais[0];
            }
            return null;
        }

        /// <summary>
        /// Buscar todos os Global.
        /// </summary>
        /// <param name="nome">Nome do Global para carregar</param>
        /// <returns>Global ou Null</returns>
        static public List<Global> GetAllGlobal()
        {
            var apiRet = GlobalController.GetAllGlobal();
            if (apiRet.Status == 1)
            {
                return apiRet.Globais;
            }
            return null;
        }
    }
}

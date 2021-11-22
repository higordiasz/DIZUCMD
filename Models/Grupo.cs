using DIZUCMD.Models.Retornos;
using Newtonsoft.Json;
using DIZUCMD.Controllers.Grupos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DIZUCMD.Models.Grupos
{
    class Grupo
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("global")]
        public string Global { get; set; }

        [JsonProperty("contas")]
        public List<string> Contas { get; set; }
    }

    class ListGrupoBidding
    {
        public string Nome { get; set; }
        public int QtdContas { get; set; }
        public bool IsCheked { get; set; }
    }

    class SenderGroup
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("global")]
        public string Global { get; set; }

        [JsonProperty("contas")]
        public List<string> Contas { get; set; }
    }

    class GroupName
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }

    class Receiver
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("erro")]
        public string Erro { get; set; }

        [JsonProperty("data")]
        public List<Grupo> Grupos { get; set; }
    }

    static class ExtendGrupos
    {
        /// <summary>
        /// Cadastrar o grupo
        /// </summary>
        /// <param name="G">Grupo a cadastrar</param>
        /// <returns>0 - Erro, 1 = Certo, 2 - Licença Vencida</returns>
        static public Retorno Cadastrar(this Grupo G)
        {
            Retorno Ret = new Retorno
            {
                Mensagem = "Não foi possivel cadastrar o grupo",
                Status = 0
            };
            SenderGroup Sender = new SenderGroup
            {
                Contas = G.Contas,
                Global = G.Global,
                Nome = G.Nome,
                Token = Variaveis.Token
            };
            if (string.IsNullOrEmpty(G.Nome))
            {
                Ret.Status = 0;
                Ret.Mensagem = "Nome do Grupo não informado";
                return Ret;
            };
            var apiRet = GrupoController.CreateGroup(Sender);
            if (apiRet.Status == 1)
            {
                Ret.Status = 1;
                Ret.Mensagem = "Grupo cadastrado com sucesso";
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
        static public Retorno Alterar(this Grupo G)
        {
            Retorno Ret = new Retorno
            {
                Mensagem = "Não foi possivel alterar o Grupo",
                Status = 0
            };
            SenderGroup Sender = new SenderGroup
            {
                Contas = G.Contas,
                Global = G.Global,
                Nome = G.Nome,
                Token = Variaveis.Token
            };
            if (string.IsNullOrEmpty(G.Nome))
            {
                Ret.Status = 0;
                Ret.Mensagem = "Nome do Grupo não informado";
                return Ret;
            };
            var apiRet = GrupoController.AlterGroup(Sender);
            if (apiRet.Status == 1)
            {
                Ret.Status = 1;
                Ret.Mensagem = "Grupo alterado com sucesso";
                return Ret;
            }
            Ret.Status = apiRet.Status;
            Ret.Mensagem = apiRet.Erro;
            return Ret;
        }

        /// <summary>
        /// Remover o Grupo
        /// </summary>
        /// <param name="G">Grupo a ser removido</param>
        /// <returns>0 - Erro, 1 = Certo, 2 - Licença Vencida</returns>
        static public Retorno Remover(string Nome)
        {
            Retorno Ret = new Retorno
            {
                Mensagem = "Não foi possivel reemover o Grupo",
                Status = 0
            };
            if (string.IsNullOrEmpty(Nome))
            {
                Ret.Status = 0;
                Ret.Mensagem = "Nome do Grupo não informado";
                return Ret;
            };
            var apiRet = GrupoController.RemoveGroupByName(Nome);
            if (apiRet.Status == 1)
            {
                Ret.Status = 1;
                Ret.Mensagem = "Grupo removido com sucesso";
                return Ret;
            }
            Ret.Status = apiRet.Status;
            Ret.Mensagem = apiRet.Erro;
            return Ret;
        }

        /// <summary>
        /// Buscar um Grupo pelo nome.
        /// </summary>
        /// <param name="nome">Nome do Grupo para carregar</param>
        /// <returns>Grupo ou Null</returns>
        static public Grupo GetGroupByname(string nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                return null;
            };
            var apiRet = GrupoController.GetGrupoByName(nome);
            if (apiRet.Status == 1)
            {
                return apiRet.Grupos[0];
            }
            return null;
        }

        /// <summary>
        /// Buscar todos os Grupo.
        /// </summary>
        /// <param name="nome">Nome do Grupo para carregar</param>
        /// <returns>Grupo ou Null</returns>
        static public List<Grupo> GetAllGroup()
        {
            var apiRet = GrupoController.GetAllGroup();
            if (apiRet.Status == 1)
            {
                return apiRet.Grupos;
            }
            return null;
        }
    }
}

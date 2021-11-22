using DIZUCMD.Controllers.HelpInstaAPI;
using DIZUCMD.DIZU;
using DIZUCMD.WebInstagram;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DIZUCMD.Models.Instagrams;

namespace DIZUCMD
{
    class Variaveis
    {
        private static string _token = "";
        private static string _grupo = "";
        private static string _contaatual = "";
        private static string _typeua = "seed";
        private static double _total = 0;
        private static double _saldogerado = 0.0;
        private static double _totalseguir = 0;
        private static double _totalcurtir = 0;
        private static double _meta = 0;
        private static double _totalmeta = 0;
        private static double _metasalcansadas = 0;
        private static double _totalconta = 0;
        private static bool _humanizacao = false;
        public static string Token { get { return _token; } set { _token = value; } }
        public static string Grupo { get { return _grupo; } set { _grupo = value; } }
        public static string Conta_Atual { get { return _contaatual; } set { _contaatual = value; } }
        public static string TypeUserAgent { get { return _typeua; } set { _typeua = value; } }
        public static double Total { get { return _total; } set { _total = value; } }
        public static double Saldo { get { return _saldogerado; } set { _saldogerado = value; } }
        public static double TotalSeguir { get { return _totalseguir; } set { _totalseguir = value; } }
        public static double TotalCurtir { get { return _totalcurtir; } set { _totalcurtir = value; } }
        public static double Meta { get { return _meta; } set { _meta = value; } }
        public static double TotalMeta { get { return _totalmeta; } set { _totalmeta = value; } }
        public static double MetaAlcancada { get { return _metasalcansadas; } set { _metasalcansadas = value; } }
        public static double TotalContaAtual { get { return _totalconta; } set { _totalconta = value; } }
        public static bool Humanizacao { get { return _humanizacao; } set { _humanizacao = value; } }
        public static ConsoleColor Verde { get { return ConsoleColor.Green; } }
        public static ConsoleColor Amarelo { get { return ConsoleColor.Yellow; } }
        public static ConsoleColor Vermelho { get { return ConsoleColor.Red; } }
        public static ConsoleColor Ciano { get { return ConsoleColor.Cyan; } }
        public static ConsoleColor Branco { get { return ConsoleColor.White; } }
        public static ConsoleColor Magenta { get { return ConsoleColor.Magenta; } }
        public static ConsoleColor Azul { get { return ConsoleColor.Blue; } }
        public static ConsoleColor AzulEscuro { get { return ConsoleColor.DarkBlue; } }
        public static ConsoleColor CianoEscuro { get { return ConsoleColor.DarkCyan; } }
        public static ConsoleColor CinzaEscuro { get { return ConsoleColor.DarkGray; } }
        public static ConsoleColor VerdeEscuro { get { return ConsoleColor.DarkGreen; } }
        public static ConsoleColor VermelhoEscuro { get { return ConsoleColor.DarkRed; } }
        public static ConsoleColor AmareloEscuro { get { return ConsoleColor.DarkYellow; } }
    }

    class Program
    {
        static Dizu Plat { get; set; }
        static private Models.Grupos.Grupo Grupo { get; set; }
        static private Models.Globals.Global Global { get; set; }
        static private List<Models.Instagrams.Instagram> Contas { get; set; }
        static private Models.Blocks.Bloqueios Bloqueios { get; set; }
        static private List<string> Challenges { get; set; }
        static private List<string> Incorrects { get; set; }
        static private List<Models.Instagrams.InstaController> InstaController { get; set; }
        static private string[] argumentos { get; set; }

        static private int Index = 0;

        static async Task Main(string[] args)
        {
            if (System.OperatingSystem.IsWindows())
            {
                Console.SetWindowSize(63, 30);
                Console.Title = "Arka Dizu";
            }
            ConsoleHelper.ClearConsole();
            try
            {
                if (args.Length > 1)
                {
                    Variaveis.Token = args[0];
                    Variaveis.Grupo = args[1];
                    Variaveis.Humanizacao = args[2] == "true";
                    Variaveis.TypeUserAgent = args[3];
                    var check = Controllers.Users.UserController.CheckToken(Variaveis.Token);
                    bool checkToken = check.Status == 1 ? true : false; //Checar o token
                    if (!checkToken)
                    {
                        ConsoleHelper.WriteFullLine(check.Erro, Variaveis.Vermelho);
                        await Task.Delay(TimeSpan.FromHours(5));
                        return;
                    }
                    else
                    {
                        argumentos = args;
                        await Iniciar();
                        await Task.Delay(TimeSpan.FromHours(5));
                        return;
                    }
                } else
                {
                    ConsoleHelper.WriteFullLine("Abra a nossa automação pelo Arka", Variaveis.Vermelho);
                    await Task.Delay(TimeSpan.FromHours(5));
                    return;
                }
            } catch
            {
                ConsoleHelper.WriteFullLine("Abra a nossa automação pelo Arka", Variaveis.Vermelho);
                await Task.Delay(TimeSpan.FromHours(5));
                return;
            }
        }

        static async Task Iniciar ()
        {
            if (String.IsNullOrEmpty(Variaveis.Grupo))
            {
                //Close
                LogMessage("Não foi possivel carregar o nome do grupo");
                ConsoleHelper.WriteFullLine("Erro ao carregar o grupo", Variaveis.Vermelho);
                return;
            }
            Models.Grupos.Grupo ret = null;
            if (argumentos.Length > 4)
            {
                ret = new Models.Grupos.Grupo
                {
                    Contas = new List<string>(),
                    Global = Variaveis.Grupo,
                    Nome = Variaveis.Grupo
                };
                for (var i = 4; i < argumentos.Length; i++)
                {
                    ret.Contas.Add(argumentos[i]);
                }
            } else
            {
                ret = Models.Grupos.ExtendGrupos.GetGroupByname(Variaveis.Grupo);
            }
            if (ret == null)
            {
                //Close
                LogMessage($"Não foi possivel localizar o grupo com nome '{Variaveis.Grupo}'");
                ConsoleHelper.WriteFullLine("Erro ao carregar o grupo com o nome " + Variaveis.Grupo, Variaveis.Vermelho);
                return;
            }
            Grupo = ret;
            if (Grupo.Contas.Count <= 0)
            {
                LogMessage($"O grupo '{Variaveis.Grupo}' não possui contas para rodar o bot");
                ConsoleHelper.WriteFullLine($"O grupo {Variaveis.Grupo} não possui contas para rodar o bot", Variaveis.Vermelho);
                return;
            }
            var resGlobal = Models.Globals.ExtendsGlobal.GetGlobalByname(Grupo.Global);
            if (resGlobal == null)
            {
                LogMessage($"Não foi possivel localizar o global com nome '{Grupo.Global}'");
                ConsoleHelper.WriteFullLine($"Não foi possivel localizar o global com o nome {Grupo.Global}", Variaveis.Vermelho);
                return;
            }
            Global = resGlobal;
            Contas = new List<Models.Instagrams.Instagram>();
            foreach (string username in Grupo.Contas)
            {
                var res = Models.Instagrams.ExtendInstagram.GetInstaByUsername(username);
                if (res != null)
                {
                    Contas.Add(res);
                }
            }
            if (Contas.Count > 0)
            {
                await IniciarSistema();
            }
            else
            {
                LogMessage($"Não foi possivel carregar suas contas do instagram.");
                ConsoleHelper.WriteFullLine("Não foi possivel carregar suas contas do instagram", Variaveis.Vermelho);
                return;
            }
        }

        static private async Task IniciarSistema()
        {
            Console.Title = $"ArkaDizu - Grupo: {Variaveis.Grupo}";
            ConsoleHelper.ClearConsole();
            ConsoleHelper.WriteFullLine("Todos os dados foram carregados do servidor, iniciando o sistema.");
            await Task.Delay(1000);
            ConsoleHelper.ClearConsole();
            ConsoleHelper.WriteFullLine("Realizando login na plataforma.");
            var dir = Directory.GetCurrentDirectory();
            if (File.Exists($@"{dir}\Config\dizutoken.txt"))
            {
                string[] linhas = File.ReadAllLines($@"{dir}\Config\dizutoken.txt");
                if (linhas.Length == 1)
                {
                    string DT = linhas[0];
                    Plat = new Dizu(DT);
                    if (Plat.LoadComplet)
                    {
                        await RodarCiclo();
                    } else
                    {
                        ConsoleHelper.WriteFullLine("Erro ao conectar com a Dizu", Variaveis.Vermelho);
                        return;
                    }
                }
                else
                {
                    LogMessage($"Não foi possivel carregar o login da plataforma");
                    ConsoleHelper.WriteFullLine("Erro ao carregar o token da plataforma", Variaveis.Vermelho);
                    return;
                }
            }
            else
            {
                LogMessage($"Não foi possivel carregar o login da plataforma");
                ConsoleHelper.WriteFullLine("Erro ao carregar token da plataforma", Variaveis.Vermelho);
                return;
            }
        }

        static private async Task RodarCiclo()
        {
            Bloqueios = new Models.Blocks.Bloqueios();
            Incorrects = new List<string>();
            Challenges = new List<string>();
            InstaController = new List<Models.Instagrams.InstaController>();
            Variaveis.Meta = Global.Meta;
            ConsoleHelper.WriteFullLine("Login efetuado na plataforma", Variaveis.Verde);
            await Task.Delay(500);
            ConsoleHelper.ClearConsole();
            ConsoleHelper.WriteFullLine("Iniciando o bot");
            try
            {
                while (true)
                {
                    for (int i = 0; i < Contas.Count; i++)
                    {
                        if (Contas[i].Block)
                        {
                            Bloqueios.LimparLista();
                            if (Bloqueios.Blocks.Exists(c => c.Username == Contas[i].Username))
                            {
                                ConsoleHelper.WriteFullLine($"Conta '{Contas[i].Username}' está bloqueada temporariamente.", Variaveis.Amarelo);
                            }
                            else
                            {
                                //Rodar conta
                                ConsoleHelper.WriteFullLine($"Conta: '{Contas[i].Username}'");
                                Index = i;
                                await RodarConta();
                            }
                        }
                        else
                        {
                            if (!Challenges.Exists(c => c == Contas[i].Username) && !Incorrects.Exists(c => c == Contas[i].Username))
                            {
                                //Rodar conta
                                ConsoleHelper.WriteFullLine($"Conta: '{Contas[i].Username}'");
                                Index = i;
                                await RodarConta();
                            }
                        }
                        if (Contas.Count < 2)
                        {
                            if (Challenges.Count == Contas.Count)
                            {
                                ConsoleHelper.WriteFullLine("Não possui contas para continuar", Variaveis.Magenta);
                                await Task.Delay(TimeSpan.FromHours(10));
                                return;
                            }
                            else
                            {
                                if ((Bloqueios.Blocks.Count + Challenges.Count) == Contas.Count)
                                {
                                    ConsoleHelper.WriteFullLine("Não possui contas para continuar", Variaveis.Magenta);
                                    ConsoleHelper.WriteFullLine("Aguardando 10 minutos", Variaveis.Ciano);
                                    await Task.Delay(TimeSpan.FromMinutes(10));
                                }
                            }
                        }
                        else
                        {
                            int t = 0;
                            foreach (var ig in Contas)
                            {
                                if (!ig.Challeng && !ig.Incorrect)
                                {
                                    t++;
                                }
                            }
                            if (t == 0)
                            {
                                ConsoleHelper.WriteFullLine("Não possui contas para continuar", Variaveis.Magenta);
                                await Task.Delay(TimeSpan.FromHours(10));
                                return;
                            }
                            else
                            {
                                if ((Bloqueios.Blocks.Count + Challenges.Count) == Contas.Count)
                                {
                                    ConsoleHelper.WriteFullLine("Não possui contas para continuar", Variaveis.Magenta);
                                    ConsoleHelper.WriteFullLine("Aguardando 10 minutos", Variaveis.Ciano);
                                    await Task.Delay(TimeSpan.FromMinutes(10));
                                }
                            }
                        }
                        ConsoleHelper.WriteFullLine("Aguardando tempo entre contas", Variaveis.Ciano);
                        await Task.Delay(TimeSpan.FromSeconds(Global.Timer_contas));
                    }
                }
            }
            catch (Exception err)
            {
                LogMessage($"Erro ao rodar 'Ciclo' : {err.Message}");
                ConsoleHelper.WriteFullLine("Um erro inesperado aconteceu, contact o suporte", Variaveis.VerdeEscuro);
                return;
            }
        }

        static private async Task RodarConta()
        {
            try
            {
                Variaveis.Conta_Atual = "@" + Contas[Index].Username;
                Console.Title = $"@{Contas[Index].Username}: 1/{Global.Quantidade}";
                int i = -1;
                if (InstaController.Exists(ig => ig.Insta.Username == Contas[Index].Username))
                {
                    i = InstaController.FindIndex(inst => inst.Insta.Username == Contas[Index].Username);
                }
                if (i < 0)
                {
                    var res = Plat.CheckAccount(Contas[Index].Username);
                    if (res)
                    {
                        Models.Instagrams.InstaController ig = new Models.Instagrams.InstaController
                        {
                            Insta = Contas[Index],
                            isLogged = false,
                            PictureURL = "https://imgur.com/b24Rzo7.jpg",
                            ContaID = Plat.GetPKID(Contas[Index].Username),
                            Web = null,
                            Seguir = 0,
                            Curtir = 0,
                            Total = 0
                        };
                        InstaController.Add(ig);
                        i = InstaController.Count - 1;
                    }
                    else
                    {
                        ConsoleHelper.WriteFullLine($"Náo foi possivel localizar a conta '{Contas[Index].Username}' na plataforma", Variaveis.CianoEscuro);
                        return;
                    }
                }
                if (!InstaController[i].isLogged)
                {
                    ConsoleHelper.ClearConsole();
                    ConsoleHelper.WriteFullLine($"Realizando login na conta '{Contas[Index].Username}'");
                    UserDate sessionData = new UserDate()
                    {
                        Username = Contas[Index].Username.ToLower(),
                        Password = Contas[Index].Password
                    };
                    InstagramWeb Web = null;
                    if (Variaveis.TypeUserAgent == "txt")
                    {
                        var dir = Directory.GetCurrentDirectory();
                        string[] linhas = File.ReadAllLines($@"{dir}\Config\useragent.txt");
                        if (linhas.Length > 0)
                        {
                            var rand = new Random();
                            var li = rand.Next(0, linhas.Length);
                            Web = new InstagramWeb(sessionData, true, Variaveis.TypeUserAgent, linhas[li]);
                        }
                        else
                        {
                            Web = new InstagramWeb(sessionData, true, "seed");
                        }
                    }
                    else
                    {
                        Web = new InstagramWeb(sessionData, true, Variaveis.TypeUserAgent);
                    }
                    InstaController[i].Web = Web;
                    var login = await Login(Web);
                    if (login.Status != 1)
                    {
                        switch (login.Status)
                        {
                            case 2:
                                ConsoleHelper.WriteFullLine(login.Response, Variaveis.Amarelo);
                                Contas[Index].AdicionarBlock();
                                Bloqueios.AdicionarBlock(Contas[Index].Username, Global.Timer_Block);
                                await Task.Delay(1548);
                                break;
                            case 5:
                                ConsoleHelper.WriteFullLine(login.Response, Variaveis.VermelhoEscuro);
                                Contas[Index].AdicionarChallenge();
                                Challenges.Add(Contas[Index].Username);
                                await Task.Delay(1548);
                                break;
                            case 6:
                                ConsoleHelper.WriteFullLine(login.Response, Variaveis.AzulEscuro);
                                Contas[Index].AdicionarChallenge();
                                Challenges.Add(Contas[Index].Username);
                                await Task.Delay(1548);
                                break;
                            case 7:
                                ConsoleHelper.WriteFullLine(login.Response, Variaveis.Vermelho);
                                Contas[Index].AdicionarChallenge();
                                Challenges.Add(Contas[Index].Username);
                                await Task.Delay(1548);
                                break;
                            case 8:
                                ConsoleHelper.WriteFullLine(login.Response, Variaveis.Ciano);
                                Contas[Index].AdicionarChallenge();
                                Challenges.Add(Contas[Index].Username);
                                await Task.Delay(1548);
                                break;
                            case 9:
                                ConsoleHelper.WriteFullLine(login.Response, Variaveis.CinzaEscuro);
                                Contas[Index].AdicionarChallenge();
                                Challenges.Add(Contas[Index].Username);
                                await Task.Delay(1548);
                                break;
                            case 10:
                                ConsoleHelper.WriteFullLine(login.Response, Variaveis.CinzaEscuro);
                                Contas[Index].AdicionarChallenge();
                                Challenges.Add(Contas[Index].Username);
                                await Task.Delay(1548);
                                break;
                            case 11:
                                ConsoleHelper.WriteFullLine(login.Response, Variaveis.CianoEscuro);
                                Contas[Index].AdicionarChallenge();
                                Challenges.Add(Contas[Index].Username);
                                await Task.Delay(1548);
                                break;
                            case 12:
                                ConsoleHelper.WriteFullLine(login.Response, Variaveis.Magenta);
                                Contas[Index].AdicionarIncorrect();
                                Incorrects.Add(Contas[Index].Username);
                                await Task.Delay(1548);
                                break;
                            default:
                                ConsoleHelper.WriteFullLine(login.Response, Variaveis.CinzaEscuro);
                                Contas[Index].AdicionarChallenge();
                                Challenges.Add(Contas[Index].Username);
                                await Task.Delay(1548);
                                break;
                        }
                        ConsoleHelper.WriteFullLine("Indo para a próxima conta");
                        return;
                    } else
                    {
                        InstaController[i].isLogged = true;
                        Contas[Index].RemoverChallenge();
                        Contas[Index].RemoverIncorrect();
                        ConsoleHelper.WriteFullLine("Login realizado com sucesso", Variaveis.Verde);
                    }
                } else
                {
                    var check = await IsLogged(InstaController[i].Web);
                    if (!check)
                    {
                        ConsoleHelper.ClearConsole();
                        ConsoleHelper.WriteFullLine($"Realizando login na conta '{Contas[Index].Username}'");
                        UserDate sessionData = new UserDate()
                        {
                            Username = Contas[Index].Username.ToLower(),
                            Password = Contas[Index].Password
                        };
                        InstagramWeb Web = null;
                        if (Variaveis.TypeUserAgent == "txt")
                        {
                            var dir = Directory.GetCurrentDirectory();
                            string[] linhas = File.ReadAllLines($@"{dir}\Config\useragent.txt");
                            if (linhas.Length > 0)
                            {
                                var rand = new Random();
                                var li = rand.Next(0, linhas.Length);
                                Web = new InstagramWeb(sessionData, true, Variaveis.TypeUserAgent, linhas[li]);
                            } else
                            {
                                Web = new InstagramWeb(sessionData, true, "seed");
                            }
                        } else
                        {
                            Web = new InstagramWeb(sessionData, true, Variaveis.TypeUserAgent);
                        }
                        InstaController[i].Web = Web;
                        var login = await Login(Web);
                        if (login.Status != 1)
                        {
                            switch (login.Status)
                            {
                                case 2:
                                    ConsoleHelper.WriteFullLine(login.Response, Variaveis.Amarelo);
                                    Contas[Index].AdicionarBlock();
                                    Bloqueios.AdicionarBlock(Contas[Index].Username, Global.Timer_Block);
                                    await Task.Delay(1548);
                                    break;
                                case 5:
                                    ConsoleHelper.WriteFullLine(login.Response, Variaveis.VermelhoEscuro);
                                    Contas[Index].AdicionarChallenge();
                                    Challenges.Add(Contas[Index].Username);
                                    await Task.Delay(1548);
                                    break;
                                case 6:
                                    ConsoleHelper.WriteFullLine(login.Response, Variaveis.AzulEscuro);
                                    Contas[Index].AdicionarChallenge();
                                    Challenges.Add(Contas[Index].Username);
                                    await Task.Delay(1548);
                                    break;
                                case 7:
                                    ConsoleHelper.WriteFullLine(login.Response, Variaveis.Vermelho);
                                    Contas[Index].AdicionarChallenge();
                                    Challenges.Add(Contas[Index].Username);
                                    await Task.Delay(1548);
                                    break;
                                case 8:
                                    ConsoleHelper.WriteFullLine(login.Response, Variaveis.Ciano);
                                    Contas[Index].AdicionarChallenge();
                                    Challenges.Add(Contas[Index].Username);
                                    await Task.Delay(1548);
                                    break;
                                case 9:
                                    ConsoleHelper.WriteFullLine(login.Response, Variaveis.CinzaEscuro);
                                    Contas[Index].AdicionarChallenge();
                                    Challenges.Add(Contas[Index].Username);
                                    await Task.Delay(1548);
                                    break;
                                case 10:
                                    ConsoleHelper.WriteFullLine(login.Response, Variaveis.CinzaEscuro);
                                    Contas[Index].AdicionarChallenge();
                                    Challenges.Add(Contas[Index].Username);
                                    await Task.Delay(1548);
                                    break;
                                case 11:
                                    ConsoleHelper.WriteFullLine(login.Response, Variaveis.CianoEscuro);
                                    Contas[Index].AdicionarChallenge();
                                    Challenges.Add(Contas[Index].Username);
                                    await Task.Delay(1548);
                                    break;
                                case 12:
                                    ConsoleHelper.WriteFullLine(login.Response, Variaveis.Magenta);
                                    Contas[Index].AdicionarIncorrect();
                                    Incorrects.Add(Contas[Index].Username);
                                    await Task.Delay(1548);
                                    break;
                                default:
                                    ConsoleHelper.WriteFullLine(login.Response, Variaveis.CinzaEscuro);
                                    Contas[Index].AdicionarChallenge();
                                    Challenges.Add(Contas[Index].Username);
                                    await Task.Delay(1548);
                                    break;
                            }
                            ConsoleHelper.WriteFullLine("Indo para a próxima conta");
                            return;
                        }
                        else
                        {
                            InstaController[i].isLogged = true;
                            Contas[Index].RemoverChallenge();
                            Contas[Index].RemoverIncorrect();
                            ConsoleHelper.WriteFullLine("Login realizado com sucesso", Variaveis.Verde);
                        }
                    } else
                    {
                        ConsoleHelper.ClearConsole();
                        ConsoleHelper.WriteFullLine("Realizando login na conta do Instagram");
                        await Task.Delay(1478);
                        ConsoleHelper.WriteFullLine("Conta já estava conectada");
                    }
                }
                await Task.Delay(978);
                ConsoleHelper.ClearConsole();
                Show(i);
                int k = 0;
                bool sair = false;
                var r = new Random();
                while (k < Global.Quantidade && sair == false)
                {
                    ConsoleHelper.ClearConsole();
                    ConsoleHelper.WriteFullLine($"Buscando tarefa para realizar {k + 1}/{Global.Quantidade}");
                    Console.Title = $"@{Contas[Index].Username}: {k + 1}/{Global.Quantidade}";
                    var task = await Plat.GetTask(InstaController[i].ContaID);
                    if (task.Status != 1 && task.Status != 2)
                    {
                        int max = Global.Trocar ? 3 : 10;
                        int x = 0;
                        while (x < max && task.Status != 1 && task.Status != 2)
                        {
                            await Task.Delay(4589);
                            task = await Plat.GetTask(InstaController[i].ContaID);
                            x++;
                        }
                    }
                    if (task.Status == 1 || task.Status == 2 )
                    {
                        ConsoleHelper.WriteFullLine("Tarefa encontrada | " + task.Response);
                        if (task.Status == 1)
                        {
                            string alvo = "";
                            if (task.Json.link.ToString().IndexOf("instagram.com") > -1)
                            {
                                var array = task.Json.link.ToString().Split("/");
                                if (array[array.Length - 1] == "")
                                    alvo = array[array.Length - 2];
                                else
                                    alvo = array[array.Length - 1];
                            }
                            else
                            {
                                alvo = task.Json.link;
                            }
                            ConsoleHelper.WriteFullLine($"Seguindo o perfil '{alvo}'");
                            var seguir = await Seguir(alvo, InstaController[i].Web);
                            if (seguir.Status == 1 || seguir.Status == 0)
                            {
                                if (k == 0)
                                {
                                    InstaController[i].Insta.RemoverBlock();
                                }
                                ConsoleHelper.WriteFullLine("Sucesso ao seguir o perfil");
                                var confirm = await Plat.ConfirmTask(InstaController[i].ContaID, task.Json.id.ToString());
                                if (confirm.Status == 1)
                                {
                                    ConsoleHelper.WriteFullLine("Tarefa realizada com sucesso", Variaveis.Verde);
                                    await AddTarefa(0, i);
                                    k++;
                                }
                                else
                                {
                                    ConsoleHelper.WriteFullLine("Erro ao confirmar a tarefa na Dizu", Variaveis.Vermelho);
                                }
                            }
                            else
                            {
                                if (seguir.Status != 0 && seguir.Status != 3 && seguir.Status != 4)
                                {
                                    var check = await IsBlockOrChallenge(InstaController[i].Web);
                                    switch (check.Status)
                                    {
                                        case 2:
                                            ConsoleHelper.WriteFullLine(check.Response, Variaveis.Amarelo);
                                            Contas[Index].AdicionarBlock();
                                            Bloqueios.AdicionarBlock(Contas[Index].Username, Global.Timer_Block);
                                            await Task.Delay(1548);
                                            sair = true;
                                            break;
                                        case 5:
                                            ConsoleHelper.WriteFullLine(check.Response, Variaveis.VermelhoEscuro);
                                            Contas[Index].AdicionarChallenge();
                                            Challenges.Add(Contas[Index].Username);
                                            await Task.Delay(1548);
                                            sair = true;
                                            break;
                                        case 6:
                                            ConsoleHelper.WriteFullLine(check.Response, Variaveis.AzulEscuro);
                                            Contas[Index].AdicionarChallenge();
                                            Challenges.Add(Contas[Index].Username);
                                            await Task.Delay(1548);
                                            sair = true;
                                            break;
                                        case 7:
                                            ConsoleHelper.WriteFullLine(check.Response, Variaveis.Vermelho);
                                            Contas[Index].AdicionarChallenge();
                                            Challenges.Add(Contas[Index].Username);
                                            await Task.Delay(1548);
                                            sair = true;
                                            break;
                                        case 8:
                                            ConsoleHelper.WriteFullLine(check.Response, Variaveis.Ciano);
                                            Contas[Index].AdicionarChallenge();
                                            Challenges.Add(Contas[Index].Username);
                                            await Task.Delay(1548);
                                            sair = true;
                                            break;
                                        case 9:
                                            ConsoleHelper.WriteFullLine(check.Response, Variaveis.CinzaEscuro);
                                            Contas[Index].AdicionarChallenge();
                                            Challenges.Add(Contas[Index].Username);
                                            await Task.Delay(1548);
                                            sair = true;
                                            break;
                                        case 10:
                                            ConsoleHelper.WriteFullLine(check.Response, Variaveis.CinzaEscuro);
                                            Contas[Index].AdicionarChallenge();
                                            Challenges.Add(Contas[Index].Username);
                                            await Task.Delay(1548);
                                            sair = true;
                                            break;
                                        case 11:
                                            ConsoleHelper.WriteFullLine(check.Response, Variaveis.CianoEscuro);
                                            Contas[Index].AdicionarChallenge();
                                            Challenges.Add(Contas[Index].Username);
                                            await Task.Delay(1548);
                                            break;
                                        default:
                                            ConsoleHelper.WriteFullLine(check.Response, Variaveis.CinzaEscuro);
                                            Contas[Index].AdicionarChallenge();
                                            Challenges.Add(Contas[Index].Username);
                                            await Task.Delay(1548);
                                            sair = true;
                                            break;
                                    }
                                }
                                else
                                {
                                    ConsoleHelper.WriteFullLine(seguir.Response, Variaveis.Azul);
                                    var pular = await Plat.PularTask(InstaController[i].ContaID, task.Json.id.ToString());
                                    if (pular.Status == 1)
                                        ConsoleHelper.WriteFullLine("Tarefa pulada");
                                    else
                                        ConsoleHelper.WriteFullLine("Erro ao pular a tarefa");
                                }
                            }
                        } else
                        {
                            string link = "";
                            if (task.Json.link.ToString().IndexOf("instagram.com") > -1)
                            {
                                var array = task.Json.link.ToString().Split("/");
                                if (array[array.Length - 1] == "")
                                    link = array[array.Length - 2];
                                else
                                    link = array[array.Length - 1];
                            }
                            else
                            {
                                link = task.Json.link;
                            }
                            ConsoleHelper.WriteFullLine($"Curtindo a publicação '{link}'");
                            var seguir = await Curtir(link, InstaController[i].Web);
                            if (seguir.Status == 1)
                            {
                                if (k == 0)
                                {
                                    InstaController[i].Insta.RemoverBlock();
                                }
                                ConsoleHelper.WriteFullLine("Sucesso ao curtir a publicação");
                                var confirm = await Plat.ConfirmTask(InstaController[i].ContaID, task.Json.id.ToString());
                                if (confirm.Status == 1)
                                {
                                    ConsoleHelper.WriteFullLine("Tarefa realizada com sucesso", Variaveis.Verde);
                                    await AddTarefa(1, i);
                                    k++;
                                }
                                else
                                {
                                    ConsoleHelper.WriteFullLine("Erro ao confirmar a tarefa na Dizu", Variaveis.Vermelho);
                                }
                            }
                            else
                            {
                                if (seguir.Status != 0 && seguir.Status != 3 && seguir.Status != 4)
                                {
                                    var check = await IsBlockOrChallenge(InstaController[i].Web);
                                    switch (check.Status)
                                    {
                                        case 2:
                                            ConsoleHelper.WriteFullLine(check.Response, Variaveis.Amarelo);
                                            Contas[Index].AdicionarBlock();
                                            Bloqueios.AdicionarBlock(Contas[Index].Username, Global.Timer_Block);
                                            await Task.Delay(1548);
                                            sair = true;
                                            break;
                                        case 5:
                                            ConsoleHelper.WriteFullLine(check.Response, Variaveis.VermelhoEscuro);
                                            Contas[Index].AdicionarChallenge();
                                            Challenges.Add(Contas[Index].Username);
                                            await Task.Delay(1548);
                                            sair = true;
                                            break;
                                        case 6:
                                            ConsoleHelper.WriteFullLine(check.Response, Variaveis.AzulEscuro);
                                            Contas[Index].AdicionarChallenge();
                                            Challenges.Add(Contas[Index].Username);
                                            await Task.Delay(1548);
                                            sair = true;
                                            break;
                                        case 7:
                                            ConsoleHelper.WriteFullLine(check.Response, Variaveis.Vermelho);
                                            Contas[Index].AdicionarChallenge();
                                            Challenges.Add(Contas[Index].Username);
                                            await Task.Delay(1548);
                                            sair = true;
                                            break;
                                        case 8:
                                            ConsoleHelper.WriteFullLine(check.Response, Variaveis.Ciano);
                                            Contas[Index].AdicionarChallenge();
                                            Challenges.Add(Contas[Index].Username);
                                            await Task.Delay(1548);
                                            sair = true;
                                            break;
                                        case 9:
                                            ConsoleHelper.WriteFullLine(check.Response, Variaveis.CinzaEscuro);
                                            Contas[Index].AdicionarChallenge();
                                            Challenges.Add(Contas[Index].Username);
                                            await Task.Delay(1548);
                                            sair = true;
                                            break;
                                        case 10:
                                            ConsoleHelper.WriteFullLine(check.Response, Variaveis.CinzaEscuro);
                                            Contas[Index].AdicionarChallenge();
                                            Challenges.Add(Contas[Index].Username);
                                            await Task.Delay(1548);
                                            sair = true;
                                            break;
                                        default:
                                            ConsoleHelper.WriteFullLine(check.Response, Variaveis.CinzaEscuro);
                                            Contas[Index].AdicionarChallenge();
                                            Challenges.Add(Contas[Index].Username);
                                            await Task.Delay(1548);
                                            sair = true;
                                            break;
                                    }
                                }
                                else
                                {
                                    ConsoleHelper.WriteFullLine(seguir.Response, Variaveis.Azul);
                                    var pular = await Plat.PularTask(InstaController[i].ContaID, task.Json.id.ToString());
                                    if (pular.Status == 1)
                                        ConsoleHelper.WriteFullLine("Tarefa pulada");
                                    else
                                        ConsoleHelper.WriteFullLine("Erro ao pular a tarefa");
                                }
                            }
                        }
                    } else
                    {
                        ConsoleHelper.WriteFullLine("Não foi possivel localizar tarefa no momento");
                        if (Global.Trocar && Contas.Count > 1)
                        {
                            sair = true;
                        }
                    }
                    if (sair == false)
                    {
                        if (task.Status == 1 || task.Status == 2)
                        {
                            var delay = r.Next(Convert.ToInt32(Global.Delay1), Convert.ToInt32(Global.Delay2));
                            ConsoleHelper.WriteFullLine($"Aguardando {delay} segundos para continuar", Variaveis.Ciano);
                            await Task.Delay(TimeSpan.FromSeconds(delay));
                        }
                        else
                        {
                            ConsoleHelper.WriteFullLine($"Aguardando {3} segundos para continuar", Variaveis.Ciano);
                            await Task.Delay(TimeSpan.FromSeconds(3));
                        }
                    }
                }
                ConsoleHelper.WriteFullLine($"Indo para a proxima conta");
                return;
            } catch (Exception err)
            {
                LogMessage($"Erro RodarConta: {err.Message}");
                ConsoleHelper.WriteFullLine("Erro ao rodar o bot, entre em contato com o suporte", Variaveis.Vermelho);
                return;
            }
        }

        #region Instagram
        /// <summary>
        /// Quando a conta levar feedback_required valida se ela só tomou Feedback ou Challenge
        /// </summary>
        /// <param name="Insta">Conta para verificar</param>
        /// <returns>2 = Block temporario / 5 = Block de SMS / 6 = Block Troca de senha / 7 = Email/SMS / 8 = Bloqueio de foto / 9 = Nao configurado / 10 = Erro ao puxar o challenge / 11 = Challenge Selfie</returns>
        static async Task<Models.ActionModels.Result> IsBlockOrChallenge(InstagramWeb Insta)
        {
            Models.ActionModels.Result Ret = new Models.ActionModels.Result
            {
                Response = "",
                Status = 2
            };
            try
            {
                var profile = await Insta.GetMyProfileAsync();
                if (profile.Status == 1)
                {
                    Ret.Status = 2;
                    Ret.Response = "Bloqueio: Temporario";
                }
                else
                {
                    if (Ret.Status == -2)
                    {
                        var challenge = await Insta.GetChallengeRequestByChallengeUrlAsync();
                        if (challenge.Status == 1)
                        {
                            switch (challenge.Response)
                            {
                                case "ReviewLoginForm":
                                    Ret.Status = 7;
                                    Ret.Response = "Bloqueio: Email/SMS";
                                    return Ret;
                                case "SelfieCaptchaChallengeForm":
                                    Ret.Status = 11;
                                    Ret.Response = "Bloqueio: Selfie";
                                    return Ret;
                                case "SelectContactPointRecoveryForm":
                                    Ret.Status = 7;
                                    Ret.Response = "Bloqueio: Email/SMS";
                                    return Ret;
                                case "RecaptchaChallengeForm":
                                    Ret.Status = 5;
                                    Ret.Response = "Bloqueio: Recaptcha e SMS";
                                    return Ret;
                                case "IeForceSetNewPasswordForm":
                                    Ret.Status = 6;
                                    Ret.Response = "Bloqueio: Troca de senha";
                                    return Ret;
                                case "SubmitPhoneNumberForm":
                                    Ret.Status = 5;
                                    Ret.Response = "Bloqueio: SMS";
                                    return Ret;
                                case "EscalationChallengeInformationalForm":
                                    var res = await Insta.ReplyChallengeByChoiceAsync("0");
                                    if (res.Satus == 1)
                                    {
                                        Ret.Status = 0;
                                        Ret.Response = "Bloqueio: Foto | Ja resolvido";
                                        return Ret;
                                    }
                                    else
                                    {
                                        LogMessage(res.Response);
                                        Ret.Status = 8;
                                        Ret.Response = "Bloqueio: Foto";
                                    }
                                    return Ret;
                                case "SelectVerificationMethodForm":
                                    Ret.Status = 7;
                                    Ret.Response = "Bloqueio: Email/SMS";
                                    return Ret;
                                default:
                                    LogMessage($"Tipo de bloqueio não detectado: {challenge.Response}");
                                    Ret.Status = 9;
                                    Ret.Response = "Bloqueio: " + challenge.Response;
                                    return Ret;
                            }
                        }
                        else
                        {
                            LogMessage(challenge.Response);
                            Ret.Status = 10;
                            Ret.Response = "Bloqueio: Erro ao detectar";
                            return Ret;
                        }
                    }
                    else
                    {
                        Ret.Status = -1;
                        Ret.Response = profile.Response;
                    }
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
        /// Realizar tarefa de seguir um perfil
        /// </summary>
        /// <param name="target">Username do Alvo</param>
        /// <param name="Insta">InstagramWeb logado na conta</param>
        /// <returns>-1 = Erro / 0 = Ja seguiu o perfil / 1 = Sucesso / 2 = Bloqueio temporario / 3 = Perfil não encontrado / 4 = Perfil Privado / 5 = Block de SMS / 6 = Block Troca de senha / 7 = Email/SMS / 8 = Bloqueio de foto / 9 = Nao configurado / 10 = Erro ao puxar o challenge / 11 = Challenge Selfie</returns>
        static async Task<Models.ActionModels.Result> Seguir(string target, InstagramWeb Insta, bool barra = false)
        {
            Models.ActionModels.Result Ret = new Models.ActionModels.Result
            {
                Response = "Erro ao realizar a tarefa",
                Status = -1
            };
            try
            {
                InstaResponse id = null;
                if (!barra)
                    id = await Insta.GetUserBySearchBarAsync(target.ToLower());
                else
                    id = await Insta.GetUserIdByUsernameAsync(target.ToLower());
                if (id.Satus == 1)
                {
                    var relation = await Insta.GetFriendshipRelationByUsernameAsync(target);
                    if (relation.Status == 1)
                    {
                        if (relation.Is_Complet)
                        {
                            if (!relation.Is_Private)
                            {
                                if (!relation.Is_Following)
                                {
                                    var seguir = await Insta.FollowUserByIdAsync(id.Response);
                                    if (seguir.Satus == 1)
                                    {
                                        Ret.Status = 1;
                                        Ret.Response = "Sucesso ao seguir o perdil";
                                        return Ret;
                                    }
                                    else
                                    {
                                        if (seguir.Satus != -2)
                                        {
                                            if (seguir.Satus == -3)
                                            {
                                                var check = await IsBlockOrChallenge(Insta);
                                                if (check.Status == 0)
                                                {
                                                    return await Seguir(target, Insta, barra);
                                                }
                                                else
                                                {
                                                    return check;
                                                }
                                            }
                                            else
                                            {
                                                Ret.Status = 3;
                                                Ret.Response = "Perfil não encontrado";
                                                return Ret;
                                            }
                                        }
                                        else
                                        {
                                            var challenge = await Insta.GetChallengeRequestAsync();
                                            if (challenge.Status == 1)
                                            {
                                                switch (challenge.Response)
                                                {
                                                    case "ReviewLoginForm":
                                                        Ret.Status = 7;
                                                        Ret.Response = "Bloqueio: Email/SMS";
                                                        return Ret;
                                                    case "SelfieCaptchaChallengeForm":
                                                        Ret.Status = 11;
                                                        Ret.Response = "Bloqueio: Selfie";
                                                        return Ret;
                                                    case "SelectContactPointRecoveryForm":
                                                        Ret.Status = 7;
                                                        Ret.Response = "Bloqueio: Email/SMS";
                                                        return Ret;
                                                    case "RecaptchaChallengeForm":
                                                        Ret.Status = 5;
                                                        Ret.Response = "Bloqueio: Recaptcha e SMS";
                                                        return Ret;
                                                    case "IeForceSetNewPasswordForm":
                                                        Ret.Status = 6;
                                                        Ret.Response = "Bloqueio: Troca de senha";
                                                        return Ret;
                                                    case "SubmitPhoneNumberForm":
                                                        Ret.Status = 5;
                                                        Ret.Response = "Bloqueio: SMS";
                                                        return Ret;
                                                    case "EscalationChallengeInformationalForm":
                                                        var res = await Insta.ReplyChallengeByChoiceAsync("0");
                                                        if (res.Satus == 1)
                                                            return await Seguir(target, Insta);
                                                        else
                                                        {
                                                            LogMessage(res.Response);
                                                            Ret.Status = 8;
                                                            Ret.Response = "Bloqueio: Foto";
                                                        }
                                                        return Ret;
                                                    case "SelectVerificationMethodForm":
                                                        Ret.Status = 7;
                                                        Ret.Response = "Bloqueio: Email/SMS";
                                                        return Ret;
                                                    default:
                                                        LogMessage($"Tipo de bloqueio não detectado: {challenge.Response}");
                                                        Ret.Status = 9;
                                                        Ret.Response = "Bloqueio: " + challenge.Response;
                                                        return Ret;
                                                }
                                            }
                                            else
                                            {
                                                LogMessage(challenge.Response);
                                                Ret.Status = 10;
                                                Ret.Response = "Bloqueio: Erro ao detectar";
                                                return Ret;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Ret.Status = 0;
                                    Ret.Response = "Ja segue o perfil";
                                    return Ret;
                                }
                            }
                            else
                            {
                                Ret.Status = 4;
                                Ret.Response = "Perfil privado";
                                return Ret;
                            }
                        }
                        else
                        {
                            Ret.Status = 3;
                            Ret.Response = "Perfil não encontrado";
                            return Ret;
                        }
                    }
                    else
                    {
                        if (relation.Status != -2)
                        {
                            if (relation.Status == -3)
                            {
                                var check = await IsBlockOrChallenge(Insta);
                                if (check.Status == 0)
                                {
                                    return await Seguir(target, Insta, barra);
                                }
                                else
                                {
                                    return check;
                                }
                            }
                            else
                            {
                                Ret.Status = 3;
                                Ret.Response = "Perfil não encontrado";
                                return Ret;
                            }
                        }
                        else
                        {
                            var challenge = await Insta.GetChallengeRequestAsync();
                            if (challenge.Status == 1)
                            {
                                switch (challenge.Response)
                                {
                                    case "ReviewLoginForm":
                                        Ret.Status = 7;
                                        Ret.Response = "Bloqueio: Email/SMS";
                                        return Ret;
                                    case "SelfieCaptchaChallengeForm":
                                        Ret.Status = 11;
                                        Ret.Response = "Bloqueio: Selfie";
                                        return Ret;
                                    case "SelectContactPointRecoveryForm":
                                        Ret.Status = 7;
                                        Ret.Response = "Bloqueio: Email/SMS";
                                        return Ret;
                                    case "RecaptchaChallengeForm":
                                        Ret.Status = 5;
                                        Ret.Response = "Bloqueio: Recaptcha e SMS";
                                        return Ret;
                                    case "IeForceSetNewPasswordForm":
                                        Ret.Status = 6;
                                        Ret.Response = "Bloqueio: Troca de senha";
                                        return Ret;
                                    case "SubmitPhoneNumberForm":
                                        Ret.Status = 5;
                                        Ret.Response = "Bloqueio: SMS";
                                        return Ret;
                                    case "EscalationChallengeInformationalForm":
                                        var res = await Insta.ReplyChallengeByChoiceAsync("0");
                                        if (res.Satus == 1)
                                            return await Seguir(target, Insta);
                                        else
                                        {
                                            LogMessage(res.Response);
                                            Ret.Status = 8;
                                            Ret.Response = "Bloqueio: Foto";
                                        }
                                        return Ret;
                                    case "SelectVerificationMethodForm":
                                        Ret.Status = 7;
                                        Ret.Response = "Bloqueio: Email/SMS";
                                        return Ret;
                                    default:
                                        LogMessage($"Tipo de bloqueio não detectado: {challenge.Response}");
                                        Ret.Status = 9;
                                        Ret.Response = "Bloqueio: " + challenge.Response;
                                        return Ret;
                                }
                            }
                            else
                            {
                                LogMessage(challenge.Response);
                                Ret.Status = 10;
                                Ret.Response = "Bloqueio: Erro ao detectar";
                                return Ret;
                            }
                        }
                    }
                }
                else
                {
                    if (id.Satus != -2)
                    {
                        if (id.Satus == -3)
                        {
                            var check = await IsBlockOrChallenge(Insta);
                            if (check.Status == 0)
                            {
                                return await Seguir(target, Insta, barra);
                            }
                            else
                            {
                                return check;
                            }
                        }
                        else
                        {
                            Ret.Status = 3;
                            Ret.Response = "Perfil não encontrado";
                            return Ret;
                        }
                    }
                    else
                    {
                        var challenge = await Insta.GetChallengeRequestAsync();
                        if (challenge.Status == 1)
                        {
                            switch (challenge.Response)
                            {
                                case "ReviewLoginForm":
                                    Ret.Status = 7;
                                    Ret.Response = "Bloqueio: Email/SMS";
                                    return Ret;
                                case "SelfieCaptchaChallengeForm":
                                    Ret.Status = 11;
                                    Ret.Response = "Bloqueio: Selfie";
                                    return Ret;
                                case "SelectContactPointRecoveryForm":
                                    Ret.Status = 7;
                                    Ret.Response = "Bloqueio: Email/SMS";
                                    return Ret;
                                case "RecaptchaChallengeForm":
                                    Ret.Status = 5;
                                    Ret.Response = "Bloqueio: Recaptcha e SMS";
                                    return Ret;
                                case "IeForceSetNewPasswordForm":
                                    Ret.Status = 6;
                                    Ret.Response = "Bloqueio: Troca de senha";
                                    return Ret;
                                case "SubmitPhoneNumberForm":
                                    Ret.Status = 5;
                                    Ret.Response = "Bloqueio: SMS";
                                    return Ret;
                                case "EscalationChallengeInformationalForm":
                                    var res = await Insta.ReplyChallengeByChoiceAsync("0");
                                    if (res.Satus == 1)
                                        return await Seguir(target, Insta);
                                    else
                                    {
                                        LogMessage(res.Response);
                                        Ret.Status = 8;
                                        Ret.Response = "Bloqueio: Foto";
                                    }
                                    return Ret;
                                case "SelectVerificationMethodForm":
                                    Ret.Status = 7;
                                    Ret.Response = "Bloqueio: Email/SMS";
                                    return Ret;
                                default:
                                    LogMessage($"Tipo de bloqueio não detectado: {challenge.Response}");
                                    Ret.Status = 9;
                                    Ret.Response = "Bloqueio: " + challenge.Response;
                                    return Ret;
                            }
                        }
                        else
                        {
                            LogMessage(challenge.Response);
                            Ret.Status = 10;
                            Ret.Response = "Bloqueio: Erro ao detectar";
                            return Ret;
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

        /// <summary>
        /// Curtir uma publicação pelo Shortcode
        /// </summary>
        /// <param name="target">Shortcode da publicação</param>
        /// <param name="Insta">Instagram</param>
        /// <returns>-1 = Erro / 0 = Ja curtiu a publicação / 1 = Sucesso / 2 = Bloqueio temporario / 3 = Publicação não encontrada / 4 = Publicação Privada / 5 = Block de SMS / 6 = Block Troca de senha / 7 = Email/SMS / 8 = Bloqueio de foto / 9 = Nao configurado / 10 = Erro ao puxar o challenge / 11 = Challenge Selfie</returns>
        static async Task<Models.ActionModels.Result> Curtir(string target, InstagramWeb Insta)
        {
            Models.ActionModels.Result Ret = new Models.ActionModels.Result
            {
                Response = "Erro ao curtir a publicação",
                Status = -1
            };
            try
            {
                var relation = await Insta.GetMediaRelationByShortcodeAsync(target);
                if (relation.Status == 1)
                {
                    if (relation.Is_Complet)
                    {
                        if (!relation.Is_Liked)
                        {
                            var curtir = await Insta.LikeMediaByIdAsync(relation.MediaID);
                            if (curtir.Satus == 1)
                            {
                                Ret.Status = 1;
                                Ret.Response = "Publicação curtida com sucesso";
                                return Ret;
                            }
                            else
                            {
                                if (curtir.Satus != -2)
                                {
                                    if (curtir.Satus == -3)
                                    {
                                        var check = await IsBlockOrChallenge(Insta);
                                        if (check.Status == 0)
                                        {
                                            return await Curtir(target, Insta);
                                        }
                                        else
                                        {
                                            return check;
                                        }
                                    }
                                    else
                                    {
                                        Ret.Status = 3;
                                        Ret.Response = "Publicação não encontrada";
                                        return Ret;
                                    }
                                }
                                else
                                {
                                    var challenge = await Insta.GetChallengeRequestAsync();
                                    if (challenge.Status == 1)
                                    {
                                        switch (challenge.Response)
                                        {
                                            case "ReviewLoginForm":
                                                Ret.Status = 7;
                                                Ret.Response = "Bloqueio: Email/SMS";
                                                return Ret;
                                            case "SelfieCaptchaChallengeForm":
                                                Ret.Status = 11;
                                                Ret.Response = "Bloqueio: Selfie";
                                                return Ret;
                                            case "SelectContactPointRecoveryForm":
                                                Ret.Status = 7;
                                                Ret.Response = "Bloqueio: Email/SMS";
                                                return Ret;
                                            case "RecaptchaChallengeForm":
                                                Ret.Status = 5;
                                                Ret.Response = "Bloqueio: Recaptcha e SMS";
                                                return Ret;
                                            case "IeForceSetNewPasswordForm":
                                                Ret.Status = 6;
                                                Ret.Response = "Bloqueio: Troca de senha";
                                                return Ret;
                                            case "SubmitPhoneNumberForm":
                                                Ret.Status = 5;
                                                Ret.Response = "Bloqueio: SMS";
                                                return Ret;
                                            case "EscalationChallengeInformationalForm":
                                                var res = await Insta.ReplyChallengeByChoiceAsync("0");
                                                if (res.Satus == 1)
                                                    return await Curtir(target, Insta);
                                                else
                                                {
                                                    LogMessage(res.Response);
                                                    Ret.Status = 8;
                                                    Ret.Response = "Bloqueio: Foto";
                                                }
                                                return Ret;
                                            case "SelectVerificationMethodForm":
                                                Ret.Status = 7;
                                                Ret.Response = "Bloqueio: Email/SMS";
                                                return Ret;
                                            default:
                                                LogMessage($"Tipo de bloqueio não detectado: {challenge.Response}");
                                                Ret.Status = 9;
                                                Ret.Response = "Bloqueio: " + challenge.Response;
                                                return Ret;
                                        }
                                    }
                                    else
                                    {
                                        LogMessage(challenge.Response);
                                        Ret.Status = 10;
                                        Ret.Response = "Bloqueio: Erro ao detectar";
                                        return Ret;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Ret.Status = 0;
                            Ret.Response = "Ja curtiu a publicação";
                            return Ret;
                        }
                    }
                    else
                    {
                        Ret.Status = 3;
                        Ret.Response = "Publicação não encontrada";
                        return Ret;
                    }
                }
                else
                {
                    if (relation.Status != -2)
                    {
                        if (relation.Status == -3)
                        {
                            var check = await IsBlockOrChallenge(Insta);
                            if (check.Status == 0)
                            {
                                return await Curtir(target, Insta);
                            }
                            else
                            {
                                return check;
                            }
                        }
                        else
                        {
                            Ret.Status = 3;
                            Ret.Response = "Publicação não encontrada";
                            return Ret;
                        }
                    }
                    else
                    {
                        var challenge = await Insta.GetChallengeRequestAsync();
                        if (challenge.Status == 1)
                        {
                            switch (challenge.Response)
                            {
                                case "ReviewLoginForm":
                                    Ret.Status = 7;
                                    Ret.Response = "Bloqueio: Email/SMS";
                                    return Ret;
                                case "SelfieCaptchaChallengeForm":
                                    Ret.Status = 11;
                                    Ret.Response = "Bloqueio: Selfie";
                                    return Ret;
                                case "SelectContactPointRecoveryForm":
                                    Ret.Status = 7;
                                    Ret.Response = "Bloqueio: Email/SMS";
                                    return Ret;
                                case "RecaptchaChallengeForm":
                                    Ret.Status = 5;
                                    Ret.Response = "Bloqueio: Recaptcha e SMS";
                                    return Ret;
                                case "IeForceSetNewPasswordForm":
                                    Ret.Status = 6;
                                    Ret.Response = "Bloqueio: Troca de senha";
                                    return Ret;
                                case "SubmitPhoneNumberForm":
                                    Ret.Status = 5;
                                    Ret.Response = "Bloqueio: SMS";
                                    return Ret;
                                case "EscalationChallengeInformationalForm":
                                    var res = await Insta.ReplyChallengeByChoiceAsync("0");
                                    if (res.Satus == 1)
                                        return await Curtir(target, Insta);
                                    else
                                    {
                                        LogMessage(res.Response);
                                        Ret.Status = 8;
                                        Ret.Response = "Bloqueio: Foto";
                                    }
                                    return Ret;
                                case "SelectVerificationMethodForm":
                                    Ret.Status = 7;
                                    Ret.Response = "Bloqueio: Email/SMS";
                                    return Ret;
                                default:
                                    LogMessage($"Tipo de bloqueio não detectado: {challenge.Response}");
                                    Ret.Status = 9;
                                    Ret.Response = "Bloqueio: " + challenge.Response;
                                    return Ret;
                            }
                        }
                        else
                        {
                            LogMessage(challenge.Response);
                            Ret.Status = 10;
                            Ret.Response = "Bloqueio: Erro ao detectar";
                            return Ret;
                        }
                    }
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
        /// Realizar login na conta do instagram
        /// </summary>
        /// <param name="Insta">InstagramWeb</param>
        /// <returns>-1 = Erro / 1 = Sucesso / 2 = Bloqueio temporario / 5 = Block de SMS / 6 = Block Troca de senha / 7 = Email/SMS / 8 = Bloqueio de foto / 9 = Nao configurado / 10 = Erro ao puxar o challenge / 11 = Challenge Selfie / 12 = Erro ao logar: Usuario ou seha inválido</returns>
        static async Task<Models.ActionModels.Result> Login(InstagramWeb Insta)
        {
            Models.ActionModels.Result Ret = new Models.ActionModels.Result
            {
                Response = "Erro ao realizar o login",
                Status = -1
            };
            try
            {
                var login = Insta.DoLogin();
                if (login.Satus == 1)
                {
                    await Task.Delay(1487);
                    var profile = await Insta.GetMyProfileAsync();
                    if (profile.Status == 1)
                    {
                        var logins = await Insta.GetSuspiciousLoginAsync();
                        if (logins.Json.Count > 0)
                        {
                            try
                            {
                                int max = logins.Json.Count >= 3 ? 3 : logins.Json.Count;
                                for (int j = 0; j < max; j++)
                                {
                                    string id = logins.Json[j].id;
                                    var confirm = await Insta.AllowSuspiciosLoginByIdAsync(id);
                                }
                            }
                            catch (Exception err)
                            {
                                LogMessage($"Erro ao verificar acesso: {err.Message}");
                            }
                        }
                        Ret.Status = 1;
                        Ret.Response = "Login realizado com sucesso";
                        return Ret;
                    }
                    else
                    {
                        if (profile.Status != -2)
                        {
                            if (profile.Status == -3)
                            {
                                var check = await IsBlockOrChallenge(Insta);
                                if (check.Status == 0)
                                {
                                    if (await IsLogged(Insta))
                                    {
                                        Ret.Status = 1;
                                        Ret.Response = "Login realizado com sucesso";
                                        return Ret;
                                    }
                                    else
                                    {
                                        return await Login(Insta);
                                    }
                                }
                                else
                                {
                                    return check;
                                }
                            }
                            else
                            {
                                Ret.Status = 12;
                                Ret.Response = "Login: Usuario ou Senha inválida";
                                return Ret;
                            }
                        }
                        else
                        {
                            var challenge = await Insta.GetChallengeRequestAsync();
                            if (challenge.Status == 1)
                            {
                                switch (challenge.Response)
                                {
                                    case "ReviewLoginForm":
                                        Ret.Status = 7;
                                        Ret.Response = "Bloqueio: Email/SMS";
                                        return Ret;
                                    case "SelfieCaptchaChallengeForm":
                                        Ret.Status = 11;
                                        Ret.Response = "Bloqueio: Selfie";
                                        return Ret;
                                    case "SelectContactPointRecoveryForm":
                                        Ret.Status = 7;
                                        Ret.Response = "Bloqueio: Email/SMS";
                                        return Ret;
                                    case "RecaptchaChallengeForm":
                                        Ret.Status = 5;
                                        Ret.Response = "Bloqueio: Recaptcha e SMS";
                                        return Ret;
                                    case "IeForceSetNewPasswordForm":
                                        Ret.Status = 6;
                                        Ret.Response = "Bloqueio: Troca de senha";
                                        return Ret;
                                    case "SubmitPhoneNumberForm":
                                        Ret.Status = 5;
                                        Ret.Response = "Bloqueio: SMS";
                                        return Ret;
                                    case "EscalationChallengeInformationalForm":
                                        var res = await Insta.ReplyChallengeByChoiceAsync("0");
                                        if (res.Satus == 1)
                                        {
                                            if (await IsLogged(Insta))
                                            {
                                                Ret.Status = 1;
                                                Ret.Response = "Login realizado com sucesso";
                                                return Ret;
                                            }
                                            else
                                            {
                                                return await Login(Insta);
                                            }
                                        }
                                        else
                                        {
                                            LogMessage(res.Response);
                                            Ret.Status = 8;
                                            Ret.Response = "Bloqueio: Foto";
                                        }
                                        return Ret;
                                    case "SelectVerificationMethodForm":
                                        Ret.Status = 7;
                                        Ret.Response = "Bloqueio: Email/SMS";
                                        return Ret;
                                    default:
                                        LogMessage($"Tipo de bloqueio não detectado: {challenge.Response}");
                                        Ret.Status = 9;
                                        Ret.Response = "Bloqueio: " + challenge.Response;
                                        return Ret;
                                }
                            }
                            else
                            {
                                LogMessage(challenge.Response);
                                Ret.Status = 10;
                                Ret.Response = "Bloqueio: Erro ao detectar";
                                return Ret;
                            }
                        }
                    }
                }
                else
                {
                    if (login.Satus != -2)
                    {
                        if (login.Satus == -3)
                        {
                            var check = await IsBlockOrChallenge(Insta);
                            if (check.Status == 0)
                            {
                                return await Login(Insta);
                            }
                            else
                            {
                                return check;
                            }
                        }
                        else
                        {
                            Ret.Status = 12;
                            Ret.Response = "Login: Usuario ou Senha inválida";
                            return Ret;
                        }
                    }
                    else
                    {
                        var challenge = await Insta.GetChallengeRequestByChallengeUrlAsync();
                        if (challenge.Status == 1)
                        {
                            switch (challenge.Response)
                            {
                                case "ReviewLoginForm":
                                    Ret.Status = 7;
                                    Ret.Response = "Bloqueio: Email/SMS";
                                    return Ret;
                                case "SelfieCaptchaChallengeForm":
                                    Ret.Status = 11;
                                    Ret.Response = "Bloqueio: Selfie";
                                    return Ret;
                                case "SelectContactPointRecoveryForm":
                                    Ret.Status = 7;
                                    Ret.Response = "Bloqueio: Email/SMS";
                                    return Ret;
                                case "RecaptchaChallengeForm":
                                    Ret.Status = 5;
                                    Ret.Response = "Bloqueio: Recaptcha e SMS";
                                    return Ret;
                                case "IeForceSetNewPasswordForm":
                                    Ret.Status = 6;
                                    Ret.Response = "Bloqueio: Troca de senha";
                                    return Ret;
                                case "SubmitPhoneNumberForm":
                                    Ret.Status = 5;
                                    Ret.Response = "Bloqueio: SMS";
                                    return Ret;
                                case "EscalationChallengeInformationalForm":
                                    var res = await Insta.ReplyChallengeByChoiceAsync("0");
                                    if (res.Satus == 1)
                                        return await Login(Insta);
                                    else
                                    {
                                        LogMessage(res.Response);
                                        Ret.Status = 8;
                                        Ret.Response = "Bloqueio: Foto";
                                    }
                                    return Ret;
                                case "SelectVerificationMethodForm":
                                    Ret.Status = 7;
                                    Ret.Response = "Bloqueio: Email/SMS";
                                    return Ret;
                                default:
                                    LogMessage($"Tipo de bloqueio não detectado: {challenge.Response}");
                                    Ret.Status = 9;
                                    Ret.Response = "Bloqueio: " + challenge.Response;
                                    return Ret;
                            }
                        }
                        else
                        {
                            LogMessage(challenge.Response);
                            Ret.Status = 10;
                            Ret.Response = "Bloqueio: Erro ao detectar";
                            return Ret;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                LogMessage($"Erro login insta: {err.Message}");
            }
            return Ret;
        }

        /// <summary>
        /// Checa se uma conta está conectada
        /// </summary>
        /// <param name="Insta">Instagram</param>
        /// <returns>True caso sim e False caso não</returns>
        static async Task<bool> IsLogged(InstagramWeb Insta)
        {
            try
            {
                var profile = await Insta.GetMyProfileAsync();
                if (profile.Status == 1)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion
        #region Funções
        static private string DateString()
        {
            var data = DateTime.Today;
            var dia = data.Day.ToString();
            var mes = data.Month.ToString();
            var ano = data.Year.ToString();
            return $"{dia}-{mes}-{ano}";
        }

        static private string HorarioString()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        static private void LogMessage(string message)
        {
            try
            {
                var dir = Directory.GetCurrentDirectory();
                if (Directory.Exists($@"{dir}\logs"))
                {
                    var data = DateString();
                    if (File.Exists($@"{dir}\logs\{data}.txt"))
                    {
                        string[] linhas = File.ReadAllLines($@"{dir}\logs\{data}.txt");
                        var list = linhas.ToList();
                        list.Add($"DIZUCMD {HorarioString()} {message}");
                        File.WriteAllLines($@"{dir}\logs\{data}.txt", list);
                        return;
                    }
                    else
                    {
                        string[] linhas = { $"DIZUCMD {HorarioString()} {message}" };
                        File.WriteAllLines($@"{dir}\logs\{data}.txt", linhas);
                        return;
                    }
                }
                else
                {
                    Directory.CreateDirectory($@"{dir}\logs");
                    var data = DateString();
                    if (File.Exists($@"{dir}\logs\{data}.txt"))
                    {
                        string[] linhas = File.ReadAllLines($@"{dir}\logs\{data}.txt");
                        var list = linhas.ToList();
                        list.Add($"DIZUCMD {HorarioString()} {message}");
                        File.WriteAllLines($@"{dir}\logs\{data}.txt", list);
                        return;
                    }
                    else
                    {
                        string[] linhas = { $"DIZUCMD {HorarioString()} {message}" };
                        File.WriteAllLines($@"{dir}\logs\{data}.txt", linhas);
                        return;
                    }
                }
            }
            catch { }
        }

        static private async Task AddTarefa(int type, int i)
        {
            Variaveis.Total++;
            Variaveis.TotalMeta++;
            if (type == 0)
            {
                Contas[Index].Seguir++;
                Contas[Index].AdicionarSeguir();
                InstaController[i].Seguir++;
                InstaController[i].Total++;
                Variaveis.TotalContaAtual = InstaController[i].Total;
                Variaveis.TotalSeguir = InstaController[i].Seguir;
            }
            else
            {
                Contas[Index].Curtir++;
                Contas[Index].AdicionarCurtir();
                InstaController[i].Curtir++;
                InstaController[i].Total++;
                Variaveis.TotalContaAtual = InstaController[i].Total;
                Variaveis.TotalCurtir = InstaController[i].Curtir;
            }
            if (Variaveis.TotalMeta >= Variaveis.Meta)
            {
                Variaveis.MetaAlcancada++;
                ConsoleHelper.ClearConsole();
                ConsoleHelper.WriteFullLine("Meta de tarefas alcançada.", Variaveis.VerdeEscuro);
                ConsoleHelper.WriteFullLine($"Aguardando {Global.Timer_Meta} minutos para continuar", Variaveis.Ciano);
                Console.Title = $"Grupo: {Variaveis.Grupo} - Meta alcançada, aguardando para continuar";
                await Task.Delay(TimeSpan.FromMinutes(Global.Timer_Meta));
                Variaveis.TotalMeta = 0;
            }
        }

        static private void Show(int i)
        {
            Variaveis.TotalSeguir = InstaController[i].Seguir;
            Variaveis.TotalCurtir = InstaController[i].Curtir;
            Variaveis.TotalContaAtual = InstaController[i].Total;
            Variaveis.Conta_Atual = InstaController[i].Insta.Username;
        }

        #endregion
    }
}

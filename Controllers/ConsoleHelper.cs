using System;

namespace DIZUCMD.Controllers.HelpInstaAPI
{

/*
           8888b.   888d888  888  888   8888b.
              "88b  888P"    888 .88P      "88b 
          .d888888  888      888888K   .d888888 
          888  888  888      888 "88b  888  888 
          "Y888888  888      888  888  "Y888888
 */

    public static class ConsoleHelper
    {
        public static void WriteFullLine(string value, ConsoleColor color = ConsoleColor.White)
        {
            //
            // This method writes an entire line to the console with the string.
            //
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void ClearConsole(ConsoleColor color = ConsoleColor.Magenta)
        {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = color;
            Console.Write("           8888b.   888d888  888  888   8888b.\n              \"88b  888P\"    888 .88P      \"88b \n          .d888888  888      888888K   .d888888 \n          888  888  888      888 \"88b  888  888 \n          \"Y888888  888      888  888  \"Y888888");
            Console.WriteLine();
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("==============================================================");
            Console.WriteLine();
            Console.WriteLine($"  Grupo: {Variaveis.Grupo}\n  Conta Atual: {Variaveis.Conta_Atual}\n  Total: {Variaveis.Total}\n  Total Conta: {Variaveis.TotalContaAtual}  Seguir: {Variaveis.TotalSeguir}  Curtir: {Variaveis.TotalCurtir}\n  Meta: {Variaveis.TotalMeta}\\{Variaveis.Meta}  Alcançadas: {Variaveis.MetaAlcancada}");
            Console.WriteLine();
            Console.WriteLine("==============================================================");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
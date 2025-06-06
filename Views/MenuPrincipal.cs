using System;

namespace GS_CSHARP.Views
{
    public static class MenuPrincipal
    {
        public static void Exibir()
        {
            Console.WriteLine("======================================");
            Console.WriteLine(" Global Solution – Monitoramento de Falhas de Energia ");
            Console.WriteLine("======================================");
            Console.WriteLine("1) Registrar Falha");
            Console.WriteLine("2) Gerar Relatório");
            Console.WriteLine("3) Simular Erro");
            Console.WriteLine("4) Sair");
            Console.Write("Escolha uma opção (1-4): ");
        }
    }
}

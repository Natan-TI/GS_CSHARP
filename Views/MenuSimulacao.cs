using System;

namespace GS_CSHARP.Views
{
    public static class MenuSimulacao
    {
        public static void ExibirOpcoes()
        {
            Console.WriteLine("=== Simulação de Erros ===");
            Console.WriteLine("1) Simular data inválida");
            Console.WriteLine("2) Simular campo vazio");
            Console.WriteLine("3) Simular número inválido");
            Console.WriteLine("4) Voltar ao menu principal");
            Console.Write("Escolha uma opção (1-4): ");
        }
    }
}

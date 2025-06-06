using System;

namespace GS_CSHARP.Views
{
    public static class MenuRelatorio
    {
        public static void CapturarPeriodos(out string dataInicio, out string horaInicio, out string dataFim, out string horaFim, out string caminhoArquivo)
        {
            Console.Write("Data Início (dd/MM/yyyy): ");
            dataInicio = Console.ReadLine();
            Console.Write("Hora Início (HH:mm): ");
            horaInicio = Console.ReadLine();
            Console.Write("Data Fim (dd/MM/yyyy): ");
            dataFim = Console.ReadLine();
            Console.Write("Hora Fim (HH:mm): ");
            horaFim = Console.ReadLine();
            Console.Write("Caminho do arquivo de relatório (ex: reports/relatorio.csv): ");
            caminhoArquivo = Console.ReadLine();
        }
    }
}

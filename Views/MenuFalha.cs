using System;

namespace GS_CSHARP.Views
{
    public static class MenuFalha
    {
        public static void CapturarDadosFalha(out string data, out string hora, out string sensor, out string tensao)
        {
            Console.Write("Data (dd/MM/yyyy): ");
            data = Console.ReadLine();
            Console.Write("Hora (HH:mm): ");
            hora = Console.ReadLine();
            Console.Write("Sensor: ");
            sensor = Console.ReadLine();
            Console.Write("Tensão (por ex., 230.5): ");
            tensao = Console.ReadLine();
        }
    }
}

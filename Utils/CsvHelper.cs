using System;
using System.IO;
using System.Collections.Generic;

namespace GS_CSHARP.Utils
{
    public static class CsvHelper
    {
        public static void CriarCabecalhoFalhas(string caminho)
        {
            if (!File.Exists(caminho) || new FileInfo(caminho).Length == 0)
            {
                using (var sw = new StreamWriter(caminho, append: false))
                {
                    sw.WriteLine("Id;DataHora;Sensor;Tensao");
                }
            }
        }

        public static void CriarCabecalhoAlertas(string caminho)
        {
            if (!File.Exists(caminho) || new FileInfo(caminho).Length == 0)
            {
                using (var sw = new StreamWriter(caminho, append: false))
                {
                    sw.WriteLine("Id;DataHoraGeracao;Nivel;Mensagem");
                }
            }
        }

        public static void CriarCabecalhoLogs(string caminho)
        {
            if (!File.Exists(caminho) || new FileInfo(caminho).Length == 0)
            {
                File.Create(caminho).Close();
            }
        }
    }
}

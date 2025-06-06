using GS_CSHARP.Models;
using System;

namespace GS_CSHARP.Services
{
    public class SimuladorErros
    {
        private readonly LogService _logService;

        public SimuladorErros(LogService logService)
        {
            _logService = logService;
        }

        public void ForcarErroData(string inputData)
        {
            try
            {
                DateTime.ParseExact(inputData, "dd/MM/yyyy", null);
            }
            catch (FormatException fe)
            {
                Console.WriteLine($"[Simulação] Data inválida: {fe.Message}");
                _logService.RegistrarEvento(TipoEvento.SimulacaoErro, $"Data inválida forçada: {inputData}");
            }
        }

        public void ForcarErroCampoVazio(string texto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(texto))
                    throw new ArgumentException("Campo vazio forçado.");
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine($"[Simulação] Erro de campo vazio: {ae.Message}");
                _logService.RegistrarEvento(TipoEvento.SimulacaoErro, $"Campo vazio simulado.");
            }
        }

        public void ForcarErroNumero(string inputNumero)
        {
            try
            {
                int.Parse(inputNumero);
            }
            catch (FormatException fe)
            {
                Console.WriteLine($"[Simulação] Número inválido: {fe.Message}");
                _logService.RegistrarEvento(TipoEvento.SimulacaoErro, $"Número inválido simulado: {inputNumero}");
            }
        }
    }
}

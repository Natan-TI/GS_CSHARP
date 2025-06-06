using GS_CSHARP.Models;
using GS_CSHARP.Repositories;
using System;
using System.Collections.Generic;

namespace GS_CSHARP.Services
{
    public class FalhaService
    {
        private readonly IRepositorio<FalhaEnergia> _repoFalha;
        private readonly AlertaService _alertaService;
        private readonly LogService _logService;
        private int _proximoIdFalha = 1;

        public FalhaService(IRepositorio<FalhaEnergia> repoFalha, AlertaService alertaService, LogService logService)
        {
            _repoFalha = repoFalha;
            _alertaService = alertaService;
            _logService = logService;
            var todas = _repoFalha.ObterTodos();
            if (todas.Count > 0)
                _proximoIdFalha = todas[todas.Count - 1].Id + 1;
        }

        public void RegistrarFalha(string inputData, string inputHora, string sensor, string inputTensao)
        {
            try
            {
                DateTime dataHora = DateTime.ParseExact($"{inputData} {inputHora}", "dd/MM/yyyy HH:mm", null);
                decimal tensao = decimal.Parse(inputTensao);

                var falha = new FalhaEnergia(_proximoIdFalha++, dataHora, sensor, tensao);
                _repoFalha.Adicionar(falha);
                _logService.RegistrarEvento(TipoEvento.CadastroFalha, $"Falha cadastrada: ID={falha.Id}, Sensor={sensor}, Tensao={tensao}");

                _alertaService.VerificarLimite(falha);

                Console.WriteLine("Falha registrada com sucesso.");
            }
            catch (FormatException fe)
            {
                Console.WriteLine($"Erro de formato (data ou tensão inválida): {fe.Message}");
                _logService.RegistrarEvento(TipoEvento.ErroValidacao, $"Falha ao converter entrada: {fe.Message}");
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine($"Erro de validação: {ae.Message}");
                _logService.RegistrarEvento(TipoEvento.ErroValidacao, $"Falha de validação: {ae.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado ao registrar falha: {ex.Message}");
                _logService.RegistrarEvento(TipoEvento.ErroValidacao, $"Erro inesperado: {ex.Message}");
            }
        }

        public List<FalhaEnergia> ObterTodasFalhas()
        {
            return _repoFalha.ObterTodos();
        }

        public List<FalhaEnergia> ObterFalhasPorPeriodo(DateTime inicio, DateTime fim)
        {
            var todas = _repoFalha.ObterTodos();
            return todas.FindAll(f => f.DataHora >= inicio && f.DataHora <= fim);
        }

        public void GerarRelatorioFalhas(string caminhoRelatorio, DateTime inicio, DateTime fim)
        {
            try
            {
                var filtradas = ObterFalhasPorPeriodo(inicio, fim);
                using (var sw = new StreamWriter(caminhoRelatorio, append: false))
                {
                    sw.WriteLine("Id;DataHora;Sensor;Tensao");
                    foreach (var f in filtradas)
                        sw.WriteLine(f.ToCsv());
                }
                _logService.RegistrarEvento(TipoEvento.GerarRelatorio, $"Relatório de falhas gerado: {caminhoRelatorio}");
                Console.WriteLine($"Relatório gerado em {caminhoRelatorio}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao gerar relatório: {ex.Message}");
                _logService.RegistrarEvento(TipoEvento.ErroValidacao, $"Erro relatório: {ex.Message}");
            }
        }
    }
}

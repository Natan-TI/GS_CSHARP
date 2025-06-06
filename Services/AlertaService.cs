using GS_CSHARP.Models;
using GS_CSHARP.Repositories;
using System;
using System.Collections.Generic;

namespace GS_CSHARP.Services
{
    public class AlertaService
    {
        private readonly IRepositorio<Alerta> _repoAlerta;
        private readonly LogService _logService;
        private int _proximoIdAlerta = 1;

        public AlertaService(IRepositorio<Alerta> repoAlerta, LogService logService)
        {
            _repoAlerta = repoAlerta;
            _logService = logService;
            var todos = _repoAlerta.ObterTodos();
            if (todos.Count > 0)
                _proximoIdAlerta = todos[todos.Count - 1].Id + 1;
        }

        public void VerificarLimite(FalhaEnergia falha)
        {
            try
            {
                if (falha.Tensao < 110m)
                {
                    GerarAlerta(NivelAlerta.Critico, $"Tensão abaixo do limite: {falha.Tensao}V no sensor {falha.Sensor}");
                }
                else if (falha.Tensao > 240m)
                {
                    GerarAlerta(NivelAlerta.Critico, $"Tensão acima do limite: {falha.Tensao}V no sensor {falha.Sensor}");
                }
                else if (falha.Tensao <= 115m || falha.Tensao >= 235m)
                {
                    GerarAlerta(NivelAlerta.Aviso, $"Tensão próxima do limite: {falha.Tensao}V no sensor {falha.Sensor}");
                }
            }
            catch (Exception ex)
            {
                _logService.RegistrarEvento(TipoEvento.ErroValidacao, $"Erro ao verificar limite de tensão: {ex.Message}");
            }
        }

        private void GerarAlerta(NivelAlerta nivel, string mensagem)
        {
            var alerta = new Alerta(_proximoIdAlerta++, nivel, mensagem);
            _repoAlerta.Adicionar(alerta);
            _logService.RegistrarEvento(TipoEvento.GeraAlerta, mensagem);
            Console.WriteLine($"*** ALERTA ({nivel}): {mensagem}");
        }

        public List<Alerta> ObterTodosAlertas()
        {
            return _repoAlerta.ObterTodos();
        }
    }
}

using GS_CSHARP.Models;
using GS_CSHARP.Repositories;
using System;
using System.Collections.Generic;

namespace GS_CSHARP.Services
{
    public class LogService
    {
        private readonly IRepositorio<LogEvento> _repoLog;

        public LogService(IRepositorio<LogEvento> repoLog)
        {
            _repoLog = repoLog;
        }

        public void RegistrarEvento(TipoEvento tipo, string descricao)
        {
            try
            {
                var evento = new LogEvento(tipo, descricao);
                _repoLog.Adicionar(evento);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha ao registrar log: {ex.Message}");
            }
        }

        public List<LogEvento> ObterTodosLogs()
        {
            return _repoLog.ObterTodos();
        }
    }
}

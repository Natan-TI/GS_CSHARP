using System;

namespace GS_CSHARP.Models
{
    public abstract class Evento
    {
        public DateTime Timestamp { get; set; }
        public TipoEvento Tipo { get; set; }
        public string Descricao { get; set; }

        protected Evento(TipoEvento tipo, string descricao)
        {
            Timestamp = DateTime.Now;
            Tipo = tipo;
            Descricao = descricao;
        }
    }

    public class LogEvento : Evento
    {
        public LogEvento(TipoEvento tipo, string descricao)
            : base(tipo, descricao)
        {
        }

        public override string ToString()
        {
            return $"{Timestamp:yyyy-MM-dd HH:mm:ss} | {Tipo} | {Descricao}";
        }
    }
}

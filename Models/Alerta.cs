using GS_CSHARP.Models;
using System;

namespace GS_CSHARP.Models
{
    public class Alerta
    {
        public int Id { get; set; }
        public DateTime DataHoraGeracao { get; private set; }
        public NivelAlerta Nivel { get; private set; }
        public string Mensagem { get; private set; }

        public Alerta(int id, NivelAlerta nivel, string mensagem)
        {
            Id = id;
            DataHoraGeracao = DateTime.Now;
            Nivel = nivel;
            Mensagem = mensagem;
        }

        public override string ToString()
        {
            return $"{DataHoraGeracao:yyyy-MM-dd HH:mm:ss} | {Nivel} | {Mensagem}";
        }

        public string ToCsv()
        {
            return $"{Id};{DataHoraGeracao:yyyy-MM-dd HH:mm:ss};{Nivel};{Mensagem}";
        }
    }
}

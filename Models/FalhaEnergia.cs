using System;

namespace GS_CSHARP.Models
{
    public class FalhaEnergia
    {
        public int Id { get; set; }
        public DateTime DataHora { get; private set; }
        public string Sensor { get; private set; }
        public decimal Tensao { get; private set; }

        public FalhaEnergia(int id, DateTime dataHora, string sensor, decimal tensao)
        {
            Id = id;
            DefinirDataHora(dataHora);
            DefinirSensor(sensor);
            DefinirTensao(tensao);
        }

        private void DefinirDataHora(DateTime dataHora)
        {
            if (dataHora < new DateTime(2020, 1, 1) || dataHora > DateTime.Now.AddDays(7))
                throw new ArgumentException("DataHora fora do intervalo permitido.");
            DataHora = dataHora;
        }

        private void DefinirSensor(string sensor)
        {
            if (string.IsNullOrWhiteSpace(sensor))
                throw new ArgumentException("Sensor não pode ser vazio.");
            Sensor = sensor.Trim();
        }

        private void DefinirTensao(decimal tensao)
        {
            if (tensao <= 0)
                throw new ArgumentException("Tensão deve ser maior que zero.");
            Tensao = tensao;
        }

        public string ToCsv()
        {
            return $"{Id};{DataHora:yyyy-MM-dd HH:mm};{Sensor};{Tensao}";
        }
    }
}

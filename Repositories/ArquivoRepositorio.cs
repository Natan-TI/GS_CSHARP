using System;
using System.Collections.Generic;
using System.IO;

namespace GS_CSHARP.Repositories
{
    public class ArquivoRepositorio<T> : IRepositorio<T> where T : class
    {
        private readonly string _caminhoArquivo;
        private readonly Func<string, T> _parseLinha;      
        private readonly Func<T, string> _toCsv;       

        public ArquivoRepositorio(string caminhoArquivo, Func<string, T> parseLinha, Func<T, string> toCsv)
        {
            _caminhoArquivo = caminhoArquivo;
            _parseLinha = parseLinha;
            _toCsv = toCsv;

            var pasta = Path.GetDirectoryName(caminhoArquivo);
            if (!Directory.Exists(pasta))
                Directory.CreateDirectory(pasta);

            if (!File.Exists(_caminhoArquivo))
                File.Create(_caminhoArquivo).Close();
        }

        public void Adicionar(T entidade)
        {
            string linha = _toCsv(entidade);
            using (var sw = new StreamWriter(_caminhoArquivo, append: true))
            {
                sw.WriteLine(linha);
            }
        }

        public List<T> ObterTodos()
        {
            var lista = new List<T>();
            foreach (var linha in File.ReadAllLines(_caminhoArquivo))
            {
                if (string.IsNullOrWhiteSpace(linha)) continue;
                T objeto = _parseLinha(linha);
                lista.Add(objeto);
            }
            return lista;
        }

        public T ObterPorId(int id)
        {
            foreach (var linha in File.ReadAllLines(_caminhoArquivo))
            {
                if (string.IsNullOrWhiteSpace(linha)) continue;
                T objeto = _parseLinha(linha);
                var prop = typeof(T).GetProperty("Id");
                if (prop != null)
                {
                    int valorId = Convert.ToInt32(prop.GetValue(objeto));
                    if (valorId == id)
                        return objeto;
                }
            }
            return null;
        }
    }
}

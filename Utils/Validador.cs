using System;

namespace GS_CSHARP.Utils
{
    public static class Validador
    {
        public static bool ValidarData(string input)
        {
            return DateTime.TryParseExact(input, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out _);
        }

        public static bool ValidarHora(string input)
        {
            return TimeSpan.TryParse(input, out _);
        }

        public static bool ValidarDecimal(string input)
        {
            return decimal.TryParse(input, out _);
        }
    }
}

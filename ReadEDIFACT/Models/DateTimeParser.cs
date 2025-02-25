using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace ReadEDIFACT.Models
{
    public class DateTimeParser
    {
     public static string ParseDateTime(string value, string format)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        try
        {
            switch (format)
            {
                case "101": // YYMMDD
                    if (value.Length == 6)
                    {
                        string year = "20" + value.Substring(0, 2); // Asumimos siglo 21
                        string month = value.Substring(2, 2);
                        string day = value.Substring(4, 2);
                        return $"{year}-{month}-{day}";
                    }
                    break;

                case "201": // YYMMDDHHMM
                    if (value.Length == 10)
                    {
                        string year = "20" + value.Substring(0, 2);
                        string month = value.Substring(2, 2);
                        string day = value.Substring(4, 2);
                        string hour = value.Substring(6, 2);
                        string minute = value.Substring(8, 2);
                        return $"{year}-{month}-{day} {hour}:{minute}";
                    }
                    break;

                case "301": // YYMMDDHHMMZZZ
                    if (value.Length >= 10)
                    {
                        string year = "20" + value.Substring(0, 2);
                        string month = value.Substring(2, 2);
                        string day = value.Substring(4, 2);
                        string hour = value.Substring(6, 2);
                        string minute = value.Substring(8, 2);
                        string timeZone = value.Length > 10 ? value.Substring(10) : "";
                        return $"{year}-{month}-{day} {hour}:{minute} {timeZone}";
                    }
                    break;

                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            // Manejo de errores (opcional)
            Console.WriteLine($"Error parsing date/time: {ex.Message}");
        }

        return value; // Si no se puede parsear, devuelve el valor original
    }

    private static string ParseCustomFormat(string value)
    {
        // Lógica para manejar formatos personalizados (133, 132, etc.)
        // Por ejemplo, si el formato es YYMMDD:
        if (value.Length == 6)
        {
            string year = "20" + value.Substring(0, 2);
            string month = value.Substring(2, 2);
            string day = value.Substring(4, 2);
            return $"{year}-{month}-{day}";
        }
        return value;
    }

    }
}
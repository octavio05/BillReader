using System;
using System.Globalization;

namespace BillReader.Extensions
{
    public static class StringExtensions
    {

        public static TReturn GetStringBetween<TReturn>(this string text, string firstString, string lastString)
        {

            int
                Pos1 = text.IndexOf(firstString) + firstString.Length,
                Pos2 = text.IndexOf(lastString, Pos1);

            return (TReturn) Convert.ChangeType(text[Pos1..Pos2], typeof(TReturn));

        }

        public static DateTime GetDateTimeBetween(this string text, string firstString, string lastString)
        {

            int
                Pos1 = text.IndexOf(firstString) + firstString.Length,
                Pos2 = text.IndexOf(lastString, Pos1);

            return DateTime.Parse(text[Pos1..Pos2], new CultureInfo("es-ES"));

        }

        public static float GetFloatBetween(this string text, string firstString, string lastString)
        {

            int
                Pos1 = text.IndexOf(firstString) + firstString.Length,
                Pos2 = text.IndexOf(lastString, Pos1);

            try
            {

                return float.Parse(text[Pos1..Pos2].Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat);

            }
            catch
            {

                return default;
            
            }

        }

    }
}

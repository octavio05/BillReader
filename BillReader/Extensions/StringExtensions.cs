namespace BillReader.Extensions
{
    public static class StringExtensions
    {

        public static string GetStringBetween(this string text, string firstString, string lastString)
        {

            int
                Pos1 = text.IndexOf(firstString) + firstString.Length,
                Pos2 = text.IndexOf(lastString, Pos1);

            return text.Substring(Pos1, Pos2 - Pos1);

        }

    }
}

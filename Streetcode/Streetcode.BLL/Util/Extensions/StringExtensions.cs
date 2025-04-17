namespace Streetcode.BLL.Util.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveWhiteSpaces(this string value)
        {
            return value.Replace(" ", "");
        }
    }
}
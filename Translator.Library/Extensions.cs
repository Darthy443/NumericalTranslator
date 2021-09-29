namespace Translator.Library
{
    public static class Extensions
    {
        public static string AppendWithSeperator(this string string_to_append, string seperator, string word_to_append)
        {
            if (string.IsNullOrWhiteSpace(string_to_append))
            {
                return word_to_append;
            } else
            {
                return string_to_append + seperator + word_to_append;
            }
        }
    }
}
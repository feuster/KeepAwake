namespace KeepAwake.Assets.Translation
{
    internal static class Translate
    {
        /// <summary>
        /// Translate text id into the desired language
        /// </summary>
        /// <param name="Tag">text id</param>
        /// <param name="LanguageCode">language of translated text</param>
        /// <returns>translated text</returns>
        public static string GetText(string Tag, string LanguageCode)
        {
            if (string.IsNullOrEmpty(Tag))
                return string.Empty;
            if (string.IsNullOrEmpty(LanguageCode))
                LanguageCode = "en-us";

            string Text;
            try
            {
                if (LanguageCode.Equals("de-de", StringComparison.InvariantCultureIgnoreCase))
                    Text = Resource_de_de.ResourceManager.GetString(Tag) ?? string.Empty;
                else
                    Text = Resource_en_us.ResourceManager.GetString(Tag) ?? string.Empty;
            }
            catch
            {
                Text = "n/a";
            }

            return Text;
        }
    }
}

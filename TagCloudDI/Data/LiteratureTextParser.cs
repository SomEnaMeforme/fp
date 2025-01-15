namespace TagCloudDI.Data
{
    internal class LiteratureTextParser : IDataParser
    {
        public string[] Parse(string text)
        {
            var punctuation = text
               .Where(char.IsPunctuation)
               .Distinct()
               .ToArray();
            var words = text.Split()
                .Select(x => x.Trim(punctuation))
                .Where(x => x.Length > 0)
                .ToArray();
            return words;
        }
    }
}

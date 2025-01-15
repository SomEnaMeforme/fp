using ErrorHandling;
using ErrorHandling.QuerySyntax;

namespace TagCloudDI.Data
{
    public enum SpeechPart
    {
        Verb,
        Noun,
        Adjective,
        Adverb,
        Numb,
        Pronoun,
        Preposition,
        Interjection,
        Part,
        Conjunction
    }
    public class WordInfo
    {
        private static Dictionary<string, SpeechPart> getSpeechPart = new()
        {
            {"S", SpeechPart.Noun},
            {"V", SpeechPart.Verb},
            {"A",  SpeechPart.Adjective},
            {"ADV", SpeechPart.Adverb},
            {"PR", SpeechPart.Preposition},
            {"SPRO", SpeechPart.Pronoun},
            {"APRO", SpeechPart.Pronoun},
            {"NUM", SpeechPart.Numb},
            {"PART",  SpeechPart.Part},
            {"INTJ", SpeechPart.Interjection},
            {"CONJ", SpeechPart.Conjunction},
        };

        public SpeechPart SpeechPart { get; }
        public string InitialForm { get; set; }

        public WordInfo(SpeechPart speechPart, string initialForm)
        {
            SpeechPart = speechPart;
            InitialForm = initialForm;
        }

        public static Result<WordInfo[]> GetInfoFromWords(string[] words)
        {
            var result = new List<WordInfo>();
            var text = string.Join("\n", words);
            return MyStem.MyStem.AnalyseWords(text)
                .Then(text => text.Split('\n').Where(w => !string.IsNullOrEmpty(w)))
                .Then(words => words
                    .Select(word => new WordInfo(GetSpeechPart(word), GetInitForm(word)))
                    .ToArray());
        }

        private static SpeechPart GetSpeechPart(string analysedWord)
        {
            var start = analysedWord.IndexOf('=') + 1;
            var endSymbols = new HashSet<char> { '=', ',' };
            if (start > 0 && endSymbols.Count > 0)
            {
                var end = start + 1;
                while(end < analysedWord.Length && !endSymbols.Contains(analysedWord[end])) end++;
                if (getSpeechPart.TryGetValue(analysedWord.Substring(start, end - start), out var part))
                    return part;
            }
            return SpeechPart.Interjection;
        }

        private static string GetInitForm(string analysedWord)
        {
            var start = analysedWord.IndexOf('{') + 1;
            var end = analysedWord.IndexOf('=');
            var initialForm = start > 0 && end - start > 0 ? analysedWord.Substring(start, end - start) : "";
            return initialForm;
        }
    }
}

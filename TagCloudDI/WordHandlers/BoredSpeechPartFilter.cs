using TagCloudDI.Data;
using static TagCloudDI.MyStem.MyStem;

namespace TagCloudDI.WordHandlers
{
    internal class BoredSpeechPartFilter : IWordFilter
    {
        private HashSet<SpeechPart> boredSpeechPart =
        [
            SpeechPart.Part, SpeechPart.Conjunction, SpeechPart.Pronoun, SpeechPart.Interjection, SpeechPart.Preposition
        ];
        public bool Accept(WordInfo word)
        {
            return !boredSpeechPart.Contains(word.SpeechPart);
        }
    }
}

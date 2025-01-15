using TagCloudDI.Data;

namespace TagCloudDI.WordHandlers
{
    internal class EmptyWordsFilter : IWordFilter
    {
        public bool Accept(WordInfo word)
        {
            return !string.IsNullOrEmpty(word.InitialForm);
        }
    }
}

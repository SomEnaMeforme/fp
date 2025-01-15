using ErrorHandling;
using TagCloudDI.Data;

namespace TagCloudDI.WordHandlers
{
    internal class LowerCaseTransformer : IWordTransformer
    {
        public WordInfo Apply(WordInfo word)
        {
            word.InitialForm = word.InitialForm.ToLower();
            return word;
        }
    }
}

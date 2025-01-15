using ErrorHandling;
using TagCloudDI.Data;

namespace TagCloudDI.WordHandlers
{
    public interface IWordTransformer
    {
        public WordInfo Apply(WordInfo word);
    }
}
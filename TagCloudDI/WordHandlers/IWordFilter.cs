using TagCloudDI.Data;

namespace TagCloudDI.WordHandlers
{
    public interface IWordFilter
    {
        public bool Accept(WordInfo word);
    }
}
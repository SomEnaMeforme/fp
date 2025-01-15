using System.Drawing;

namespace TagCloudDI.CloudVisualize
{
    public class WordParameters
    {
        public float FontSize { get; }
        public Rectangle WordBorder { get; private set; }
        public string Word { get; }

        public WordParameters(string word, Rectangle border, float fontSize) 
        {
            WordBorder = border;
            Word = word;
            FontSize = fontSize;
        }

        public void MoveBorderToNewLocation(Point location)
        {
            WordBorder = new Rectangle(location, WordBorder.Size);
        }
    }
}

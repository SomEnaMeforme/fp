using System.Drawing;

namespace TagCloudDI.CloudVisualize
{
    public class VisualizeSettings
    {
        public FontFamily FontFamily { get; set; } = new FontFamily("Arial");
        public Color BackgroundColor { get; set; } = Color.White;
        public Size ImageSize { get; set; } = new Size(5000, 5000);
        public Color[] WordColors { get; set; } = new Color[]
        {
            Color.Blue, Color.Yellow, Color.Green, Color.Orange, Color.Orchid
        };

        public int MaxFontSize { get; set; } = 100;
        public int MinFontSize { get; set; } = 11;
    }
}
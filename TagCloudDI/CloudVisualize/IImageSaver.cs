using ErrorHandling;
using System.Drawing;

namespace TagCloudDI.CloudVisualize
{
    public interface IImageSaver
    {
        public Result<string> SaveImage(Bitmap image, string filename = null);
    }
}

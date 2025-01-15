using System.Drawing.Imaging;
using System.Drawing;
using ErrorHandling;

namespace TagCloudDI.CloudVisualize
{
    internal class DefaultImageSaver: IImageSaver
    {
        private ImageFormat format = ImageFormat.Png;

        public Result<string> SaveImage(Bitmap image, string? filePath = null)
        {
            var rnd = new Random();
            filePath ??= Path.Combine(Path.GetTempPath(), $"tagCloud{rnd.Next()}.{FormatToString().GetValueOrThrow()}");
            return image
                .AsResult()
                .Then(img => img.Save(filePath, format))
                .Then(_ => filePath)
                .RefineError("The file could not be saved using the transmitted path.");
        }

        private Result<string> FormatToString()
        {
            var converter = new ImageFormatConverter().AsResult();
            return converter
                .Then(c => c.ConvertToString(format))
                .Validate(format => format != null, "Try use unexpected image format")
                .Then(format => format.ToLower());
        }
    }
}
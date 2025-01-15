using ErrorHandling;
using System.Drawing;

namespace TagCloudDI
{
    public interface ICloudLayouter
    {
        public Result<Rectangle> PutNextRectangle(Size forInsertion);
        public void Clear();
    }
}
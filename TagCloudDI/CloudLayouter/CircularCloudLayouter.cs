using ErrorHandling;
using System.Drawing;
using TagCloudDI;

namespace TagsCloudVisualization.CloudLayouter
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly List<Rectangle> storage;
        private readonly CloudCompressor compressor;
        private readonly CirclePositionDistributor distributor;

        public CircularCloudLayouter(): this(new Point(0,0), [])
        { }

        private CircularCloudLayouter(Point center, List<Rectangle> storage)
        {
            this.storage = storage;
            distributor = new(center);
            compressor = new(center, storage);
        }

        public static CircularCloudLayouter CreateLayouterWithStartRectangles(Point center, List<Rectangle> storage)
        {
            return new CircularCloudLayouter(center, storage);
        }

        public Result<Rectangle> PutNextRectangle(Size nextRectangle)
        {
            var rectangleWithOptimalPosition = ValidateRectangleSize(nextRectangle)
            .Then(PutRectangleWithoutIntersection)
            .Then(compressor.CompressCloudAfterInsertion);

            var savingResult = rectangleWithOptimalPosition.Then(storage.Add);
            return savingResult.IsSuccess ? rectangleWithOptimalPosition : Result.Fail<Rectangle>(savingResult.Error);
        }

        public Result<Rectangle> PutRectangleWithoutIntersection(Size forInsertionSize)
        {
            Result<bool> isIntersected;
            Result<Rectangle> forInsertion;
            do
            {
                forInsertion = Result.Of(distributor.GetNextPosition)
                    .Then(p => new Rectangle(p, forInsertionSize));
                isIntersected = forInsertion.Then(r => r.IntersectedWithAnyFrom(storage));
            }
            while (isIntersected.GetValueOrDefault());

            return forInsertion;
        }

        private static Result<Size> ValidateRectangleSize(Size forInsertion)
        {
            return forInsertion.AsResult()
                .Validate(r => r.Width > 0 && r.Height > 0, $"Rectangle has incorrect size");
        }

        public void Clear()
        {
            storage.Clear();
        }
    }
}
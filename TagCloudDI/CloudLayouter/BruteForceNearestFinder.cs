using ErrorHandling;
using System.Drawing;

namespace TagsCloudVisualization.CloudLayouter
{
    public static class BruteForceNearestFinder
    {
        public static Rectangle? FindNearestByDirection(Rectangle r, Direction direction, List<Rectangle> rectangles)
        {
            if (rectangles.FirstOrDefault() == default)
                return null;
            var nearestByDirection = rectangles
                .Select(possibleNearest =>
                (Distance: CalculateMinDistanceBy(direction, possibleNearest, r), Nearest: possibleNearest))
                .Where(el => el.Distance.GetValueOrDefault() >= 0)
                .ToList();

            return nearestByDirection.Count > 0 ? nearestByDirection.MinBy(el => el.Distance.GetValueOrDefault()).Nearest : null;
        }

        public static Result<int> CalculateMinDistanceBy(Direction direction,
            Rectangle possibleNearest, Rectangle rectangleForFind)
        {
            return direction switch
            {
                Direction.Left => Result.AsResult(rectangleForFind.Left - possibleNearest.Right),
                Direction.Right => Result.AsResult(possibleNearest.Left - rectangleForFind.Right),
                Direction.Top => Result.AsResult(rectangleForFind.Top - possibleNearest.Bottom),
                Direction.Bottom => Result.AsResult(possibleNearest.Top - rectangleForFind.Bottom),
                _ => Result.Fail<int>("Try calculate distance by non existed direction"),
            };
        }
    }
}

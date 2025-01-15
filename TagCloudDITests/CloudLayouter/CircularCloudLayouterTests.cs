using System.Drawing;
using ErrorHandling;
using FluentAssertions;
using TagsCloudVisualization.CloudLayouter;

namespace TagCloudDITests.CloudLayouter;

public class CircularCloudLayouterTests
{
    private CircularCloudLayouter layouter;
    private Point defaultCenter;
    private List<Rectangle> storage;

    public static IEnumerable<TestCaseData> IntersectionTestsData
    {
        get
        {
            yield return new TestCaseData(new Size[]
                {
                    new(1, 1), new(1, 1), new(1, 1), new(400, 400),
                    new(1, 4), new(6, 6)
                })
                .SetName("WhenAddedSmallRectanglesWithOneVeryBig");
            yield return new TestCaseData(new Size[]
                {
                    new(100, 100), new(123, 121), new(100, 100), new(400, 400),
                    new(100, 400), new(600, 128)
                })
                .SetName("WhenAddedBigRectangles");
            yield return new TestCaseData(new Size[] { new(4, 4), new(4, 4), new(4, 4), new(4, 4), new(4, 4) })
                .SetName("WhenAddedRectanglesHasSameSize");
        }
    }

    [SetUp]
    public void SetUp()
    {
        defaultCenter = new Point(5, 5);
        storage = [];
        layouter = CircularCloudLayouter.CreateLayouterWithStartRectangles(defaultCenter, storage);
    }

    [TestCase(0, 4, TestName = "WhenWidthZero")]
    [TestCase(3, 0, TestName = "WhenHeightZero")]
    [TestCase(-3, 4, TestName = "WhenWidthIsNegative")]
    [TestCase(3, -4, TestName = "WhenHeightNegative")]
    [TestCase(-3, -4, TestName = "WhenWidthAndHeightNegative")]
    [TestCase(0, 0, TestName = "WhenWidthAndHeightIsZero")]
    public void Insert_ShouldThrow(int width, int height)
    {
        var inCorrectSize = new Size(width, height);
        var res = layouter.PutNextRectangle(inCorrectSize);
        Action act = () => layouter.PutNextRectangle(inCorrectSize).GetValueOrThrow();

        act.Should().Throw<InvalidOperationException>();
    }

    [TestCaseSource(nameof(IntersectionTestsData))]
    public void PutRectangleOnCircleWithoutIntersection_ShouldPutRectangleWithoutIntersection(Size[] sizes)
    {
        layouter = InsertFewRectangles(layouter, sizes, storage);
        var last = layouter.PutRectangleWithoutIntersection(new Size(3, 3));

        storage.Where(addedRectangle => addedRectangle.IntersectsWith(last.GetValueOrThrow())).Should().BeEmpty();
    }

    private static CircularCloudLayouter InsertFewRectangles(CircularCloudLayouter layouter, Size[] sizes,
        List<Rectangle> storage)
    {
        for (var i = 0; i < sizes.Length; i++)
        {
            var forInsertion = layouter.PutNextRectangle(sizes[i]);
            storage.Add(forInsertion.GetValueOrThrow());
        }

        return layouter;
    }

    [Test]
    public void PutNextRectangle_ShouldTryMoveRectangleCloserToCenter_WhenItPossible()
    {
        var firstRectangleSize = new Size(6, 4);
        var secondRectangleSize = new Size(4, 4);

        var first = layouter.PutNextRectangle(firstRectangleSize).GetValueOrThrow();
        var second = layouter.PutNextRectangle(secondRectangleSize).GetValueOrThrow();
        var hasEqualBound = first.Right == second.Left || first.Top == second.Bottom
                                                       || second.Right == first.Left || second.Top == first.Bottom;

        first.IntersectsWith(second).Should().BeFalse();
        hasEqualBound.Should().BeTrue();
    }
}
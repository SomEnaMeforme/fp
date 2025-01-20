using ErrorHandling;
using FakeItEasy;
using FluentAssertions;
using System.Drawing;
using TagCloudDI;
using TagCloudDI.CloudVisualize;
using TagCloudDI.Data;
using TagCloudDI.WordHandlers;

namespace TagCloudDITests
{
    internal class ErrorHandlingTests
    {
        private IImageSaver fakeSaver;
        private IDataParser fakeParser;
        private IFileDataSource fakeDataSource;
        private ICloudLayouter fakeLayouter;
        private IWordColorDistributor fakeColorDistributor;
        private Func<string, IFileDataSource> sourceFactory;

        private DataProvider dataProvider;
        private CloudVisualizer visualizer;
        private CloudCreator cloudCreator;
        
        private VisualizeSettings settings;

        [SetUp]
        public void SetUp()
        {
            CreateFakeDataProviderWithDependencies();
            CreateFakeCloudVisualizerWithDependencies();
            fakeSaver = A.Fake<IImageSaver>();
            cloudCreator = new CloudCreator(dataProvider, visualizer, fakeSaver);
            DefineDefaultBehavior();
        }

        private void CreateFakeDataProviderWithDependencies()
        {
            fakeDataSource = A.Fake<IFileDataSource>();
            sourceFactory = A.Fake<Func<string, IFileDataSource>>();
            fakeParser = A.Fake<IDataParser>();
            dataProvider = new DataProvider(sourceFactory, Array.Empty<IWordTransformer>(), Array.Empty<IWordFilter>(), fakeParser);
        }

        private void CreateFakeCloudVisualizerWithDependencies()
        {
            fakeLayouter = A.Fake<ICloudLayouter>();
            fakeColorDistributor = A.Fake<IWordColorDistributor>();
            settings = A.Fake<VisualizeSettings>();
            visualizer = new CloudVisualizer(settings, fakeLayouter, fakeColorDistributor);
        }

        private void DefineDefaultBehavior()
        {
            A.CallTo(() => sourceFactory(A<string>.Ignored)).Returns(fakeDataSource);
            A.CallTo(() => fakeDataSource.GetData(A<string>.Ignored)).Returns(string.Empty);
            A.CallTo(() => fakeParser.Parse(A<string>.Ignored)).Returns(new string[] { "праздник" });

            A.CallTo(() => fakeSaver.SaveImage(A<Bitmap>.Ignored, A<string>.Ignored)).Returns(Result.Ok("somePath"));
            A.CallTo(() => fakeLayouter.PutNextRectangle(A<Size>.Ignored)).Returns(Result.Ok(new Rectangle(new(1, 1), new(1, 1))));
            A.CallTo(() => fakeColorDistributor.GetColor(A<Color[]>.Ignored)).Returns(Color.Aqua);
        }

        [Test]
        public void CloudCreator_ShouldReturnFailResult_WhenImageSaveFail()
        {
            var errorMessage = "Some error reason";

            A.CallTo(() => fakeSaver.SaveImage(A<Bitmap>.Ignored, A<string>.Ignored)).Returns(Result.Fail<string>(errorMessage));
            var result = cloudCreator.CreateTagCloud("somePath");

            result.Should().Be(Result.Fail<string>($"Failed to create tag cloud. {errorMessage}"));
        }

        [Test]
        public void CloudCreator_ShouldReturnFailResult_WhenCreateImageFail()
        {
            var errorMessage = "Some error";
            A.CallTo(() => fakeLayouter.PutNextRectangle(A<Size>.Ignored)).Returns(Result.Fail<Rectangle>(errorMessage));
            var result = cloudCreator.CreateTagCloud("somePath");

            result.Should().Be(Result.Fail<string>($"Failed to create tag cloud. Failed to layout all words, by causes: {errorMessage}"));
        }

        [Test]
        public void CloudCreator_ShouldNotInvokeImageSave_WhenCreateImageFail()
        {
            A.CallTo(() => fakeLayouter.PutNextRectangle(A<Size>.Ignored)).Returns(Result.Fail<Rectangle>("error"));

            var result = cloudCreator.CreateTagCloud("somePath");

            A.CallTo(() => fakeSaver.SaveImage(A<Bitmap>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public void CloudCreator_ShouldReturnFailResult_WhenGetPreprocessedWordsFail()
        {
            A.CallTo(() => fakeParser.Parse(A<string>.Ignored)).Returns(Array.Empty<string>());

            var result = cloudCreator.CreateTagCloud("somePath");

            result.Should().Be(Result.Fail<string>($"Failed to create tag cloud. The source provided does not contain words for the tag cloud."));
        }
    }
}

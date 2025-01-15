using TagCloudDI.Data;
using FakeItEasy;
using TagCloudDI.WordHandlers;
using FluentAssertions;

namespace TagCloudDITests.Data
{
    public class DataProviderTests
    {
        private DataProvider provider;
        private IDataParser fakeParser;
        [SetUp]
        public void SetUp()
        {
            var fakeDataSource = A.Fake<IFileDataSource>();
            var sourceFactory = A.Fake<Func<string, IFileDataSource>>();
            A.CallTo(() => sourceFactory(A<string>.Ignored)).Returns(fakeDataSource);
            A.CallTo(() => fakeDataSource.GetData(A<string>.Ignored)).Returns(string.Empty);
            fakeParser = A.Fake<IDataParser>();
            A.CallTo(() => fakeParser.Parse(A<string>.Ignored)).Returns(new string[] { });
            provider = new DataProvider(sourceFactory, Array.Empty<IWordTransformer>(), Array.Empty<IWordFilter>(), fakeParser);
        }

        [Test]
        public void ShouldCorrectCalculateFrequency_WhenDifferentWords()
        {
            A.CallTo(() => fakeParser.Parse(A<string>.Ignored)).Returns(new string[] {
                "праздник",
                "праздник",
                "трагедия",
                "праздник"});

            var result = provider.GetPreprocessedWords("").GetValueOrThrow();

            result.Should().BeEquivalentTo(new (string Word, double Frequency)[]
            {
                (Word: "праздник", Frequency: 1),
                (Word: "трагедия", Frequency: 1/3.0),
            });

        }
        [Test]
        public void ShouldCorrectCalculateFrequency_WhenSameWord()
        {
            A.CallTo(() => fakeParser.Parse(A<string>.Ignored)).Returns(new string[] {
                "праздник",
                "праздник",
                "праздник",
                "праздник"});

            var result = provider.GetPreprocessedWords("").GetValueOrThrow();

            result.Should().BeEquivalentTo(new (string Word, double Frequency)[]
            {
                (Word: "праздник", Frequency: 1),
            });

        }
        [Test]
        public void ShouldCorrectCalculateFrequency_WhenDifferentWordsWithSameFrequency()
        {
            A.CallTo(() => fakeParser.Parse(A<string>.Ignored)).Returns(new string[] {
                "праздник",
                "праздник",
                "трагедия",
                "трагедия"});

            var result = provider.GetPreprocessedWords("").GetValueOrThrow();

            result.Should().BeEquivalentTo(new (string Word, double Frequency)[]
            {
                (Word: "праздник", Frequency: 1),
                (Word: "трагедия", Frequency: 1),
            });
        }
    }
}

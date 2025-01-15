using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagCloudDI.Data;

namespace TagCloudDITests.Data
{
    public class WordInfoTests
    {
        [Test]
        public void ParseText_ShouldCorrectParseWords()
        {
            var words = new string[] { "гулять", "лес", "в", "красивый" };

            var result = WordInfo.GetInfoFromWords(words).GetValueOrThrow();

            result.Length.Should().Be(4);
            result.Select(i => i.InitialForm).Should().BeEquivalentTo(words);
        }
        [Test]
        public void ParseText_ShouldCorrectMapSpeechPartParseWords()
        {
            var words = new string[] { "гулять", "лес", "из-под", "красивый" };

            var result = WordInfo.GetInfoFromWords(words).GetValueOrThrow();

            result.Length.Should().Be(4);
            result.Select(i => i.SpeechPart).Should().BeEquivalentTo(new SpeechPart[]
            {
                SpeechPart.Verb, SpeechPart.Noun, SpeechPart.Preposition, SpeechPart.Adjective
            });
        }
        [Test]
        public void ParseText_ShouldCorrectMapSpeech_WhenTwoOrMoreVariantsForSpeechPart()
        {
            var words = new string[] { "в", "к", "из-под", "за", "для", "из-за", "о", "об", "без", "с", "вроде", "до", "по" };

            var result = WordInfo.GetInfoFromWords(words).GetValueOrThrow();

            result.Select(i => i.SpeechPart).Should().BeEquivalentTo(words.Select(_ => SpeechPart.Preposition));
        }
    }
}

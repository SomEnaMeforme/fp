using FluentAssertions;
using System.Drawing;
using TagCloudDI.ConsoleInterface;

namespace TagCloudDITests.ConsoleInterface
{
    public class SettingsParsersTests
    {
        public static IEnumerable<TestCaseData> ColorsForParse
        {
            get
            {
                yield return new TestCaseData("alice blue", false, Color.Empty).SetName("WhenColorNameHasSpace");
                yield return new TestCaseData("AliceBlue", true, Color.AliceBlue).SetName("WhenColorNameCorrectAndHasTwoWords");
                yield return new TestCaseData("aliceblue", true, Color.AliceBlue).SetName("WhenColorNameCorrectAndLowerCase");
                yield return new TestCaseData("AliceBluberryHa", false, Color.Empty).SetName("WhenColorNonExisted");
            }
        }

        [TestCaseSource(nameof(ColorsForParse))]
        public void ColorParser_ShouldParseStringCorrect(string value, bool isSuccess, Color expected)
        {
            var register = new SettingsParsersRegister();

            var actual = register.ColorParser(value);

            actual.Item1.Should().Be(isSuccess);
            actual.Item2.Should().Be(expected);
        }
        public static IEnumerable<TestCaseData> FontsForParse
        {
            get
            {
                yield return new TestCaseData("Calibri", true, new FontFamily("Calibri")).SetName("WhenFontCorrect");
                yield return new TestCaseData("Times New Roman", true, new FontFamily("Times New Roman")).SetName("WhenFontCorrectAndHasMoreThanOneWord");
                yield return new TestCaseData("calibri", false, null).SetName("WhenFontCorrectAndLowerCase");
                yield return new TestCaseData("StrangeFont", false, null).SetName("WhenFontNonExisted");
            }
        }

        [TestCaseSource(nameof(FontsForParse))]
        public void FontParser_ShouldParseStringCorrect(string value, bool isSuccess, FontFamily expected)
        {
            var register = new SettingsParsersRegister();

            var actual = register.FontParser(value);

            actual.Item1.Should().Be(isSuccess);
            actual.Item2.Should().Be(expected);
        }
        public static IEnumerable<TestCaseData> SizeForParse
        {
            get
            {
                yield return new TestCaseData("-11;-100", false, Size.Empty).SetName("WhenHeightOrWidthLessThanZero");
                yield return new TestCaseData("0;0", false, Size.Empty).SetName("WhenHeightOrWidthZero");
                yield return new TestCaseData("11;100", true, new Size(11, 100)).SetName("WhenHeightOrWidthCorrect");
                yield return new TestCaseData("-11;-1b0", false, Size.Empty).SetName("WhenHeightOrWidthNotNumber");
                yield return new TestCaseData("11;-1.0", false, Size.Empty).SetName("WhenHeightOrWidthIsDouble");
            }
        }

        [TestCaseSource(nameof(SizeForParse))]
        public void SizeParser_ShouldParseStringCorrect(string value, bool isSuccess, Size expected)
        {
            var register = new SettingsParsersRegister();

            var actual = register.ImageSizeParser(value);

            actual.Item1.Should().Be(isSuccess);
            actual.Item2.Should().Be(expected);
        }
    }
}

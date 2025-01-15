using McMaster.Extensions.CommandLineUtils.Abstractions;
using System.Drawing;
using System.Drawing.Text;

namespace TagCloudDI.ConsoleInterface
{
    public class SettingsParsersRegister
    {
        public void RegisterParsers(ValueParserProvider store)
        {
            Func<string?, string?, FormatException> errorSelector =
                (name, _) => new FormatException($"The passed value for the {name} is incorrect");
            store.Add(ValueParser.Create((value, _) => FontParser(value), errorSelector));
            store.Add(ValueParser.Create((value, _) => ImageSizeParser(value), errorSelector));
            store.Add(ValueParser.Create((value, _) => ColorParser(value), errorSelector));
        }
        public (bool, FontFamily) FontParser(string? value)
        {
            var isSuccessParse = !string.IsNullOrEmpty(value) && IsFontInstalled(value);
            return (isSuccessParse, isSuccessParse ? new FontFamily(value) : default);
        }

        private bool IsFontInstalled(string font)
        {
            return FontFamily.Families.Any(f => f.Name == font);
        }
        public (bool, Size) ImageSizeParser(string value)
        {
            var values = value.Split(';');
            var hasWidth = int.TryParse(values[0], out var width);
            var hasHeight = int.TryParse(values[1], out var height);
            var isSuccessParse = hasHeight && hasWidth && width > 0 && height > 0;
            return (isSuccessParse, isSuccessParse ? new Size(width, height) : Size.Empty);
        }
        public (bool, Color) ColorParser(string value)
        {
            var color = Color.FromName(value);
            return (color.IsKnownColor, color.IsKnownColor ? color : Color.Empty);
        }
    }
}
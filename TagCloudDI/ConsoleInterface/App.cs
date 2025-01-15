using TagCloudDI.CloudVisualize;
using McMaster.Extensions.CommandLineUtils;
using System.Drawing;
using System.Diagnostics;
using ErrorHandling;
using Spire.Doc;

namespace TagCloudDI.ConsoleInterface
{
    public class App
    {
        private CommandLineApplication app;
        private CloudCreator creator;
        private VisualizeSettings settings;
        private SettingsParsersRegister parserRegister = new();
        public App(CloudCreator creator, VisualizeSettings visualizeSettings) 
        {
            this.creator = creator;
            settings = visualizeSettings;
            app = new CommandLineApplication<App>();

            app.HelpOption();
            app.Command("createTagCloud", ConfigureCreateCloudCommand);
            app.Command("configure", ConfigureChangeImageParameterCommand);
        }

        private void ConfigureCreateCloudCommand(CommandLineApplication cmd)
        {
            cmd.Description = "Create a tag cloud based on the transmitted text file";
            cmd.HelpOption();
            var filePath = cmd.Argument("file", "Path to file").IsRequired();
            cmd.OnExecute(() =>
            {
                var pathToImage = Result.Of(() => filePath.Value)
                .Then(CheckIsFileExist)
                .Then(creator.CreateTagCloud)
                .Then(OpenImage)
                .OnFail(Console.WriteLine);                
            });
        }
        private Result<string> CheckIsFileExist(string filePath)
        {
            return File.Exists(filePath) ? Result.Ok(filePath) : Result.Fail<string>("Try read non existed file");
        }

        private Result<string> OpenImage(string pathToImage)
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(pathToImage) { UseShellExecute = true };
            p.Start();
            return Result.AsResult(pathToImage);
        }

        private void ConfigureChangeImageParameterCommand(CommandLineApplication cmd)
        {
            parserRegister.RegisterParsers(cmd.ValueParsers);
            var sizeSeparator = ";";
            cmd.Description = "Configure settings for tag cloud visualization";
            cmd.HelpOption();
            var font = cmd.Option<FontFamily>("-f|--font <FONT>", "The font for words", CommandOptionType.SingleValue);
            var fontSizeMin = cmd.Option<int>("-fs-min|--fontSize <MIN>", "The min for font size", CommandOptionType.SingleValue);
            var fontSizeMax = cmd.Option<int>("-fs-max|--fontSize <MAX>", "The max for font size", CommandOptionType.SingleValue);
            var wordsColor = cmd.Option<Color>("-wc|--word-color <COLOR_NAMES>", "The words colors", CommandOptionType.MultipleValue);
            var backgroundColor = cmd.Option<Color>("-bg|--background-color <COLOR_NAME>", "The background color", CommandOptionType.SingleValue);
            var imageSize = cmd.Option<Size>($"-i|--imageSize <WIDTH{sizeSeparator}HEIGHT>", "The size for image in pixel", CommandOptionType.SingleValue);
            cmd.OnExecute(() =>
            {
                settings.FontFamily = font.HasValue() ? font.ParsedValue : settings.FontFamily;
                settings.MinFontSize = fontSizeMin.HasValue() ? fontSizeMin.ParsedValue : settings.MinFontSize;
                settings.MaxFontSize = fontSizeMax.HasValue() ? fontSizeMax.ParsedValue : settings.MaxFontSize;
                settings.ImageSize = imageSize.HasValue() ? imageSize.ParsedValue : settings.ImageSize;
                settings.WordColors = wordsColor.HasValue() ? wordsColor.ParsedValues.ToArray() : settings.WordColors;
                settings.BackgroundColor = backgroundColor.HasValue() ? backgroundColor.ParsedValue : settings.BackgroundColor;
            });
        }

        public void Run()
        {
            while (true)
            {
                Result.Of(() => Console.ReadLine())
                    .Then(args => args.Split(' '))
                    .Then(app.Execute)
                    .RefineError("Couldn't create tag cloud")
                    .OnFail(Console.WriteLine);
            }
        }
    }
}

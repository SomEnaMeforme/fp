using ErrorHandling;
using System.Diagnostics;
using System.Text;

namespace TagCloudDI.MyStem
{
    public static class MyStem
    {
        public static Result<string> AnalyseWords(string words)
        {
            var directory = ".\\MyStem";
            var inputFile = Path.Combine(directory, "input.txt");
            var outputFile = Path.Combine(directory, "output.txt");
            var mySteam = Path.Combine(directory, "mystem.exe");
            var arguments = string.Format("-in {0} {1}", inputFile, outputFile);

            return Result
                .OfAction(() =>
                {
                    using (var writer = new StreamWriter(inputFile))
                    {
                        writer.Write(words);
                    }
                })
                .Then((_) => new ProcessStartInfo
                {
                    FileName = mySteam,
                    Arguments = arguments,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                })
                .Then(info => new Process { StartInfo = info })
                .Then(process =>
                {
                    process.Start();
                    process.WaitForExit();
                })
                .Then((_) =>
                {
                    using var reader = new StreamReader(outputFile, Encoding.UTF8);
                    return reader.ReadToEnd();
                })
                .RefineError("Failed to analyze word with MyStem");
        }
    }
}

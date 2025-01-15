using ErrorHandling;
using TagCloudDI.WordHandlers;

namespace TagCloudDI.Data
{
    public class DataProvider
    {
        private Func<string, IFileDataSource> getSource;
        private IEnumerable<IWordTransformer> transformers;
        private IEnumerable<IWordFilter> filters;
        private IDataParser parser;
        public DataProvider(
            Func<string, IFileDataSource> factory, 
            IEnumerable<IWordTransformer> transformers, 
            IEnumerable<IWordFilter> filters,
            IDataParser parser) 
        {
            this.transformers = transformers;
            this.filters = filters;
            getSource = factory;
            this.parser = parser;
        }

        public Result<(string Word, double Frequency)[]> GetPreprocessedWords(string filePath)
        {
            var source = getSource(Path.GetExtension(filePath));
            return Result.Of(() => source.GetData(filePath), "Failed to read the transferred file with words")
                .Then(parser.Parse)
                .Then(PreprocessData);
        }

        private Result<(string Word, double Frequency)[]> PreprocessData(string[] words)
        {
            var noSuitableWordsError = "The source provided does not contain words for the tag cloud.";
            if (words.Length == 0) return 
                    Result.Fail<(string Word, double Frequency)[]>(noSuitableWordsError);
            var wordInfos = WordInfo.GetInfoFromWords(words);
            return wordInfos
                .Then(info => info
                    .Where(w => filters.All(f => f.Accept(w)))
                    .Select(ApplyTransformers))
                .Validate(words => words.FirstOrDefault() != default, noSuitableWordsError)
                .Then(CalculateFrequency)
                .Then(w => w.Select(kvp => (Word: kvp.Key, Frequency: kvp.Value)).ToArray())
                .RefineError("Failed to preprocess words");
        }

        private Dictionary<string, double> CalculateFrequency(IEnumerable<string> words)
        {
            var frequency = new Dictionary<string, double>();
            foreach (var word in words)
            {
                if (!frequency.ContainsKey(word))
                    frequency.Add(word, 0);
                frequency[word]++;
            }
            var maxFrequency = frequency.Max(kvp => kvp.Value);                
            return frequency.ToDictionary(kvp => kvp.Key, kvp => kvp.Value / maxFrequency);
        }

        private string ApplyTransformers(WordInfo word)
        {
            foreach (var transformer in transformers)
            {
                word = transformer.Apply(word);
            }
            return word.InitialForm;
        }
    }
}

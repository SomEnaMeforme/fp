using ErrorHandling;
using TagCloudDI.CloudVisualize;
using TagCloudDI.Data;

namespace TagCloudDI
{
    public class CloudCreator
    {
        private DataProvider dataProvider;
        private CloudVisualizer cloudVisualizer;
        private readonly IImageSaver imageSaver;
        public CloudCreator(DataProvider dataProvider, CloudVisualizer visualizer, IImageSaver saver) 
        {
            this.dataProvider = dataProvider;
            cloudVisualizer = visualizer;
            imageSaver = saver;
        }
        
        public Result<string> CreateTagCloud(string pathToFileWithWords)
        {
            return dataProvider.GetPreprocessedWords(pathToFileWithWords)
                .Then(cloudVisualizer.CreateImage)
                .Then(image => imageSaver.SaveImage(image))
                .RefineError("Failed to create tag cloud");
        }
    }
}
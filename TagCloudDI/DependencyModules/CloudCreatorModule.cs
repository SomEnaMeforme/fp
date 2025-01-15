using Autofac;
using TagCloudDI.CloudVisualize;
using TagsCloudVisualization.CloudLayouter;

namespace TagCloudDI.DependencyModules
{
    public class CloudCreatorModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CloudCreator>();
            builder.RegisterType<CloudVisualizer>();
            builder.RegisterType<DefaultImageSaver>().As<IImageSaver>();
            RegisterVisualizerDependencies(builder);
        }

        private void RegisterVisualizerDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<VisualizeSettings>().SingleInstance();
            builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
            builder.RegisterType<RandomWordColorDistributor>().As<IWordColorDistributor>();
        }
    }
}

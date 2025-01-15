using Autofac;
using TagCloudDI.WordHandlers;

namespace TagCloudDI.DependencyModules
{
    internal class WordHandlersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterWordTransformers(builder);
            RegisterWordFilters(builder);
        }

        private void RegisterWordFilters(ContainerBuilder builder)
        {
            builder.RegisterType<BoredSpeechPartFilter>().As<IWordFilter>();
            builder.RegisterType<EmptyWordsFilter>().As<IWordFilter>();
        }
        private void RegisterWordTransformers(ContainerBuilder builder)
        {
            builder.RegisterType<LowerCaseTransformer>().As<IWordTransformer>();
        }
    }
}

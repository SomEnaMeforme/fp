using Autofac;
using TagCloudDI.Data;

namespace TagCloudDI.DependencyModules
{
    internal class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterMainDependencies(builder);
            RegisterDataParser(builder);
            RegisterFileDataSource(builder);
        }

        private void RegisterMainDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<DataProvider>();
            builder.Register<Func<string, IFileDataSource>>(c =>
            {
                var ctx = c.Resolve<IComponentContext>();
                return p => ctx.IsRegisteredWithKey<IFileDataSource>(p) 
                ? ctx.ResolveKeyed<IFileDataSource>(p)
                : new DefaultDataSource();
            });
        }

        private void RegisterDataParser(ContainerBuilder builder)
        {
            builder.RegisterType<LiteratureTextParser>().As<IDataParser>();
        }

        private void RegisterFileDataSource(ContainerBuilder builder)
        {
            builder.RegisterType<TxtFileDataSource>().Keyed<IFileDataSource>(".txt");
            builder.RegisterType<DocFileDataSource>().Keyed<IFileDataSource>(".doc");
            builder.RegisterType<DocxFileDataSource>().Keyed<IFileDataSource>(".docx");
        }
    }
}

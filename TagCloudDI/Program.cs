using Autofac;
using TagCloudDI.ConsoleInterface;
using TagCloudDI.DependencyModules;

namespace TagCloudDI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var container = RegisterDependencies();
            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<App>();
                app.Run();
            }
        }

        public static IContainer? RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<CloudCreatorModule>();
            builder.RegisterModule<DataModule>();
            builder.RegisterModule<WordHandlersModule>();
            builder.RegisterType<App>().SingleInstance();
            return builder.Build();
        }
    }
}

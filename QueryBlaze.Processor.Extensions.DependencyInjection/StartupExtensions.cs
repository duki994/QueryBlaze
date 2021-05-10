using QueryBlaze.Processor.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static ICustomizableBuilder AddQueryProcessor(this IServiceCollection services)
        {
            return new StartupServicesBuilder(services)
                .AddQueryProcessor();
        }
    }
}

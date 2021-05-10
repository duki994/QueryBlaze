using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using QueryBlaze.Processor.Abstractions;
using QueryBlaze.Processor.Implementation;

namespace QueryBlaze.Processor.Extensions.DependencyInjection
{
    public interface ICustomizableBuilder
    {
        ICustomizableBuilder WithInputParser<T>() where T : class, IInputParser;
        ICustomizableBuilder WithProcessorOptionsProvicer<T>() where T : class, ISortProcessorOptionsProvider;
    }

    public class StartupServicesBuilder : ICustomizableBuilder
    {
        private readonly IServiceCollection _services;

        public StartupServicesBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public ICustomizableBuilder AddQueryProcessor()
        {
            _services.AddScoped<IInputParser, InputParser>();
            _services.AddScoped<ISortProcessorOptionsProvider, DefaultSortProcessorOptionsProvider>();
            _services.AddSingleton<LambdaExpressionFactory>();

            return this;
        }


        public ICustomizableBuilder WithInputParser<T>()
            where T : class, IInputParser
        {
            _services.Replace(ServiceDescriptor.Scoped<IInputParser, T>());
            return this;
        }

        public ICustomizableBuilder WithProcessorOptionsProvicer<T>()
            where T : class, ISortProcessorOptionsProvider
        {
            _services.Replace(ServiceDescriptor.Scoped<ISortProcessorOptionsProvider, T>());
            return this;
        }
    }
}

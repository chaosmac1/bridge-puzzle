using Microsoft.Extensions.DependencyInjection;
using Mosaic.Repository.Collection.Adapter.Export;
using Mosaic.Repository.Collection.Adapter.LoadProvider;

namespace Mosaic.Repository.Collection.Domain;

public class LoadProvider: ILoadProvider {
    public ServiceCollection Collection { get; }
    public ILoadProvider Bind(IServiceBinder services) {
        services.Bind(Collection);
        return this;
    }
    
    public LoadProvider() => Collection = new ServiceCollection();


    public void Build() => Adapter.Injection.SetProvider(Collection.BuildServiceProvider());
    
    
    public ILoadProviderAdd<ILoadProviderAdd> AsILoadProviderAdd() => this;
    public ILoadProvider AsILoadProvider() => this;
}
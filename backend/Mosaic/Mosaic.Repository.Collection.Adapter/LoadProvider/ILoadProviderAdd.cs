using Microsoft.Extensions.DependencyInjection;
using Mosaic.Repository.Collection.Adapter.Export;

namespace Mosaic.Repository.Collection.Adapter.LoadProvider;

public interface ILoadProviderAdd<out T> : ILoadProviderAdd where T : ILoadProviderAdd {
    public ServiceCollection Collection { get; }
    
    public new T Bind(IServiceBinder services);
}

public interface ILoadProviderAdd {
    public ILoadProviderAdd Bind(IServiceBinder services);
}
using Microsoft.Extensions.DependencyInjection;

namespace Mosaic.Repository.Collection.Adapter.Export;

public interface IServiceBinder {
    public void Bind(ServiceCollection serviceCollection);
}
using Cassandra;
using Microsoft.Extensions.DependencyInjection;
using Mosaic.Repository.ScyllaDb.Adapter;
using Mosaic.Repository.ScyllaDb.Domain;

namespace Mosaic.Repository.ScyllaDb.Binder;

public class ServiceScyllaDbBinder: Collection.Adapter.Export.IServiceBinder {
    private ServiceScyllaDbBinder() { }

    public static Collection.Adapter.Export.IServiceBinder Create() => new ServiceScyllaDbBinder();
    
    public void Bind(ServiceCollection serviceCollection) {
        serviceCollection.AddScoped<IScyllaDbContext>((provider => new Domain.ScyllaDbContext()));
        serviceCollection.AddScoped<IScyllaDbTableCreator>((provider => new ScyllaDbTableCreator(provider.GetScyllaDbContext())));
    }
}
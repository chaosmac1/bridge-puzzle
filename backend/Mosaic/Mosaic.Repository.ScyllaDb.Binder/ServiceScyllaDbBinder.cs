using Cassandra;
using Cassandra.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Mosaic.Repository.ScyllaDb.Adapter;
using Mosaic.Repository.ScyllaDb.Adapter.Interface;
using Mosaic.Repository.ScyllaDb.Domain;
using Mosaic.Repository.ScyllaDb.Domain.Query;
using Mosaic.Repository.ScyllaDb;
namespace Mosaic.Repository.ScyllaDb.Binder;

public class ServiceScyllaDbBinder: Collection.Adapter.Export.IServiceBinder {
    private ServiceScyllaDbBinder() { }

    public static Collection.Adapter.Export.IServiceBinder Create() => new ServiceScyllaDbBinder();
    
    public void Bind(ServiceCollection serviceCollection) {
        serviceCollection.AddScoped<IScyllaDbContext>((provider => new Domain.ScyllaDbContext()));
        serviceCollection.AddScoped<Mapper>((provider => {
            var mapper = provider.GetScyllaDbContext().GetMapperAsync();
            if (mapper.IsCompletedSuccessfully) {
                return mapper.Result;
            }

            return mapper.GetAwaiter().GetResult();
        }));
        serviceCollection.AddScoped<ISession>((provider => {
            var session = provider.GetScyllaDbContext().GetDbAsync();
            if (session.IsCompletedSuccessfully) {
                return session.Result;
            }

            return session.GetAwaiter().GetResult();
        }));
        serviceCollection.AddScoped<IScyllaDbTableCreator>((provider => new ScyllaDbTableCreator(provider.GetScyllaDbContext())));
        serviceCollection.AddScoped<IQueryLogContext>((provider => new QueryLogContext(provider.GetMapper())));
    }
}
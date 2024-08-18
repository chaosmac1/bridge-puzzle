using Microsoft.Extensions.DependencyInjection;
using Mosaic.Repository.Collection.Adapter.Export;
using Mosaic.Repository.Postgresql.Adapter;

namespace Mosaic.Repository.Postgresql.Binder;

public class ServicePostgresqlBinder: IServiceBinder {
    private ServicePostgresqlBinder() { }
    
    public static IServiceBinder Create() => new ServicePostgresqlBinder();
    
    public void Bind(ServiceCollection serviceCollection) {
        serviceCollection.AddScoped<INpgsqlContext>((provider => new Domain.NpgsqlContext()));
        serviceCollection.AddScoped<INpgsqlTableCreator>((provider => new Domain.NpgsqlTableCreator(provider.GetNpgsqlContext())));
    }
}
using Microsoft.Extensions.DependencyInjection;
using Mosaic.Repository.Collection.Adapter.Export;
using Mosaic.Repository.Postgresql.Adapter;
using Mosaic.Repository.Postgresql.Adapter.Interface;
using Mosaic.Repository.Postgresql.Domain;

namespace Mosaic.Repository.Postgresql.Binder;

public class ServicePostgresqlBinder: IServiceBinder {
    private ServicePostgresqlBinder() { }
    
    public static IServiceBinder Create() => new ServicePostgresqlBinder();
    
    public void Bind(IServiceCollection serviceCollection) {
        serviceCollection.AddScoped<INpgsqlContext>();
        serviceCollection.AddScoped<INpgsqlTableCreator>((provider => new Domain.NpgsqlTableCreator(provider.GetNpgsqlContext())));
        serviceCollection.AddSingleton<NpgsqlBuilder>();
    }
}
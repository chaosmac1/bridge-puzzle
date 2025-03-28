using Microsoft.Extensions.DependencyInjection;
using Mosaic.Repository.Collection.Adapter.Export;
using Mosaic.Repository.Logging.Adapter.Interface;
using Mosaic.Repository.Logging.Domain;
using Mosaic.Repository.ScyllaDb.Adapter;
using Mosaic.Repository.ScyllaDb.Adapter.Interface;

namespace Mosaic.Repository.Logging.Binder;

public class ServiceLoggingBinder: IServiceBinder {
    private ServiceLoggingBinder() { }
    
    public static ServiceLoggingBinder Create() => new ServiceLoggingBinder();
    
    public void Bind(IServiceCollection serviceCollection) {
        serviceCollection.AddScoped<ILogId>(x => LogId.New);
        serviceCollection.AddSingleton<IInitializeLogging>(x => InitializeLogging.Create(x.GetService<IScyllaDbContext>(), x.GetService<IQueryLogContext>()));
    }
}
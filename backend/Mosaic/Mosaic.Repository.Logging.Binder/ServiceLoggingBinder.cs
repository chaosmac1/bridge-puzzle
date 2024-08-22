using Microsoft.Extensions.DependencyInjection;
using Mosaic.Repository.Collection.Adapter.Export;
using Mosaic.Repository.Logging.Adapter.Interface;
using Mosaic.Repository.Logging.Domain;

namespace Mosaic.Repository.Logging.Binder;

public class ServiceLoggingBinder: IServiceBinder {
    private ServiceLoggingBinder() { }
    
    public static ServiceLoggingBinder Create() => new ServiceLoggingBinder();
    
    public void Bind(ServiceCollection serviceCollection) {
        serviceCollection.AddScoped<ILogId>(x => LogId.New);
        serviceCollection.AddScoped<IInitializeLogging>(x => InitializeLogging.Create());
    }
}
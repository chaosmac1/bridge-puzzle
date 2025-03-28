using Microsoft.Extensions.DependencyInjection;
using Mosaic.Repository.Logging.Adapter;
using Mosaic.Repository.Postgresql.Adapter;
using Mosaic.Repository.ScyllaDb.Adapter;

namespace Mosaic.Init;

public class InitializeRepository {
    private InitializeRepository() { }
    
    public static InitializeRepository Create() => new ();

    public void Run(IServiceCollection serviceCollection) {
        Repository.Environment.Binder.ServiceEnvironmentBinder.Create().Bind(serviceCollection);
        Repository.Postgresql.Binder.ServicePostgresqlBinder.Create().Bind(serviceCollection);
        Repository.ScyllaDb.Binder.ServiceScyllaDbBinder.Create().Bind(serviceCollection);
        Repository.Logging.Binder.ServiceLoggingBinder.Create().Bind(serviceCollection);
    }
}
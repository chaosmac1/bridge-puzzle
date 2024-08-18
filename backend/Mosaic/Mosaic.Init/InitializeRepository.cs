using Microsoft.Extensions.DependencyInjection;
using Mosaic.Repository.Postgresql.Adapter;
using Mosaic.Repository.ScyllaDb.Adapter;

namespace Mosaic.Init;

public class InitializeRepository {
    private InitializeRepository() { }
    
    public static InitializeRepository Create() => new ();

    public void Run() {
        Mosaic.Repository.Collection.Loader.CreateLoader.Create()
              .Bind(Repository.Environment.Binder.ServiceEnvironmentBinder.Create())
              .Bind(Repository.Postgresql.Binder.ServicePostgresqlBinder.Create())
              .Bind(Repository.ScyllaDb.Binder.ServiceScyllaDbBinder.Create())
              .Build()
        ;

        using var asyncScope = Mosaic.Repository.Collection.Adapter.Injection.GlobalServiceProvider.CreateAsyncScope();
        var serviceProvider = asyncScope.ServiceProvider;
        serviceProvider.GetNpgsqlTableCreator().CreateTableAsync().GetAwaiter().GetResult();
        serviceProvider.GetScyllaDbTableCreator().CreateTableAsync().GetAwaiter().GetResult();
    }
}
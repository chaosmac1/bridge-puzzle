using Microsoft.Extensions.DependencyInjection;
using Mosaic.Repository.Collection.Adapter.Export;
using Mosaic.Repository.Environment.Adapter.Interface;

namespace Mosaic.Repository.Environment.Binder;

public class ServiceEnvironmentBinder: IServiceBinder {
    private ServiceEnvironmentBinder() { }

    public static IServiceBinder Create() => new ServiceEnvironmentBinder();
    
    public void Bind(ServiceCollection serviceCollection) {
        serviceCollection.AddSingleton<IEnvProvider>(provider => new Domain.EnvProvider());
        serviceCollection.AddSingleton<IEnvJson>(provider => provider.GetService<IEnvProvider>()!.EnvJson);
        serviceCollection.AddSingleton<IEnvDb>(provider => provider.GetService<IEnvProvider>()!.EnvDb);
    }
}
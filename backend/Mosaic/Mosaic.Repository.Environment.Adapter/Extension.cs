using Microsoft.Extensions.DependencyInjection;
using Mosaic.Repository.Environment.Adapter.Interface;

namespace Mosaic.Repository.Environment.Adapter;

public static class Extension {
    public static IEnvJson GetEnvJson(this IServiceProvider self) => self.GetService<IEnvJson>() ?? throw new InvalidOperationException();
    public static IEnvProvider GetEnvProvider(this IServiceProvider self) => self.GetService<IEnvProvider>() ?? throw new InvalidOperationException();
}
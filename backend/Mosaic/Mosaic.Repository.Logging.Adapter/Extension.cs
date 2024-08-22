using Microsoft.Extensions.DependencyInjection;
using Mosaic.Repository.Logging.Adapter.Interface;

namespace Mosaic.Repository.Logging.Adapter;

public static class Extension {
    public static IInitializeLogging GetInitializeLogging(this IServiceProvider self) => self.GetService<IInitializeLogging>() ?? throw new InvalidOperationException();
}
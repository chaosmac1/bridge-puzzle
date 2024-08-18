using Microsoft.Extensions.DependencyInjection;

namespace Mosaic.Repository.ScyllaDb.Adapter;

public static class Extension {
    public static IScyllaDbContext GetScyllaDbContext(this IServiceProvider self) => self.GetService<IScyllaDbContext>() ?? throw new InvalidOperationException();
    public static IScyllaDbTableCreator GetScyllaDbTableCreator(this IServiceProvider self) => self.GetService<IScyllaDbTableCreator>() ?? throw new InvalidOperationException();
}
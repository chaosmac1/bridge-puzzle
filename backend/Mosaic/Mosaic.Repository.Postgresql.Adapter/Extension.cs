
using Microsoft.Extensions.DependencyInjection;

namespace Mosaic.Repository.Postgresql.Adapter;

public static class Extension {
    public static INpgsqlContext GetNpgsqlContext(this IServiceProvider self) => self.GetService<INpgsqlContext>() ?? throw new InvalidOperationException();
    public static INpgsqlTableCreator GetNpgsqlTableCreator(this IServiceProvider self) => self.GetService<INpgsqlTableCreator>() ?? throw new InvalidOperationException();
}
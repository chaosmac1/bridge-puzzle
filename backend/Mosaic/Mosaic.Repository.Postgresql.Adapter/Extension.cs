
using Microsoft.Extensions.DependencyInjection;
using Mosaic.Repository.Postgresql.Adapter.Interface;

namespace Mosaic.Repository.Postgresql.Adapter;

public static class Extension {
    public static INpgsqlContext GetNpgsqlContext(this IServiceProvider self) 
        => self.GetService<INpgsqlContext>() ?? throw new InvalidOperationException();
    public static INpgsqlTableCreator GetNpgsqlTableCreator(this IServiceProvider self) 
        => self.GetService<INpgsqlTableCreator>() ?? throw new InvalidOperationException();
}
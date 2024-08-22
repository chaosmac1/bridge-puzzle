using Cassandra;
using Cassandra.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Mosaic.Repository.ScyllaDb.Adapter.Interface;

namespace Mosaic.Repository.ScyllaDb.Adapter;

public static class Extension {
    public static IScyllaDbContext GetScyllaDbContext(this IServiceProvider self) => self.GetService<IScyllaDbContext>() ?? throw new InvalidOperationException();
    public static IScyllaDbTableCreator GetScyllaDbTableCreator(this IServiceProvider self) => self.GetService<IScyllaDbTableCreator>() ?? throw new InvalidOperationException();
    public static IQueryLogContext GetQueryLogContext(this IServiceProvider self) => self.GetService<IQueryLogContext>() ?? throw new InvalidOperationException();
    public static Mapper GetMapper(this IServiceProvider self) => self.GetService<Mapper>() ?? throw new InvalidOperationException();
    public static Task<ISession> GetSessionAsync(this IServiceProvider self) => self.GetService<Task<ISession>>() ?? throw new InvalidOperationException();
    
    public static IReadOnlyLog AsReadOnlyLog(this IReadOnlyLog self) => self; 
}
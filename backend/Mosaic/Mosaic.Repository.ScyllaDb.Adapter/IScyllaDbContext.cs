using Cassandra;
using Cassandra.Mapping;

namespace Mosaic.Repository.ScyllaDb.Adapter;

public interface IScyllaDbContext : IDisposable {
    public ValueTask<ISession> GetDbAsync();
    public ValueTask<Mapper> GetMapperAsync();
    
    public Task WaitIsReadyAsync();
    
    public bool IsDispose { get; }
}
using Cassandra;

namespace Mosaic.Repository.ScyllaDb.Adapter;

public interface IScyllaDbContext : IDisposable {
    public ValueTask<ISession> GetDbAsync();
    
    public bool IsDispose { get; }
}
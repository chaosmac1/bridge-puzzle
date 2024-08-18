using Cassandra;
using Mosaic.Repository.ScyllaDb.Adapter;

namespace Mosaic.Repository.ScyllaDb.Domain;

public class ScyllaDbContext: IScyllaDbContext {
    public bool IsDispose { get; private set; }
    
    private readonly Task<ISession> _session;
    
    public ScyllaDbContext() {
        _session = Domain.ScyllaDbBuilder.CreateNewSessionAsync();
    }
    
    public ValueTask<ISession> GetDbAsync() {
        if (IsDispose) {
            throw new ObjectDisposedException("ScyllaDbContext");
        }
        if (_session.IsCompletedSuccessfully) {
            return ValueTask.FromResult(_session.Result);
        }

        return new ValueTask<ISession>(_session);
    }
    
    public void Dispose() {
        if (IsDispose) {
            return;
        }
        
        var ses = _session.GetAwaiter().GetResult();
        ses.Dispose();
        
        IsDispose = true;
        
        GC.SuppressFinalize(this);
    }
}
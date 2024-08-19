using Cassandra;
using Cassandra.Mapping;
using Mosaic.Repository.ScyllaDb.Adapter;

namespace Mosaic.Repository.ScyllaDb.Domain;

public class ScyllaDbContext: IScyllaDbContext {
    public Task WaitIsReadyAsync() {
        throw new NotImplementedException();
    }

    public bool IsDispose { get; private set; }
    
    private readonly Task<ISession> _session;
    private Task<Mapper> _mapper;
    
    public ScyllaDbContext() {
        _session = Domain.ScyllaDbBuilder.CreateNewSessionAsync();
        _mapper = GetMapperTask(_session);
        
        return;

        static async Task<Mapper> GetMapperTask(Task<ISession> sessionTask) => new(await sessionTask);
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

    public ValueTask<Mapper> GetMapperAsync() {
        if (IsDispose) {
            throw new ObjectDisposedException("ScyllaDbContext");
        }

        if (this._mapper.IsCompleted) {
            return ValueTask.FromResult(this._mapper.Result);
        }

        return new ValueTask<Mapper>(_mapper);
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
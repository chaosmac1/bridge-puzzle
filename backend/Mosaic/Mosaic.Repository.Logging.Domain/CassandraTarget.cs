using Cassandra;
using Mosaic.Repository.ScyllaDb.Adapter;
using Mosaic.Repository.ScyllaDb.Adapter.Interface;
using NLog;
using NLog.Common;
using NLog.Targets;

namespace Mosaic.Repository.Logging.Domain;

public class CassandraTarget: AsyncTaskTarget {
    private bool _disposed = false;
    private readonly object _connectionLock = new object();
    private readonly IScyllaDbContext _dbContext;
    private readonly IQueryLogContext _queryLogContext;

    public CassandraTarget() {
        _dbContext = Collection.Adapter.Injection.GlobalServiceProvider.GetScyllaDbContext();
        _queryLogContext = Collection.Adapter.Injection.GlobalServiceProvider.GetQueryLogContext();
    }

    protected override async Task WriteAsyncTask(LogEventInfo logEvent, CancellationToken cancellationToken) {
        if (this._disposed) {
            throw new ObjectDisposedException(nameof(CassandraTarget));
        }
        
        
        
        await this.EnsureNothingIsDisposedAsync();
        
        var logDto = logEvent.ToLogDto();
        await _queryLogContext.InsertLogAsync(logDto);
    }

    protected override async Task WriteAsyncTask(IList<LogEventInfo> logEvents, CancellationToken cancellationToken) {
        if (this._disposed) {
            throw new ObjectDisposedException(nameof(CassandraTarget));
        }
        if (logEvents.Count == 1) {
            await WriteAsyncTask(logEvents[0], cancellationToken);
            return;
        }
        
        await this.EnsureNothingIsDisposedAsync();
        
        await _queryLogContext.InsertLogsAsync(logEvents.Select(x => x.ToLogDto().AsReadOnlyLog()).ToList());
        for (int i = 0; i < logEvents.Count; i++) {
            logEvents[i] = null!;
        }
    }

    private async ValueTask EnsureNothingIsDisposedAsync() {
        ISession session  = await this._dbContext.GetDbAsync();
        
        if (_disposed) {
            throw new ObjectDisposedException(nameof(CassandraTarget));
        }

        if (this._dbContext.IsDispose) {
            throw new ObjectDisposedException(nameof(IScyllaDbContext));
        }
        
        if (session.IsDisposed) {
            throw new ObjectDisposedException(nameof(ISession));
        }
    }

    

    protected override void Dispose(bool disposing) {
        if (!this._disposed) {
            return;
        }
        
        this._disposed = true;
        this._dbContext.Dispose();
        
        base.Dispose(disposing);
        GC.SuppressFinalize(this);
    }
}
using System.Data;
using LamLibAllOver.ErrorHandling;
using Microsoft.Extensions.Logging;
using Mosaic.Repository.Postgresql.Kernel;
using Npgsql;

namespace Mosaic.Repository.Postgresql.Adapter;

public interface INpgsqlContext: IAsyncDisposable, IDisposable {
    public ValueTask<NpgsqlConnection> GetDbAsync();
    public EDisposeHandling DisposeHandling { get; }
    public Option<NpgsqlTransaction> NpgsqlTransaction { get; }
    public ETransactionStatus TransactionStatus { get; }
    public bool IsDispose { get; }

    public Task StartTransactionAsync(IsolationLevel isolationLevel);

    public Task CommitTransactionAsync();

    public Task RollbackTransactionAsync();

    public new void Dispose() {
        this.DisposeAsync().GetAwaiter().GetResult();
    }
}
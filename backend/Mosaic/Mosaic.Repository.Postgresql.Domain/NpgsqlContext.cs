using System.Data;
using LamLibAllOver.ErrorHandling;
using Npgsql;
using Mosaic.Repository.Postgresql.Adapter;

namespace Mosaic.Repository.Postgresql.Domain;

public class NpgsqlContext: INpgsqlContext {
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    private bool _isInit = false;
    private Task<NpgsqlConnection> _db;

    public ValueTask<NpgsqlConnection> GetDbAsync() {
        return _isInit 
            ? ValueTask.FromResult(_db.Result) 
            : new ValueTask<NpgsqlConnection>(InitAsync());
    }
    
    public Kernel.EDisposeHandling DisposeHandling { get; }
    public Option<NpgsqlTransaction> NpgsqlTransaction { get; private set; }
    public Kernel.ETransactionStatus TransactionStatus { get; private set; }
    public bool IsDispose { get; private set; } = false;

    private async Task<NpgsqlConnection> InitAsync() {
        var db = await _db;
        await EnsureOpenAsync(db);
        _isInit = true;
        return db;
    }
    
    public NpgsqlContext() {
        _db = NpgsqlBuilder.BuildNpgsqlConnection();
        DisposeHandling = Kernel.EDisposeHandling.TransactionRollback;
    }
    
    /// <exception cref="Exception"></exception>
    public async Task StartTransactionAsync(IsolationLevel isolationLevel) {
        var db = await GetDbAsync();
        
        if (IsDispose) {
            Logger.Error(ErrorIsDispose);
            throw new Exception(ErrorStartTransactionAsync.AddExtra(ErrorIsDispose).ToString());
        }
        
        if (TransactionStatus == Kernel.ETransactionStatus.Active) {
            return;
        }

        NpgsqlTransaction = Option<NpgsqlTransaction>.With(await db.BeginTransactionAsync(isolationLevel));
    }

    public async Task CommitTransactionAsync() {
        var db = await GetDbAsync();
            
        if (db.State != ConnectionState.Open) {
            throw new Exception(ErrorDatabaseIsNotOpen.AddStackTrace().ToString());
        }
        if (TransactionStatus != Kernel.ETransactionStatus.Active) {
            throw new Exception(ErrorTransactionNotActive.AddStackTrace().ToString());
        }

        await NpgsqlTransaction.Unwrap().CommitAsync();
    }
    
    public async Task RollbackTransactionAsync() {
        var db = await GetDbAsync();
            
        if (db.State != ConnectionState.Open) {
            throw new Exception(ErrorDatabaseIsNotOpen.AddStackTrace().ToString());
        }
        if (TransactionStatus != Kernel.ETransactionStatus.Active) {
            throw new Exception(ErrorTransactionNotActive.AddStackTrace().ToString());
        }

        await NpgsqlTransaction.Unwrap().RollbackAsync();
    }
    
    public async ValueTask DisposeAsync() {
        if (IsDispose) return;
        IsDispose = true;
        if (this.TransactionStatus == Kernel.ETransactionStatus.IsActive) {
            switch (this.DisposeHandling) {
                case Kernel.EDisposeHandling.TransactionRollback:
                    await this.RollbackTransactionAsync();
                    break;
                case Kernel.EDisposeHandling.TransactionCommit:
                    await this.CommitTransactionAsync();
                    break;
                default:
                case Kernel.EDisposeHandling.TransactionNothing:
                    break;
            }
        }

        if (this.NpgsqlTransaction.IsSet()) {
            await this.NpgsqlTransaction.Unwrap().DisposeAsync();
        }

        var db = await GetDbAsync();
        
        await db.DisposeAsync();
    }
    
    public enum ETransactionStatus {
        Active,
        IsActive,
    }

    public enum EDisposeHandling {
        TransactionRollback,
        TransactionCommit,
        TransactionNothing
    }
    
    private static ValueTask EnsureOpenAsync(
        NpgsqlConnection connection, 
        CancellationToken cancellationToken = default) {
        
        return connection.State != ConnectionState.Open 
            ? new ValueTask(connection.OpenAsync(cancellationToken)) 
            : ValueTask.CompletedTask;
    }

    private static ErrorDomain ErrorStartTransactionAsync => new ErrorDomain(
        "Can Not Begin Transaction",
        "Error By Start Transaction",
        typeof(NpgsqlContext).FullName??""
    );
    
    private static ErrorDomain ErrorCommitTransactionAsync => new ErrorDomain(
        "Can Not Commit Transaction",
        "Error By Commit Transaction",
        typeof(NpgsqlContext).FullName??""
    );
    
    private static ErrorDomain ErrorRollbackTransactionAsync => new ErrorDomain(
        "Can Not Rollback Transaction",
        "Error By Rollback Transaction",
        typeof(NpgsqlContext).FullName??""
    );
    
    private static ErrorDomain ErrorDatabaseIsNotOpen => new ErrorDomain(
        "Database is not Open",
        "Database Not Active",
        typeof(NpgsqlContext).FullName??""
    );
    
    private static ErrorDomain ErrorTransactionNotActive => new ErrorDomain(
        "Database Transaction Is Not Active",
        "Transaction Is Inactive",
        typeof(NpgsqlContext).FullName??""
    );
    
    private static ErrorDomain ErrorIsDispose => new ErrorDomain(
        "Cam Not Reuse Status Is Disposed",
        "IsDispose",
        typeof(NpgsqlContext).FullName??""
    );

    public void Dispose() {
        this.DisposeAsync().GetAwaiter().GetResult();
    }
}
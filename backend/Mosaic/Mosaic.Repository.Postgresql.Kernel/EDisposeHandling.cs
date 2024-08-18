namespace Mosaic.Repository.Postgresql.Kernel;

public enum EDisposeHandling {
    TransactionRollback,
    TransactionCommit,
    TransactionNothing
}
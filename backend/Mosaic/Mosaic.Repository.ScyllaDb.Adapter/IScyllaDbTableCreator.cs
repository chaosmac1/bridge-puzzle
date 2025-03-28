namespace Mosaic.Repository.ScyllaDb.Adapter;

public interface IScyllaDbTableCreator {
    public Task CreateTablesAsync(CancellationToken cancellationToken = default);
}
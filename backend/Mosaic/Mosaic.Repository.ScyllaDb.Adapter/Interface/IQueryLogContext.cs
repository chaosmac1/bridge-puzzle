namespace Mosaic.Repository.ScyllaDb.Adapter.Interface;

public interface IQueryLogContext {
    public Task InsertLogAsync(IReadOnlyLog log);
    public Task InsertLogsAsync(List<IReadOnlyLog> logs);
}
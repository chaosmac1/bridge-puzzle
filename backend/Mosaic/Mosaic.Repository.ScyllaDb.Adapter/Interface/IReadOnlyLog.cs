using Cassandra;

namespace Mosaic.Repository.ScyllaDb.Adapter.Interface;

public interface IReadOnlyLog {
    public DateOnly Date { get; }
    public DateTime DateTime { get; }
    public TimeUuid Id { get; }
    public string Message { get; }
    public string Status { get; }
    public string Stack { get; }
    public string Trigger { get; }
}
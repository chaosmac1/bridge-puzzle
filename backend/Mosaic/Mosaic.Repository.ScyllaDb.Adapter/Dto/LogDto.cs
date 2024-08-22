using Cassandra;
using Mosaic.Repository.ScyllaDb.Adapter.Interface;

namespace Mosaic.Repository.ScyllaDb.Adapter.Dto;

public class LogDto: IReadOnlyLog {
    public required DateOnly Date { get; init; }
    public required DateTime DateTime { get; init; }
    public required TimeUuid Id { get; init; }
    public required string Message { get; init; }
    public required string Status { get; init; }
    public required string Stack { get; init; }
    public required string Trigger { get; init; }
}
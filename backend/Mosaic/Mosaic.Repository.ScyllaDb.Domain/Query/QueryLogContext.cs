using Cassandra;
using Cassandra.Mapping;
using Mosaic.Repository.ScyllaDb.Adapter.Interface;
using Mosaic.Repository.ScyllaDb.Kernel.Entities;

namespace Mosaic.Repository.ScyllaDb.Domain.Query;

public class QueryLogContext: IQueryLogContext {
    private readonly Mapper _mapper;

    public QueryLogContext(Mapper mapper) {
        this._mapper = mapper;
    }


    public async Task InsertLogAsync(IReadOnlyLog log) {
        await _mapper.InsertAsync(Log.ConvertIfNotLog(log));
    }

    public async Task InsertLogsAsync(List<IReadOnlyLog> logs) {
        var batch = _mapper.CreateBatch();
        foreach (var log in Log.ConvertsIfNotLog(logs)) {
            batch.Insert(log);
        }
        
        await _mapper.ExecuteAsync(batch);
    }
}
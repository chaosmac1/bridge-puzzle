using Mosaic.Repository.ScyllaDb.Adapter;

namespace Mosaic.Repository.ScyllaDb.Domain;

public class ScyllaDbTableCreator: IScyllaDbTableCreator {
    private readonly IScyllaDbContext _scyllaDbContext;

    public ScyllaDbTableCreator(IScyllaDbContext scyllaDbContext) {
        this._scyllaDbContext = scyllaDbContext;
    }

    public async Task CreateTableAsync() {
        await _scyllaDbContext.GetDbAsync();
    }
}
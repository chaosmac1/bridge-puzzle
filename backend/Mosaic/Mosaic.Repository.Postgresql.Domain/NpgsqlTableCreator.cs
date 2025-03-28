using Mosaic.Repository.Postgresql.Adapter;
using Mosaic.Repository.Postgresql.Adapter.Interface;

namespace Mosaic.Repository.Postgresql.Domain;

public class NpgsqlTableCreator: INpgsqlTableCreator {
    private readonly INpgsqlContext npgsqlContext;

    public NpgsqlTableCreator(INpgsqlContext npgsqlContext) {
        this.npgsqlContext = npgsqlContext;
    }

    public async Task CreateTablesAsync() {
        var db = await npgsqlContext.GetDbAsync();
        throw new NotImplementedException(nameof(CreateTablesAsync));
    }
}
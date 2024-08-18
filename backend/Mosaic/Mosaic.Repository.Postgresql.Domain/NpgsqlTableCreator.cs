using Mosaic.Repository.Postgresql.Adapter;

namespace Mosaic.Repository.Postgresql.Domain;

public class NpgsqlTableCreator: INpgsqlTableCreator {
    private readonly INpgsqlContext npgsqlContext;

    public NpgsqlTableCreator(INpgsqlContext npgsqlContext) {
        this.npgsqlContext = npgsqlContext;
    }

    public async Task CreateTableAsync() {
        await npgsqlContext.GetDbAsync();
        // throw new NotImplementedException(nameof(CreateTableAsync));
    }
}
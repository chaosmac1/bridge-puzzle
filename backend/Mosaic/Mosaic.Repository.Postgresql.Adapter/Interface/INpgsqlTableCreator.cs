namespace Mosaic.Repository.Postgresql.Adapter.Interface;

public interface INpgsqlTableCreator {
    public Task CreateTablesAsync();
}
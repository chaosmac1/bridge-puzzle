namespace Mosaic.Share.Kernel.Database;

public interface ITableCreator {
    public Task CreateTableAsync();
}
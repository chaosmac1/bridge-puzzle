namespace Mosaic.Repository.ScyllaDb.Adapter.Interface;

public interface ToDto<T> {
    public T ToDto();
}
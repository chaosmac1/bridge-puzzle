using Mosaic.Share.Kernel.Word;
using Mosaic.Share.Kernel.WordWithVector;

namespace Mosaic.Repository.Postgresql.Adapter.Query;

public interface IQueryWordVectorSpaceContext {
    public Task InsertBulkAsync(IReadOnlyWordWithVector[] wordWithVectors);

    public Task<IReadOnlyWordWithVector[]> FilterByWordsAsync(IReadOnlyWord[] words);
}
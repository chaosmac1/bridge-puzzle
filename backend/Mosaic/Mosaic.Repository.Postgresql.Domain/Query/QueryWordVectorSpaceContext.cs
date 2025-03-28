using Dapper;
using Mosaic.Repository.Postgresql.Kernel;
using Mosaic.Repository.Postgresql.Kernel.Entities;
using Mosaic.Share.Kernel.Word;
using Mosaic.Share.Kernel.WordWithVector;

namespace Mosaic.Repository.Postgresql.Domain.Query;

public class QueryWordVectorSpaceContext {
    private readonly NpgsqlContext _npgsqlContext;
    public QueryWordVectorSpaceContext(NpgsqlContext npgsqlContext) {
        _npgsqlContext = npgsqlContext;
    }

    public async Task InsertBulkAsync(IReadOnlyWordWithVector[] wordWithVectors) {
        var sql = """
                  insert into "WordVectorSpace" ("Word", "V300") VALUES (@Word, @V300)
                  """;
        await (await _npgsqlContext.GetDbAsync())
            .ExecuteAsync(sql, wordWithVectors);
    }

    public async Task<IReadOnlyWordWithVector[]> FilterByWordsAsync(IReadOnlyWord[] words) {
        var sql = """
                  SELECT * 
                  FROM "WordVectorSpace"
                  WHERE Word in (@Words)
                  """; 
        
        var strs = words.Select(x => x.Name).ToArray();

        return (await (await _npgsqlContext.GetDbAsync()).QueryAsync<WordVectorSpace>(sql, new {Words = strs})
            ).Select(x => x.ToWordWithVectorDto())
             .ToArray<IReadOnlyWordWithVector>();
    }
}
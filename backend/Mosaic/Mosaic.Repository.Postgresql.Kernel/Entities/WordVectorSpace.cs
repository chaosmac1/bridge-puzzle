using Mosaic.Share.Kernel.WordWithVector;
using Pgvector;

namespace Mosaic.Repository.Postgresql.Kernel.Entities;

public class WordVectorSpace {
    public string Word { get; set; } = "";
    public Pgvector.Vector V300 { get; set; } = new (new ReadOnlyMemory<float>());
    
    public static WordVectorSpace FromWordWithVector(IReadOnlyWordWithVector wordVectorSpace) {
        return new WordVectorSpace {
            Word = wordVectorSpace.Name,
            V300 = wordVectorSpace.Vector
        };
    }
    
    public static WordVectorSpace[] FromWordWithVectors(IList<IReadOnlyWordWithVector> wordVectorSpaces) {
        var res = new WordVectorSpace[wordVectorSpaces.Count];

        for (var i = 0; i < wordVectorSpaces.Count; i++) {
            res[i] = WordVectorSpace.FromWordWithVector(wordVectorSpaces[i]);
        }

        return res;
    }
}
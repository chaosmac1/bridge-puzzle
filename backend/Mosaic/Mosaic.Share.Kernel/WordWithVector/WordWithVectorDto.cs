using Mosaic.Share.Kernel;
using Mosaic.Share.Kernel.Word;
using Mosaic.Share.Kernel.WordWithVector;
using Pgvector;

public class WordWithVectorDto: IReadOnlyWordWithVector {
    public string Name { get; }
    public Pgvector.Vector Vector { get; }
    
    public WordWithVectorDto(string name, Vector vector) {
        Name = name;
        Vector = vector;
    }

    public float ComputeDistance(IReadOnlyWordWithVector wordWithVector) {
        return this.Vector.ComputeDistance(wordWithVector.Vector);
    }

    public (IReadOnlyWordWithVector WordWithVector, float Distance)[] ComputeDistances(IReadOnlyCollection<IReadOnlyWordWithVector> wordWithVectors) {
        return wordWithVectors.Select(x => (x, this.ComputeDistance(x))).ToArray();
    }

    public IReadOnlyWordWithVector GetNears(IReadOnlyCollection<IReadOnlyWordWithVector> wordWithVectors) {
        return ComputeDistances(wordWithVectors)
               .OrderBy(x => x.Distance)
               .First().WordWithVector
        ;
    }

    public Pgvector.Vector ComputeMidpoint(IReadOnlyWordWithVector wordWithVector) {
        return this.Vector.ComputeMidpoint(wordWithVector.Vector);
    }
}
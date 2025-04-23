using Mosaic.Share.Kernel;
using Mosaic.Share.Kernel.ValueObject;
using Mosaic.Share.Kernel.Word;
using Mosaic.Share.Kernel.WordWithVector;
using Pgvector;

namespace Mosaic.Share.Kernel.WordWithVector;

public sealed class WordWithVectorDto: IReadOnlyWordWithVector {
    public string Name { get; }
    public Vector300 Vector { get; }
    
    public WordWithVectorDto(string name, Vector300 vector) {
        Name = name;
        Vector = vector;
    }

    public float ComputeDistance(IReadOnlyWordWithVector wordWithVector) {
        var vector = wordWithVector.Vector;
        return this.Vector.ComputeDistance(ref vector);
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

    public Vector300 ComputeMidpoint(IReadOnlyWordWithVector wordWithVector) {
        var vector = wordWithVector.Vector;
        return this.Vector.ComputeMidpoint(ref vector);
    }
}
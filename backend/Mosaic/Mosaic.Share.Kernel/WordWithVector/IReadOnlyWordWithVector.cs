using Mosaic.Share.Kernel.Word;

namespace Mosaic.Share.Kernel.WordWithVector;

public interface IReadOnlyWordWithVector: IReadOnlyWord  {
    public string Name { get; }
    public Pgvector.Vector Vector { get; }

    public float ComputeDistance(IReadOnlyWordWithVector wordWithVector);

    public (IReadOnlyWordWithVector WordWithVector, float Distance)[] ComputeDistances(IReadOnlyCollection<IReadOnlyWordWithVector> wordWithVectors);

    public IReadOnlyWordWithVector GetNears(IReadOnlyCollection<IReadOnlyWordWithVector> wordWithVectors);

    public Pgvector.Vector ComputeMidpoint(IReadOnlyWordWithVector wordWithVector);
}
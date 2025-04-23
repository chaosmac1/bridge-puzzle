using Mosaic.Share.Kernel.ValueObject;
using Mosaic.Share.Kernel.Word;

namespace Mosaic.Share.Kernel.WordWithVector;

public interface IReadOnlyWordWithVector: IReadOnlyWord  {
    public string Name { get; }
    public Vector300 Vector { get; }

    public float ComputeDistance(IReadOnlyWordWithVector wordWithVector);

    public (IReadOnlyWordWithVector WordWithVector, float Distance)[] ComputeDistances(IReadOnlyCollection<IReadOnlyWordWithVector> wordWithVectors);

    public IReadOnlyWordWithVector GetNears(IReadOnlyCollection<IReadOnlyWordWithVector> wordWithVectors);

    public Vector300 ComputeMidpoint(IReadOnlyWordWithVector wordWithVector);
}
namespace Mosaic.Slice.BridgePuzzle.Domain;

public sealed class PuzzleSolutionResponse {
    public required string[] SolutionWords { get; init; }
    public required string SolutionMiddleWord { get; init; }
}
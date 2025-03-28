namespace Mosaic.Slice.BridgePuzzle.Domain;

public sealed class PuzzleSolutionRequest {
    public required PuzzleSolutionRequestRow[] Rows { get; set; }
    public required string[] AllowedWords { get; set; }

    public int MiddleWordLength() => Rows.Length;
    
    public sealed class PuzzleSolutionRequestRow {
        public required string LeftWord { get; set; }
        public required string RightWord { get; set; }
        public required uint Space { get; set; }
        public required uint SpaceMiddle { get; set; }
    }
}
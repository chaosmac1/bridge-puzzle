using System.Collections.Frozen;
using FastEndpoints;
using Mosaic.Repository.Environment.Adapter.Interface;
using Mosaic.Repository.Postgresql.Adapter.Interface;
using Mosaic.Repository.Postgresql.Adapter.Query;
using Mosaic.Share.Kernel;
using Mosaic.Share.Kernel.ValueObject;
using Mosaic.Share.Kernel.Word;
using Mosaic.Share.Kernel.WordWithVector;
using Pgvector;

namespace Mosaic.Slice.BridgePuzzle.Domain;

public class PuzzleSolutionEndpoint : Endpoint<PuzzleSolutionRequest, PuzzleSolutionResponse> {
    private readonly IEnvJson _envJson;
    private readonly INpgsqlContext _npgsqlContext;
    private readonly IQueryWordVectorSpaceContext _queryWordVectorSpaceContext;

    public PuzzleSolutionEndpoint(IEnvJson envJson, INpgsqlContext npgsqlContext, IQueryWordVectorSpaceContext queryWordVectorSpaceContext) {
        _envJson = envJson;
        _npgsqlContext = npgsqlContext;
        _queryWordVectorSpaceContext = queryWordVectorSpaceContext;
    }

    public override void Configure() {
        Post("/Api/V1/puzzle/solution");
        AllowAnonymous();
    }

    public override async Task HandleAsync(PuzzleSolutionRequest req, CancellationToken ct) {
        IReadOnlyWordWithVector[] wordWithVectors = await _queryWordVectorSpaceContext.FilterByWordsAsync(req.AllowedWords.Select(x => new Word(x)).ToArray<IReadOnlyWord>());
        FrozenDictionary<string, IReadOnlyWordWithVector> words = (await _queryWordVectorSpaceContext.FilterByWordsAsync(
            req.Rows
               .Select(x => new Word[] { new Word(x.LeftWord), new Word(x.RightWord) })
               .SelectMany(x => x)
               .ToArray<IReadOnlyWord>()
        )).ToFrozenDictionary(x => x.Name);

        if (wordWithVectors.Length != req.AllowedWords.Length) {
            await this.SendErrorsAsync(400, ct);
            return;
        }
        
        var rows = req.Rows.Select(x => Row.Create(words[x.LeftWord], words[x.RightWord], x.Space)).ToArray();
        while (rows.Length != 1) {
            var anyTrue = false;

            foreach (var rowNow in rows) {
                foreach (var rowNext in rows) {
                    anyTrue = rowNow.SwitchFoundWordDistanceIfBetter(rowNext);
                }
            }
            
            if (anyTrue) {
                continue;
            }  
            break;
        }
        
        await SendOkAsync(new PuzzleSolutionResponse() {
            SolutionMiddleWord = new string(req.Rows.Select(reqX => {
                return rows.First(x => reqX.LeftWord == x.Left.Name && reqX.RightWord == x.Right.Name)!
                           .FoundWordWithVector!.Name[(int)reqX.SpaceMiddle];
            }).ToArray()),
            SolutionWords = rows.Select(x => x.FoundWordWithVector!.Name).ToArray(),
        }, ct);
    }

    private class Row {
        public IReadOnlyWordWithVector Left { get; }
        public IReadOnlyWordWithVector Right { get; }
        public Vector300 MidVector { get; }
        public int Space { get; }
        public float FoundWordDistance { get; set; }
        public IReadOnlyWordWithVector? FoundWordWithVector { get; private set; }
        
        public Row(IReadOnlyWordWithVector left, IReadOnlyWordWithVector right, Vector300 midVector, uint space) {
            Left = left;
            Right = right;
            MidVector = midVector;
            Space = (int)space;
        }

        public static Row Create(IReadOnlyWordWithVector left, IReadOnlyWordWithVector right, uint space) {
            return new Row(left, right, left.ComputeMidpoint(right), space);
        }

        public void PutNearWordWithVectorAndRemoveFromList(List<IReadOnlyWordWithVector> readOnlyWordWithVectors) {
            var nearVector = this.MidVector.GetNears(readOnlyWordWithVectors.Select(x => x.Vector).ToArray());
            
            for (var i = 0; i < readOnlyWordWithVectors.Count; i++) {
                var near = readOnlyWordWithVectors[i];
                if (near.Vector != nearVector) {
                    continue;
                }
                FoundWordWithVector = near;
                FoundWordDistance = MidVector.ComputeDistance(ref nearVector);
                readOnlyWordWithVectors.RemoveAt(i);
                return;
            }

            throw new Exception("WordWithVector Not Found By self Vector");
        }

        public bool SwitchFoundWordDistanceIfBetter(Row row) {
            if (Object.ReferenceEquals(this, row) || row.Space != this.Space) {
                return false;
            }

            var rowVec = row.FoundWordWithVector!.Vector;
            var foundVec = this.FoundWordWithVector!.Vector;
            
            float selfLower = this.MidVector.ComputeDistance(ref rowVec);
            float rowLower = row.MidVector.ComputeDistance(ref foundVec);

            if (!(this.FoundWordDistance >= selfLower) || !(row.FoundWordDistance >= rowLower)) {
                return false;
            }
            
            (this.FoundWordWithVector, row.FoundWordWithVector) = (row.FoundWordWithVector, this.FoundWordWithVector);
            return true;
        }
    }
}
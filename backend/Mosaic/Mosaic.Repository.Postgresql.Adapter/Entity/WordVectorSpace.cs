using Mosaic.Share.Kernel.ValueObject;
namespace Mosaic.Repository.Postgresql.Adapter.Entity;

public sealed class WordVectorSpace {
    public required string Word { get; set; }
    public required Vector300 V300 { get; set; }
}
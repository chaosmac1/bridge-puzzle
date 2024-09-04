using System.Numerics;
using Mosaic.Repository.Logging.Adapter;
using Mosaic.Repository.Logging.Adapter.Interface;

namespace Mosaic.Repository.Logging.Domain;

public record struct LogId(Guid Id) : ILogId {
    public static LogId New => new LogId(Guid.NewGuid());
}
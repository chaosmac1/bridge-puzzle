using Cassandra;
using Mosaic.Repository.ScyllaDb.Adapter.Dto;
using NLog;
using NLog.Fluent;

namespace Mosaic.Repository.Logging.Domain;

public static class Mapper {
    // public static LogDto ToLogDto(LogEventInfo logEvent) {
    //     return new LogDto() {
    //         Date = DateOnly.FromDateTime(logEvent.TimeStamp),
    //         DateTime = logEvent.TimeStamp,
    //         Id = TimeUuid.NewId(logEvent.TimeStamp),
    //         Message = logEvent.Message??"",
    //         Status = logEvent.LoggerName??"",
    //         Stack = logEvent.HasStackTrace? logEvent.StackTrace.ToString() :"",
    //         Trigger = logEvent.CallerClassName??"",
    //     };
    // }
    
    public static LogDto ToLogDto(this LogEventInfo logEvent) {
        return new LogDto() {
            Date = DateOnly.FromDateTime(logEvent.TimeStamp),
            DateTime = logEvent.TimeStamp,
            Id = TimeUuid.NewId(logEvent.TimeStamp),
            Message = logEvent.Message??"",
            Status = logEvent.LoggerName??"",
            Stack = logEvent.HasStackTrace? logEvent.StackTrace.ToString() :"",
            Trigger = logEvent.CallerClassName??"",
        };
    }
}
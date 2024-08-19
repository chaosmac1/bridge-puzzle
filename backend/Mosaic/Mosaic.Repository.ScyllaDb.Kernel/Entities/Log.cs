using System.Runtime.InteropServices.JavaScript;
using Cassandra;
using Mosaic.Repository.ScyllaDb.Adapter.Dto;
using Mosaic.Repository.ScyllaDb.Adapter.Interface;

namespace Mosaic.Repository.ScyllaDb.Kernel.Entities;

public class Log: IReadOnlyLog, ToDto<LogDto> {
    public required DateOnly Date { get; set; }
    public required DateTime DateTime { get; set; }
    public required TimeUuid Id { get; set; }
    public required string Message { get; set; }
    public required string Status { get; set; }
    public required string Stack { get; set; }
    public required string Trigger { get; set; }
    
    public LogDto ToDto() {
        return new LogDto() {
            Date = Date,
            DateTime = this.DateTime,
            Id = this.Id,
            Message = this.Message,
            Status = this.Status,
            Stack = this.Stack,
            Trigger = this.Trigger,
        };
    }

    public static Log FromDto(LogDto dto) {
        return new Log() {
            Date = dto.Date,
            DateTime = dto.DateTime,
            Id = dto.Id,
            Message = dto.Message,
            Status = dto.Status,
            Stack = dto.Stack,
            Trigger = dto.Trigger,
        };
    }
}
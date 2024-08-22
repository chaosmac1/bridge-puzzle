using Cassandra;
using Cassandra.Mapping;
using Mosaic.Repository.ScyllaDb.Kernel.Entities;

namespace Mosaic.Repository.ScyllaDb.Domain;

public class ScyllaDbMapper: Mappings {
    public static void DefineGlobalScyllaDbMapper() {
        MappingConfiguration.Global.Define<ScyllaDbMapper>();
    }

    public ScyllaDbMapper() {
        this.For<Log>()
            .TableName("log")
            .PartitionKey(u => u.Date)
            .ClusteringKey(u => u.DateTime, SortOrder.Descending)
            .ClusteringKey(u => u.Id, SortOrder.Descending)
            .Column(x => x.Date, x => x.WithDbType<DateTime>())
            .Column(x => x.Id, x => x.WithDbType<DateTime>())
            .Column(x => x.Message)
            .Column(x => x.Status)
            .Column(x => x.Stack)
            .Column(x => x.Trigger);
    }
}
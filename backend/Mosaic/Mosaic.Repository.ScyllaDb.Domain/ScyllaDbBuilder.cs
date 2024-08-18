using Cassandra;
using Mosaic.Repository.Environment.Adapter;

namespace Mosaic.Repository.ScyllaDb.Domain;

public static class ScyllaDbBuilder {
    private static Cluster? _cluster = null;
    
    private static void Init() {
        var envJson = Repository.Collection.Adapter.Injection.GlobalServiceProvider.GetEnvJson();
        _cluster = Cluster.Builder()
                          .AddContactPoint(envJson.SCYLLADB_URL)
                          .WithPort(envJson.SCYLLADB_PORT)
                          .WithCredentials(envJson.SCYLLADB_USERNAME, envJson.SCYLLADB_PASSWORD)
                          .WithDefaultKeyspace(envJson.SCYLLADB_KEYSPACE)
                          .WithPoolingOptions(new PoolingOptions()
                                              .SetWarmup(true)
                                              .SetHeartBeatInterval(10_000)
                                              .SetCoreConnectionsPerHost(HostDistance.Local, 4)
                                              .SetMaxConnectionsPerHost(HostDistance.Local, 20)
                                              .SetCoreConnectionsPerHost(HostDistance.Remote, 2)
                                              .SetMaxConnectionsPerHost(HostDistance.Remote, 4)
                          )
                          .Build()
        ;
    }

    public static async Task<ISession> CreateNewSessionAsync() {
        if (_cluster is null) {
            Init();
        }

        return await _cluster!.ConnectAsync();
    }
}
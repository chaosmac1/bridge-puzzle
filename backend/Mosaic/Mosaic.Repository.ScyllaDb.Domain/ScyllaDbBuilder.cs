using Cassandra;
using Mosaic.Repository.Environment.Adapter;
using Mosaic.Repository.Environment.Adapter.Interface;

namespace Mosaic.Repository.ScyllaDb.Domain;

public class ScyllaDbBuilder {
    private static Cluster? _cluster = null;

    private readonly IEnvJson _envJson;

    public ScyllaDbBuilder(IEnvJson envJson) {
        _envJson = envJson;
    }
    
    private void Init() {
        ScyllaDbMapper.DefineGlobalScyllaDbMapper();
        
        var envJson = _envJson;
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

    public async Task<ISession> CreateNewSessionAsync() {
        if (_cluster is null) {
            Init();
        }

        return await _cluster!.ConnectAsync();
    }
}
// ReSharper disable InconsistentNaming
namespace Mosaic.Repository.Environment.Adapter.Interface;

public interface IEnvJson {
    public string POSTGRESQL_URL { get; }
    public int POSTGRESQL_PORT { get; }
    public string POSTGRESQL_PASSWORD { get; }
    public string POSTGRESQL_USERNAME { get; }
    public string POSTGRESQL_DOMAIN { get; }
    
    public string SCYLLADB_URL { get; }
    public int SCYLLADB_PORT { get; }
    public string SCYLLADB_PASSWORD { get; }
    public string SCYLLADB_USERNAME { get; }
    public string SCYLLADB_KEYSPACE { get; }
}
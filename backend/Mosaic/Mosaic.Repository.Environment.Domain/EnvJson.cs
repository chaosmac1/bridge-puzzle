using Microsoft.Extensions.Configuration;
using Mosaic.Repository.Environment.Adapter.Interface;
// ReSharper disable UnassignedGetOnlyAutoProperty

namespace Mosaic.Repository.Environment.Domain;

public class EnvJson: IEnvJson {
    public string POSTGRESQL_URL { get; init; }
    public string POSTGRESQL_PASSWORD { get; init; }
    public string POSTGRESQL_USERNAME { get; init; }
    public string POSTGRESQL_DOMAIN { get; init; }
    public int POSTGRESQL_PORT { get; init; }

    public string SCYLLADB_URL { get; set; }
    public int SCYLLADB_PORT { get; set; }
    public string SCYLLADB_PASSWORD { get; set; }
    public string SCYLLADB_USERNAME { get; set; }
    public string SCYLLADB_KEYSPACE { get; set; }
    
    private EnvJson(IConfigurationRoot config) {
        POSTGRESQL_URL = GetEnvironmentVariableCheckNull(config, "POSTGRESQL_URL");
        POSTGRESQL_PASSWORD = GetEnvironmentVariableCheckNull(config, "POSTGRESQL_PASSWORD");
        POSTGRESQL_USERNAME = GetEnvironmentVariableCheckNull(config, "POSTGRESQL_USERNAME");
        POSTGRESQL_DOMAIN = GetEnvironmentVariableCheckNull(config, "POSTGRESQL_DOMAIN");    
        POSTGRESQL_PORT = Int32.Parse(GetEnvironmentVariableCheckNull(config, "POSTGRESQL_PORT"));
        
        SCYLLADB_URL = GetEnvironmentVariableCheckNull(config, "SCYLLADB_URL");
        SCYLLADB_KEYSPACE = GetEnvironmentVariableCheckNull(config, "SCYLLADB_KEYSPACE");
        SCYLLADB_PASSWORD = GetEnvironmentVariableCheckNull(config, "SCYLLADB_PASSWORD");
        SCYLLADB_USERNAME = GetEnvironmentVariableCheckNull(config, "SCYLLADB_USERNAME");    
        SCYLLADB_PORT = Int32.Parse(GetEnvironmentVariableCheckNull(config, "SCYLLADB_PORT")); 
    }

    public static EnvJson Create() {
        IConfigurationRoot config = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json", true)
                                    .AddEnvironmentVariables()
                                    .Build();
        return new EnvJson(config);
    }
    
    private static string GetEnvironmentVariableCheckNull(IConfigurationRoot config, string name) {
        return config[name] ?? throw new NullReferenceException("EnvironmentVariable " + name);
    }
}
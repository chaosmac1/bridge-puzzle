using Mosaic.Repository.Environment.Adapter;
using Npgsql;

namespace Mosaic.Repository.Postgresql.Domain;

internal static class NpgsqlBuilder {
    private static string? _npgsqlConnectionString;

    private static void Init() {
        IServiceProvider serviceProvider = Collection.Adapter.Injection.GlobalServiceProvider;
        var envJson = serviceProvider.GetEnvJson();
        
        
        var connStringBuilder = new NpgsqlConnectionStringBuilder();
        connStringBuilder.Host = envJson.POSTGRESQL_URL;
        connStringBuilder.Port = envJson.POSTGRESQL_PORT;
        connStringBuilder.Password = envJson.POSTGRESQL_PASSWORD;
        connStringBuilder.Username = envJson.POSTGRESQL_USERNAME;
        connStringBuilder.Database = envJson.POSTGRESQL_DOMAIN;
        connStringBuilder.Pooling = true;
        connStringBuilder.ReadBufferSize = 1048576;
        connStringBuilder.WriteBufferSize = 1048576;
        connStringBuilder.MaxPoolSize = 1024;
        connStringBuilder.MinPoolSize = 256;
        connStringBuilder.KeepAlive = 10;
        connStringBuilder.TcpKeepAlive = true;
        
        _npgsqlConnectionString = connStringBuilder.ToString();
    }

    public static async Task<NpgsqlConnection> BuildNpgsqlConnection() {
        if (_npgsqlConnectionString is null) {
            Init();
        }
        
        var con = new NpgsqlConnection(
            _npgsqlConnectionString
            ?? throw new NullReferenceException(nameof(_npgsqlConnectionString))
        );
        await con.OpenAsync();
        return con;
    }
}
using Dapper;
using Mosaic.Repository.Environment.Adapter;
using Mosaic.Repository.Environment.Adapter.Interface;
using Npgsql;
using Pgvector.Dapper;

namespace Mosaic.Repository.Postgresql.Domain;

public class NpgsqlBuilder {
    private static string? _npgsqlConnectionString;
    private static NpgsqlDataSourceBuilder? _npgsqlDataSourceBuilder;

    private IEnvJson _envJson;

    public NpgsqlBuilder(IEnvJson envJson) {
        _envJson = envJson;
    }

    private void Init() {
        var envJson = _envJson;
        
        
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
        _npgsqlDataSourceBuilder = new NpgsqlDataSourceBuilder(_npgsqlConnectionString);
        _npgsqlDataSourceBuilder.UseVector();
        
        SqlMapper.AddTypeHandler(new VectorTypeHandler());
    }

    public async Task<NpgsqlConnection> BuildNpgsqlConnection() {
        if (_npgsqlConnectionString is null || _npgsqlDataSourceBuilder is null) {
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
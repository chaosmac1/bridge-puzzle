using Mosaic.Repository.Environment.Adapter.Interface;

namespace Mosaic.Repository.Environment.Domain;

public class EnvProvider: IEnvProvider {
    private IEnvJson? _envJson;
    private IEnvDb? _envDb;
    public IEnvJson EnvJson => _envJson ??= Mosaic.Repository.Environment.Domain.EnvJson.Create();
    public IEnvDb EnvDb => _envDb ??= Mosaic.Repository.Environment.Domain.EnvDb.Create();
    
    public EnvProvider() {
        
    }
}
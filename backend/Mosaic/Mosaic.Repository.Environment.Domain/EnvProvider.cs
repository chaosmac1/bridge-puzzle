using Mosaic.Repository.Environment.Adapter.Interface;

namespace Mosaic.Repository.Environment.Domain;

public class EnvProvider: IEnvProvider {
    private IEnvJson? _envJson;
    public IEnvJson EnvJson => _envJson ??= Mosaic.Repository.Environment.Domain.EnvJson.Create();
    
    public EnvProvider() {
        
    }
}
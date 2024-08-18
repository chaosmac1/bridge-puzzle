namespace Mosaic.Repository.Environment.Adapter.Interface;

public interface IEnvProvider {
    public IEnvJson EnvJson { get; }
    public IEnvDb EnvDb { get; }
}
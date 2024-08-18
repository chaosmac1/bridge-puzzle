using Mosaic.Repository.Environment.Adapter.Interface;

namespace Mosaic.Repository.Environment.Domain;

public class EnvDb: IEnvDb {
    private EnvDb() {
    }

    public static IEnvDb Create() {
        return new EnvDb();
    }
}
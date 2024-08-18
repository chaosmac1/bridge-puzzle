using Mosaic.Repository.Collection.Adapter.LoadProvider;
using Mosaic.Repository.Collection.Domain;

namespace Mosaic.Repository.Collection.Loader;

public static class CreateLoader {
    public static ILoadProvider Create() => new LoadProvider();
}
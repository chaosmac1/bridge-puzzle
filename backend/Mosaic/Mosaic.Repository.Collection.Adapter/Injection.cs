using Microsoft.Extensions.DependencyInjection;

namespace Mosaic.Repository.Collection.Adapter;

public static class Injection {
    private static IServiceProvider? _provider;

    public static IServiceProvider GlobalServiceProvider =>
        _provider ?? throw new NullReferenceException(nameof(_provider));

    public static IServiceProvider SetProvider(IServiceProvider provider) {
        return _provider = provider;
    }
}
using Mosaic.Repository.Collection.Adapter.Export;

namespace Mosaic.Repository.Collection.Adapter.LoadProvider;

public interface ILoadProvider: ILoadProviderAdd<ILoadProvider> {
    public void Build();
    
    ILoadProviderAdd ILoadProviderAdd.Bind(IServiceBinder services) {
        return this.Bind(services);
    }

    public ILoadProviderAdd<ILoadProviderAdd> AsILoadProviderAdd();
}
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Mosaic.Init.Interface;

namespace Mosaic.Init;

public class InitializeSlices: IInitializeSlices {
    private readonly EndpointDiscoveryOptions _options;
    private InitializeSlices(EndpointDiscoveryOptions options) {
        _options = options;
    }


    public static IInitializeSlices Create(EndpointDiscoveryOptions options) => new InitializeSlices(options);
    
    public void Bind() {
        _options.Assemblies = [
            Mosaic.Slice.BridgePuzzle.Binder.BindEndpoint.GetEndpointDomainAssembly
        ];
    }
}
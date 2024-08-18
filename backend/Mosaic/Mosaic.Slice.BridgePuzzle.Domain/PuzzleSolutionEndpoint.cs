using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Mosaic.Repository.Environment.Adapter;
using Mosaic.Repository.Environment.Adapter.Interface;

namespace Mosaic.Slice.BridgePuzzle.Domain;

public class PuzzleSolutionEndpoint : Endpoint<PuzzleSolutionRequest, PuzzleSolutionResponse> {
    private readonly AsyncServiceScope _asyncServiceScope;
    private readonly IServiceProvider _serviceProvider;
    private readonly IEnvJson _envJson;

    public PuzzleSolutionEndpoint() {
        _asyncServiceScope = Repository.Collection.Adapter.Injection.GlobalServiceProvider.CreateAsyncScope();
        _serviceProvider = _asyncServiceScope.ServiceProvider;
        _envJson = _serviceProvider.GetEnvJson();
    }

    public override async Task OnAfterHandleAsync(PuzzleSolutionRequest req, PuzzleSolutionResponse res, CancellationToken ct) {
        await _asyncServiceScope.DisposeAsync();        
        await base.OnAfterHandleAsync(req, res, ct);
    }

    public override void Configure() {
        Post("/Api/V1/puzzle/solution");
        AllowAnonymous();
    }

    public override async Task HandleAsync(PuzzleSolutionRequest req, CancellationToken ct) {
        await SendOkAsync(new PuzzleSolutionResponse() { Test = "Test"}, ct);
    }
}
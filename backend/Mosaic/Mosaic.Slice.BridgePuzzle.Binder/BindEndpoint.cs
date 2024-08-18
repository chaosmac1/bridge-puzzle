using System.Reflection;

namespace Mosaic.Slice.BridgePuzzle.Binder;

public static class BindEndpoint {
    public static Assembly GetEndpointDomainAssembly {
        get {
            return typeof(Mosaic.Slice.BridgePuzzle.Domain.PuzzleSolutionEndpoint).Assembly;
        }
    }
}
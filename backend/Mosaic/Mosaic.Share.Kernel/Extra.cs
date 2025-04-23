using Mosaic.Share.Kernel.ValueObject;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Pgvector;
namespace Mosaic.Share.Kernel;

public static class Extra {
    public static float ComputeDistance(this Pgvector.Vector self, Pgvector.Vector vector) {
        var v1 = self.Memory.Span;
        var v2 = vector.Memory.Span;
        
        if (v1.Length != v2.Length)
            throw new ArgumentException("Vectors must have the same length");

        float sum = 0;
        for (int i = 0; i < v1.Length; i++) {
            float diff = v1[i] - v2[i];
            sum += diff * diff;
        }

        return MathF.Sqrt(sum);
    }

    public static (Vector, float Distance)[] ComputeDistances(this Vector self, IEnumerable<Vector> vectors) {
        return vectors.Select(x => (x, ComputeDistance(self,x))).ToArray();
    }

    public static Vector GetNears(this Vector self, IEnumerable<Vector> vectors) {
        return ComputeDistances(self, vectors)
               .OrderBy(x => x.Distance)
               .First().Item1
            ;
    }
}
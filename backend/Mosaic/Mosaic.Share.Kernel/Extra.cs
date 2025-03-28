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

    public static Vector ComputeMidpoint(this Vector self, Vector vector) {
        var v1 = self.Memory.Span;
        var v2 = vector.Memory.Span;
        
        if (v1.Length != v2.Length)
            throw new ArgumentException("Vectors must have the same length");

        float[] midpoint = new float[v1.Length];

        for (int i = 0; i < v1.Length; i++)
        {
            midpoint[i] = (v1[i] + v2[i]) / 2f;
        }

        return new Vector(new ReadOnlyMemory<float>(midpoint));
    }

    public static byte[] ToByteArray(this Vector self) {
        using var ms = new MemoryStream();
        using var writer = new BsonWriter(ms);
        var serializer = new JsonSerializer();
        serializer.Serialize(writer, self);

        return ms.ToArray();
    }

    public static Vector ToVector(byte[] bytes) {
        using var ms = new MemoryStream(bytes, false);
        using var reader = new BsonReader(ms);
        var serializer = new JsonSerializer();
        var floats = serializer.Deserialize<float[]>(reader) ?? throw new NullReferenceException("floats");

        return new Pgvector.Vector(floats);
    }
}
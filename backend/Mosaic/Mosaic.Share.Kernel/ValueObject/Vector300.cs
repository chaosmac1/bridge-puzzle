using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
namespace Mosaic.Share.Kernel.ValueObject;

[System.Runtime.CompilerServices.InlineArray(300)]
public struct Vector300: IEqualityComparer<Vector300>, IEquatable<Vector300> {
    private float _vectors;
    public float ComputeDistance(ref Vector300 vector300) {
        float sum = 0;
        for (int i = 0; i < 300; i++) {
            float diff = this[i] - vector300[i];
            sum += diff * diff;
        }

        return MathF.Sqrt(sum);
    }

    public (Vector300 Vector, float Distance)[] ComputeDistances(IReadOnlyList<Vector300> vectors) {
        var res = new (Vector300 WordWithVector, float Distance)[vectors.Count];
        for (var i = 0; i < vectors.Count; i++) {
            var vector = vectors[i];
            res[i] = (vector, this.ComputeDistance(ref vector));
        }

        return res;
    }

    public Vector300 GetNears(IReadOnlyList<Vector300> vectors) {
        return ComputeDistances(vectors)
               .OrderBy(x => x.Distance)
               .First().Vector
            ;
    }

    public Vector300 ComputeMidpoint(ref Vector300 vector300) {
        Vector300 midpoint = new Vector300();

        for (int i = 0; i < 300; i++) {
            midpoint[i] = (this[i] + vector300[i]) / 2f;
        }

        return midpoint;
    }
    
    public byte[] ToByteArray() {
        using var ms = new MemoryStream();
        using var writer = new BsonDataWriter(ms);
        var serializer = new JsonSerializer();
        
        var floats = new float[300];
        for (int i = 0; i < 300; i++) {
            floats[i] = this[i];
        }
        
        serializer.Serialize(writer, floats);

        return ms.ToArray();
    }

    public static Vector300 ToVector(byte[] bytes) {
        using var ms = new MemoryStream(bytes, false);
        using var reader = new BsonDataReader(ms);
        var serializer = new JsonSerializer();

        var vector300 = new Vector300();
        float[] floats = serializer.Deserialize<float[]>(reader) ?? throw new NullReferenceException("floats");

        for (var i = 0; i < 300; i++) {
            vector300[i] = floats[i];
        }
        
        return vector300;
    }
    public bool Equals(Vector300 x, Vector300 y) => x == y;
    public int GetHashCode(Vector300 obj) {
        int res = 0;

        for (int i = 0; i < 300; i++) {
            res = res ^ this[i].GetHashCode();
        }
        
        return res;
    }
    public bool Equals(Vector300 other) => this == other;

    public override bool Equals(object? obj) => obj is Vector300 other && Equals(other);
    public override int GetHashCode() => _vectors.GetHashCode();
    
    public static bool operator ==(Vector300 x, Vector300 y) {
        for (int i = 0; i < 300; i++) {
            if (x[i] == y[i]) {
                continue;
            }

            return false;
        }

        return true; 
    }
    
    public static bool operator !=(Vector300 x, Vector300 y) {
        return !(x == y);
    }
}
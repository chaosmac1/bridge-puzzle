using System.Diagnostics.CodeAnalysis;
using Mosaic.Share.Kernel.ValueObject;
using Npgsql.Internal;
using Npgsql;
using Npgsql.Internal.Postgres;
using Pgvector;
using Pgvector.Npgsql;

namespace Mosaic.Repository.Postgresql.Domain.Handler;

[Experimental("NPG9001")]
public sealed class Vector300TypeInfoResolverFactory: PgTypeInfoResolverFactory {
    public Vector300TypeInfoResolverFactory() {
    }

    public override IPgTypeInfoResolver CreateResolver() => new Resolver();
    public override IPgTypeInfoResolver? CreateArrayResolver() => new ArrayResolver();
    
    class Resolver: IPgTypeInfoResolver {
        TypeInfoMappingCollection? _mappings;
        protected TypeInfoMappingCollection Mappings => _mappings ??= AddMappings(new());

        public PgTypeInfo? GetTypeInfo(Type? type, DataTypeName? dataTypeName, PgSerializerOptions options)
            => Mappings.Find(type, dataTypeName, options);

        static TypeInfoMappingCollection AddMappings(TypeInfoMappingCollection mappings) {
            mappings.AddStructType<Vector300>("vector",
                static (options, mapping, _) => mapping.CreateInfo(options, new Vector300Converter()), isDefault: true);

            mappings.AddStructType<Vector300>("halfvec",
                static (options, mapping, _) => mapping.CreateInfo(options, new Vector300Converter()), isDefault: true);

            mappings.AddStructType<Vector300>("sparsevec",
                static (options, mapping, _) => mapping.CreateInfo(options, new Vector300Converter()), isDefault: true);
            mappings.AddStructType<Vector300>("bytea",
                static (options, mapping, _) => mapping.CreateInfo(options, new Vector300Converter()), isDefault: true);
            return mappings;
        }
    }
    
    class ArrayResolver : Resolver, IPgTypeInfoResolver {
        TypeInfoMappingCollection? _mappings;
        new TypeInfoMappingCollection Mappings => _mappings ??= AddMappings(new(base.Mappings));

        public new PgTypeInfo? GetTypeInfo(Type? type, DataTypeName? dataTypeName, PgSerializerOptions options)
            => Mappings.Find(type, dataTypeName, options);

        static TypeInfoMappingCollection AddMappings(TypeInfoMappingCollection mappings)
        {
            mappings.AddStructType<Vector300>("vector",
                static (options, mapping, _) => mapping.CreateInfo(options, new Vector300Converter()), isDefault: true);

            mappings.AddStructType<Vector300>("halfvec",
                static (options, mapping, _) => mapping.CreateInfo(options, new Vector300Converter()), isDefault: true);

            mappings.AddStructType<Vector300>("sparsevec",
                static (options, mapping, _) => mapping.CreateInfo(options, new Vector300Converter()), isDefault: true);
            
            mappings.AddStructType<Vector300>("bytea",
                static (options, mapping, _) => mapping.CreateInfo(options, new Vector300Converter()), isDefault: true);
            return mappings;
        }
    }

    public sealed class Vector300Converter: PgStreamingConverter<Vector300> {
        public override Vector300 Read(PgReader reader) {
            using var mem = new MemoryStream();
            reader.GetStream().CopyTo(mem);
            return Vector300.ToVector(mem.ToArray());
        }
        
        public override async ValueTask<Vector300> ReadAsync(PgReader reader, CancellationToken cancellationToken = new CancellationToken()) {
            using var mem = new MemoryStream();
            await reader.GetStream().CopyToAsync(mem, cancellationToken);
            return Vector300.ToVector(mem.ToArray());
        }
        
        public override Size GetSize(SizeContext context, Vector300 value, ref object? writeState) {
            return Size.Create(4 * 300);
        }
        
        public override void Write(PgWriter writer, Vector300 value) {
            var bytes = value.ToByteArray();
            writer.GetStream().Write(bytes, 0, bytes.Length);
        }
        
        public override async ValueTask WriteAsync(PgWriter writer, Vector300 value, CancellationToken cancellationToken = new CancellationToken()) {
            var bytes = value.ToByteArray();
            await writer.GetStream().WriteAsync(bytes, 0, bytes.Length, cancellationToken);
        }
    }
}
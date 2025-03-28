using Mosaic.Repository.Postgresql.Kernel.Entities;
using Mosaic.Share.Kernel.Word;

namespace Mosaic.Repository.Postgresql.Kernel;

public static class Mapper {
    public static WordWithVectorDto ToWordWithVectorDto(this WordVectorSpace wordVectorSpace) {
        return new WordWithVectorDto(wordVectorSpace.Word, wordVectorSpace.V300);
    }
    
    public static Word ToWord(this WordVectorSpace wordVectorSpace) {
        return new Word(wordVectorSpace.Word);
    }
}
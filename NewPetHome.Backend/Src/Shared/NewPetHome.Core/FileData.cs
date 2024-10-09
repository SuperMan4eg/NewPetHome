using NewPetHome.SharedKernel.ValueObjects;

namespace NewPetHome.Core;

public record FileData(Stream Stream, FileInfo Info);

public record FileInfo(FilePath FilePath, string BucketName);
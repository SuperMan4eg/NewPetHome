using NewPetHome.Domain.Shared.ValueObjects;

namespace NewPetHome.Applications.Files;

public record FileData(Stream Stream, FileInfo Info);

public record FileInfo(FilePath FilePath, string BucketName);

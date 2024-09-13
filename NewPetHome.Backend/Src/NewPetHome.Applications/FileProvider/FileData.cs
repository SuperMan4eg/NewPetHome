using NewPetHome.Domain.Shared.ValueObjects;

namespace NewPetHome.Applications.FileProvider;

public record FileData(Stream Stream, FilePath FilePath, string BucketName);

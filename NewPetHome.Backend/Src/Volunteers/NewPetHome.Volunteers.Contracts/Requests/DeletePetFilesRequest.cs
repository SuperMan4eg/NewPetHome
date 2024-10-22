namespace NewPetHome.Volunteers.Contracts.Requests;

public record DeletePetFilesRequest(IEnumerable<string> FilePaths);
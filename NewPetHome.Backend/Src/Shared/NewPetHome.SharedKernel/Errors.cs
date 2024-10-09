namespace NewPetHome.SharedKernel;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("value.is.invalid", $"{label} is invalid");
        }

        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $" for Id '{id}'";
            return Error.NotFound("record.not.found", $"record not found{forId}");
        }

        public static Error ValueIsRequired(string? name = null)
        {
            var label = name == null ? "" : name + " ";
            return Error.Validation("length.is.invalid", $"invalid {label}length");
        }

        public static Error AlreadyExists(string entity, string? fieldName = null, string? label = null)
        {
            fieldName ??= "field name";

            label ??= $"such";

            return Error.Failure("already.exists", $"{entity} with {label} {fieldName} already exists");
        }
    }
}
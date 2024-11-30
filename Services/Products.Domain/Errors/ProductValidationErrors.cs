using Abstractions.ResultsPattern;

namespace Products.Domain.Errors;

public class ProductValidationErrors
{
    public static Error MissingField(string fieldName) => new("Validation.MissingField", $"The field '{fieldName}' is required but was not provided.");
    public static Error InvalidField(string fieldName) => new("Validation.InvalidField", $"The field '{fieldName}' has an invalid value.");
    public static Error UnsupportedCategory(string category) => new("Validation.UnsupportedCategory", $"The category '{category}' is not supported.");
    public static Error NullDynamicFields=> new("Validation.NullDynamicFields", "Dynamic Fields Can't be null");
}
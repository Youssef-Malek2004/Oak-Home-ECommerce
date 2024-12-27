using Abstractions.ResultsPattern;

namespace Payments.Domain.Errors;

public static class PaymentErrors
{
    public static Error PaymentNotFoundId(Guid id) => new("Payments.PaymentNotFound", $"Payment with ID {id} was not found.");
    public static readonly Error PaymentAddFailed = new("Payments.PaymentAddFailed", "Failed to add payment.");
    public static readonly Error PaymentRemoveFailed = new("Payments.PaymentRemoveFailed", "Failed to remove payment.");
    public static readonly Error PaymentUpdateFailed = new("Payments.PaymentUpdateFailed", "Failed to update payment.");
    public static readonly Error PaymentQueryFailed = new("Payments.PaymentQueryFailed", "Failed to query payments.");
}
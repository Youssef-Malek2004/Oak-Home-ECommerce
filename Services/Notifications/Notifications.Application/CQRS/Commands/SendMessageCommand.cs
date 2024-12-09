using Abstractions.ResultsPattern;
using MediatR;

namespace Notifications.Application.CQRS.Commands;

public record SendMessageCommand(string Message) : IRequest<Result>;
using Ardalis.Result;
using custom_chat_backend.Core.DTOs.Users.Request;
using custom_chat_backend.Core.DTOs.Users.Response;
using MediatR;

namespace custom_chat_backend.Core.Application.UseCases.User.Queries;

public record GetUserQuery(GetUserInput Input):IRequest<Result<GetUserOutput>>;
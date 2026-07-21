using Ardalis.Result;
using custom_chat_backend.Core.DTOs.Auth.Request;
using custom_chat_backend.Core.DTOs.Auth.Response;
using MediatR;

namespace custom_chat_backend.Core.Application.UseCases.Auth.Queries;

public record CallbackQuery(GetLoginCallbackInput Input):IRequest<Result<OAuthLoginOutput>>;
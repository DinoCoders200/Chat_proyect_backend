using Ardalis.Result;
using custom_chat_backend.Core.Application.Strategies.Auth;
using custom_chat_backend.Core.DTOs.Auth.Response;
using MediatR;

namespace custom_chat_backend.Core.Application.UseCases.Auth.Queries;

public class CallbackQueryHandler(AuthProviderFactory authProviderFactory):IRequestHandler<CallbackQuery,Result<GetLoginCallbackOutput>>
{
    public async Task<Result<GetLoginCallbackOutput>> Handle(CallbackQuery request,
        CancellationToken cancellationToken)
    {
        var strategy = authProviderFactory.GetStrategy(request.Input.ProviderName);
        var value = strategy.ExtractUserData(request.Input.Claims);

        var output = new GetLoginCallbackOutput(value.Username, value.Email);
        return Result.Success(output);
    }
}
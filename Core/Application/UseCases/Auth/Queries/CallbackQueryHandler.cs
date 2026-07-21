using Ardalis.Result;
using Ardalis.Specification;
using custom_chat_backend.Core.Application.Strategies.Auth;
using custom_chat_backend.Core.Domain.Entities.OAuthAccount;
using custom_chat_backend.Core.Domain.Entities.OAuthAccount.Enums;
using custom_chat_backend.Core.Domain.Entities.OAuthAccount.Specifications;
using custom_chat_backend.Core.Domain.Entities.User;
using custom_chat_backend.Core.Domain.Entities.User.Specifications;
using custom_chat_backend.Core.DTOs.Auth.Response;
using custom_chat_backend.Core.Interfaces.Auth;
using MediatR;

namespace custom_chat_backend.Core.Application.UseCases.Auth.Queries;

public class CallbackQueryHandler(
    AuthProviderFactory authProviderFactory,
    IRepositoryBase<UserEntity> userRepository,
    IRepositoryBase<OAuthAccountEntity> oauthRepository,
    ITokenService tokenService,
    IExternalAuthService externalAuthService)
    : IRequestHandler<CallbackQuery, Result<OAuthLoginOutput>>
{
    public async Task<Result<OAuthLoginOutput>> Handle(
        CallbackQuery request,
        CancellationToken cancellationToken)
    {
        var strategy = authProviderFactory.GetStrategy(request.Input.ProviderName);

        var externalUser = strategy.ExtractUserData(request.Input.Claims);

        var provider = request.Input.ProviderName.ToLower() switch
        {
            "google" => OAuthProvider.Google,
            "discord" => OAuthProvider.Discord,
            _ => throw new ArgumentException("Unsupported provider.")
        };

        var oauthSpec = new OAuthAccountByProviderSpec(
            provider,
            externalUser.ProviderId);

        var oauthAccount = await oauthRepository.FirstOrDefaultAsync(
            oauthSpec,
            cancellationToken);

        UserEntity? user;

        if (oauthAccount is not null)
        {
            user = await userRepository.GetByIdAsync(
                oauthAccount.UserId,
                cancellationToken);

            if (user is null)
            {
                return Result.NotFound("User not found.");
            }
        }
        else
        {
            var userSpec = new UserByEmailSpec(externalUser.Email);

            user = await userRepository.FirstOrDefaultAsync(
                userSpec,
                cancellationToken);

            if (user is null)
            {
                user = new UserEntity(
                    externalUser.Username,
                    externalUser.Email);

                await userRepository.AddAsync(
                    user,
                    cancellationToken);
            }

            var newOAuthAccount = new OAuthAccountEntity(
                user.Id,
                provider,
                externalUser.ProviderId);

            await oauthRepository.AddAsync(
                newOAuthAccount,
                cancellationToken);
        }

        var accessToken = tokenService.GenerateAccessToken(user);

        var refreshToken = tokenService.GenerateRefreshToken();

        await externalAuthService.SignOutExternalAsync();

        return Result.Success(
            new OAuthLoginOutput(
                accessToken,
                refreshToken));
    }
}
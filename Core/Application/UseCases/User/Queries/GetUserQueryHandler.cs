using Ardalis.Result;
using Ardalis.Specification;
using custom_chat_backend.Core.Domain.Entities.User;
using custom_chat_backend.Core.Domain.Entities.User.Specifications;
using custom_chat_backend.Core.DTOs.Users.Response;
using MediatR;

namespace custom_chat_backend.Core.Application.UseCases.User.Queries;

public class GetUserQueryHandler(IRepositoryBase<UserEntity> userRepository):IRequestHandler<GetUserQuery,Result<GetUserOutput>>
{
    public async Task<Result<GetUserOutput>> Handle(GetUserQuery req, CancellationToken cancellationToken)
    {
        var spec = new UserByIdSpec(req.Input.IdUser);
        
        var res= await userRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (res == null) return Result.NotFound("User not found");

        return new GetUserOutput(res.Username, res.Password);
    }
}
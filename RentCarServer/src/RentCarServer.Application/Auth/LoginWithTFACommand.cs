using GenericRepository;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Auth
{
    public sealed record LoginWithTFACommand(
    string EmailOrUserName,
    string TFACode,
    string TFAConfirmCode) : IRequest<Result<LoginCommandResponse>>;

    internal sealed class LoginWithTFACommandHandler(
        IUserRepository userRepository,
        IJwtProvider jwtProvider,
        IUnitOfWork unitOfWork) : IRequestHandler<LoginWithTFACommand, Result<LoginCommandResponse>>
    {
        public async Task<Result<LoginCommandResponse>> Handle(LoginWithTFACommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FirstOrDefaultAsync(p =>
                p.Email == request.EmailOrUserName
                || p.UserName == request.EmailOrUserName);

            if (user is null)
            {
                return Result<LoginCommandResponse>.Failure("Kullanıcı adı ya da şifre yanlış");
            }

            if (
                user.TFAIsCompleted is null
                || user.TFAExpiresDate is null
                || user.TFACode is null
                || user.TFAConfirmCode is null
                || user.TFAIsCompleted.Value
                || user.TFAExpiresDate.Value < DateTimeOffset.Now
                || (user.TFAConfirmCode != request.TFAConfirmCode
                || user.TFACode != request.TFACode))
            {
                return Result<LoginCommandResponse>.Failure("TFA kodu geçersiz");
            }

            user.SetTFACompleted();
            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var token = await jwtProvider.CreateTokenAsync(user, cancellationToken);
            var res = new LoginCommandResponse() { Token = token };

            return res;
        }
    }
}

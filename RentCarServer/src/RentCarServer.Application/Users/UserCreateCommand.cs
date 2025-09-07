using FluentValidation;
using GenericRepository;
using RentCarServer.Application.Behaviours;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Users
{
    [Permission("user:create")]
    public sealed record UserCreateCommand(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    Guid? BranchId,
    Guid RoleId,
    bool IsActive) : IRequest<Result<string>>;

    public sealed class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
    {
        public UserCreateCommandValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty().WithMessage("Geçerli bir ad girin");
            RuleFor(p => p.LastName).NotEmpty().WithMessage("Geçerli bir soyad girin");
            RuleFor(p => p.UserName).NotEmpty().WithMessage("Geçerli bir kullanıcı adı girin");
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Geçerli bir mail adresi girin")
                .EmailAddress().WithMessage("Geçerli bir mail adresi girin");
        }
    }

    internal sealed class UserCreateCommandHandler(
        IUserRepository userRepository,
        IClaimContext claimContext,
        IUnitOfWork unitOfWork) : IRequestHandler<UserCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await userRepository.AnyAsync(p => p.Email == request.Email, cancellationToken);
            if (emailExists)
            {
                return Result<string>.Failure("Bu mail adresi daha önce kullanılmış");
            }

            var userNameExists = await userRepository.AnyAsync(p => p.UserName == request.UserName, cancellationToken);
            if (userNameExists)
            {
                return Result<string>.Failure("Bu kullanıcı adı daha önce kullanılmış");
            }

            var branchId = claimContext.GetBranchId();
            if (request.BranchId is not null)
            {
                branchId = request.BranchId.Value;
            }

            Password password = new("123");

            User user = new(request.FirstName, request.LastName, request.UserName, request.Email, password, branchId, request.RoleId,request.IsActive);
            userRepository.Add(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Kullanıcı başarıyla oluşturuldu";
        }
    }
}

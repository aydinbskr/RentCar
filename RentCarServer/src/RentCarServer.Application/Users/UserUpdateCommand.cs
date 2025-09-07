﻿using FluentValidation;
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
    [Permission("user:update")]
    public sealed record UserUpdateCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    Guid? BranchId,
    Guid RoleId,
    bool IsActive) : IRequest<Result<string>>;

    public sealed class UserUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
    {
        public UserUpdateCommandValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty().WithMessage("Geçerli bir ad girin");
            RuleFor(p => p.LastName).NotEmpty().WithMessage("Geçerli bir soyad girin");
            RuleFor(p => p.UserName).NotEmpty().WithMessage("Geçerli bir kullanıcı adı girin");
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Geçerli bir mail adresi girin")
                .EmailAddress().WithMessage("Geçerli bir mail adresi girin");
        }
    }

    internal sealed class UserUpdateCommandHandler(
        IUserRepository userRepository,
        IClaimContext claimContext,
        IUnitOfWork unitOfWork) : IRequestHandler<UserUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
            if (user is null)
            {
                return Result<string>.Failure("Kullanıcı bulunamadı");
            }

            if (user.Email != request.Email)
            {
                var emailExists = await userRepository.AnyAsync(p => p.Email == request.Email, cancellationToken);
                if (emailExists)
                {
                    return Result<string>.Failure("Bu mail adresi daha önce kullanılmış");
                }
            }

            if (user.UserName != request.UserName)
            {
                var userNameExists = await userRepository.AnyAsync(p => p.UserName == request.UserName, cancellationToken);
                if (userNameExists)
                {
                    return Result<string>.Failure("Bu kullanıcı adı daha önce kullanılmış");
                }
            }

            var branchId = claimContext.GetBranchId();
            if (request.BranchId is not null)
            {
                branchId = request.BranchId.Value;
            }

            user.SetFirstName(request.FirstName);
            user.SetLastName(request.LastName);
            user.SetEmail(request.Email);
            user.SetFullName();
            user.SetUserName(request.UserName);
            user.SetBranchId(branchId);
            user.SetRoleId(request.RoleId);
            user.SetStatus(request.IsActive);
            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Kullanıcı başarıyla güncellendi";
        }
    }
}

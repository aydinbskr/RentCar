﻿using FluentValidation;
using GenericRepository;
using RentCarServer.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Customers
{
    public sealed record CustomerUpdateCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string IdentityNumber,
    DateOnly DateOfBirth,
    string PhoneNumber,
    string Email,
    DateOnly DrivingLicenseIssuanceDate,
    string FullAddress,
    bool IsActive
) : IRequest<Result<string>>;

    public sealed class CustomerUpdateCommandValidator : AbstractValidator<CustomerUpdateCommand>
    {
        public CustomerUpdateCommandValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty().WithMessage("Ad alaný boþ olamaz.");
            RuleFor(p => p.LastName).NotEmpty().WithMessage("Soyad alaný boþ olamaz.");
            RuleFor(p => p.IdentityNumber).NotEmpty().WithMessage("TC kimlik numarasý boþ olamaz.");
            RuleFor(p => p.Email).NotEmpty().WithMessage("E-posta adresi boþ olamaz.")
                                 .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");
            RuleFor(p => p.PhoneNumber).NotEmpty().WithMessage("Telefon numarasý boþ olamaz.");
            RuleFor(p => p.FullAddress).NotEmpty().WithMessage("Adres alaný boþ olamaz.");
        }
    }

    internal sealed class CustomerUpdateCommandHandler(
        ICustomerRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<CustomerUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
        {
            Customer? customer = await repository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (customer is null)
                return Result<string>.Failure("Müþteri bulunamadý");

            if (!string.Equals(customer.IdentityNumber, request.IdentityNumber, StringComparison.OrdinalIgnoreCase))
            {
                bool exists = await repository.AnyAsync(
                    x => x.IdentityNumber == request.IdentityNumber && x.Id != request.Id, cancellationToken);
                if (exists)
                    return Result<string>.Failure("Bu TC kimlik numarasý ile kayýtlý baþka bir müþteri var.");
            }


            customer.SetFirstName(request.FirstName);
            customer.SetLastName(request.LastName);
            customer.SetFullName();
            customer.SetIdentityNumber(request.IdentityNumber);
            customer.SetDateOfBirth(request.DateOfBirth);
            customer.SetPhoneNumber(request.PhoneNumber);
            customer.SetEmail(request.Email);
            customer.SetDrivingLicenseIssuanceDate(request.DrivingLicenseIssuanceDate);
            customer.SetFullAddress(request.FullAddress);
            customer.SetStatus(request.IsActive);

            repository.Update(customer);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Müþteri baþarýyla güncellendi";
        }
    }
}

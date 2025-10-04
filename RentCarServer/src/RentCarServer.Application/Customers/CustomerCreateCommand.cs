using FluentValidation;
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
    public sealed record CustomerCreateCommand(
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

    public sealed class CustomerCreateCommandValidator : AbstractValidator<CustomerCreateCommand>
    {
        public CustomerCreateCommandValidator()
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

    internal sealed class CustomerCreateCommandHandler(
        ICustomerRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<CustomerCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CustomerCreateCommand request, CancellationToken cancellationToken)
        {
            bool exists = await repository.AnyAsync(x => x.IdentityNumber == request.IdentityNumber, cancellationToken);
            if (exists)
                return Result<string>.Failure("Bu TC kimlik numarasý ile kayýtlý müþteri var.");

            Customer customer = new(
                request.FirstName,
                request.LastName,
                request.IdentityNumber,
                request.DateOfBirth,
                request.PhoneNumber,
                request.Email,
                request.DrivingLicenseIssuanceDate,
                request.FullAddress,
                request.IsActive
            );

            repository.Add(customer);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Müþteri baþarýyla kaydedildi";
        }
    }
}

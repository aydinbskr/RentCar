using FluentValidation;
using GenericRepository;
using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.Categories;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Categories
{
    [Permission("category:create")]
    public sealed record CategoryCreateCommand(
        string Name,
        bool IsActive) : IRequest<Result<string>>;

    public sealed class CategoryCreateCommandValidator : AbstractValidator<CategoryCreateCommand>
    {
        public CategoryCreateCommandValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Geçerli bir kategori adý girin");
        }
    }

    internal sealed class CategoryCreateCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<CategoryCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
        {
            var nameExists = await categoryRepository.AnyAsync(p => p.Name == request.Name, cancellationToken);

            if (nameExists)
            {
                return Result<string>.Failure("Kategori adý daha önce tanýmlanmýþ");
            }

            Category category = new(request.Name, request.IsActive);
            categoryRepository.Add(category);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Kategori baþarýyla kaydedildi";
        }
    }
}
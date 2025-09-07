using FluentValidation;
using GenericRepository;
using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.Categories;
using System.Xml.Linq;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Categories
{
    [Permission("category:edit")]
    public sealed record CategoryUpdateCommand(
        Guid Id,
        string Name,
        bool IsActive) : IRequest<Result<string>>;

    public sealed class CategoryUpdateCommandValidator : AbstractValidator<CategoryUpdateCommand>
    {
        public CategoryUpdateCommandValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Ge�erli bir kategori ad� girin");
        }
    }

    internal sealed class CategoryUpdateCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<CategoryUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (category is null)
            {
                return Result<string>.Failure("Kategori bulunamad�");
            }

            if (!string.Equals(category.Name, request.Name, StringComparison.OrdinalIgnoreCase))
            {
                var nameExists = await categoryRepository.AnyAsync(
                    p => p.Name == request.Name && p.Id != request.Id,
                    cancellationToken);

                if (nameExists)
                {
                    return Result<string>.Failure("Kategori ad� daha �nce tan�mlanm��");
                }
            }

            category.SetName(request.Name);
            category.SetStatus(request.IsActive);
            categoryRepository.Update(category);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Kategori ba�ar�yla g�ncellendi";
        }
    }
}
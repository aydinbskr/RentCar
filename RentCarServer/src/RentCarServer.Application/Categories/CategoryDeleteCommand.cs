using GenericRepository;
using RentCarServer.Application.Behaviours;
using RentCarServer.Domain.Categories;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Categories
{
    [Permission("category:delete")]
    public sealed record CategoryDeleteCommand(
        Guid Id) : IRequest<Result<string>>;

    internal sealed class CategoryDeleteCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<CategoryDeleteCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (category is null)
            {
                return Result<string>.Failure("Kategori bulunamad�");
            }

            category.Delete();
            categoryRepository.Update(category);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Kategori ba�ar�yla silindi";
        }
    }
}
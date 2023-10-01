namespace Infrastructure.Repositories
{
    using Domain.Entities;

    public interface IImageRepository
    {
        Task<IEnumerable<Image>> GetAll();
        Task<Image> Add(Image image);
        Task<Image?> Update(Image image);
        Task<Image?> Remove(Guid id);
        Task<Image?> GetById(Guid id);
        Task SaveChanges();
    }
}

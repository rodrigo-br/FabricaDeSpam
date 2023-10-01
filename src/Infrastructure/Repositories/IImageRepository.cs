namespace Infrastructure.Repositories
{
    using Domain.DTO;
    using Domain.Entities;

    public interface IImageRepository
    {
        Task<IEnumerable<ImageDTO>> GetAll();
        Task<Image> Add(ImageDTO image);
        Task<Image?> Update(ImageDTO image);
        Task<Image?> Remove(Guid id);
        Task<ImageDTO?> GetById(Guid id);
        Task SaveChanges();
    }
}

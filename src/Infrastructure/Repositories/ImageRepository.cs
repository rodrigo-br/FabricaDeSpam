namespace Infrastructure.Repositories
{
    using Domain.Entities;
    using Infrastructure.ApplicationContext;
    using Microsoft.EntityFrameworkCore;

    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ImageRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Image> Add(Image image)
        {
            await _applicationDbContext.AddAsync(image);
            await _applicationDbContext.SaveChangesAsync();
            return image;
        }

        public async Task<IEnumerable<Image>> GetAll()
        {
            return await _applicationDbContext.Images.ToListAsync();
        }

        public async Task<Image?> GetById(Guid id)
        {
            return await _applicationDbContext.Images
                .Include(x => x.User)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Image?> Remove(Guid id)
        {
            var image = await _applicationDbContext.Images.FindAsync(id);

            if (image != null)
            {
                _applicationDbContext.Images.Remove(image);
                await _applicationDbContext.SaveChangesAsync();

                return image;
            }

            return null;
        }

        public async Task SaveChanges()
        {
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<Image?> Update(Image image)
        {
            var existingImage = await _applicationDbContext.Images
                .Include(x => x.User)
                .FirstOrDefaultAsync(i => i.Id == image.Id);

            if (existingImage != null)
            {
                _applicationDbContext.Entry(image).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();

                return existingImage;
            }

            return null;
        }
    }
}

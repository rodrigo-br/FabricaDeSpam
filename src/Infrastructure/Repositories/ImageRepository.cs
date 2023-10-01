namespace Infrastructure.Repositories
{
    using AutoMapper;
    using Domain.DTO;
    using Domain.Entities;
    using Infrastructure.ApplicationContext;
    using Microsoft.EntityFrameworkCore;

    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public ImageRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<Image> Add(ImageDTO image)
        {
            Image imageDomainModel = _mapper.Map<Image>(image);
            imageDomainModel.Id = Guid.NewGuid();
            imageDomainModel.User = _applicationDbContext.Users.FirstOrDefault(user => user.Id == imageDomainModel.UserId);
            await _applicationDbContext.AddAsync(imageDomainModel);
            await _applicationDbContext.SaveChangesAsync();
            return imageDomainModel;
        }

        public async Task<IEnumerable<ImageDTO>> GetAll()
        {
            var images = await _applicationDbContext.Images.ToListAsync();
            return _mapper.Map<List<ImageDTO>>(images);
        }

        public async Task<ImageDTO?> GetById(Guid id)
        {
            var image = await _applicationDbContext.Images
                .Include(i => i.User)
                .SingleOrDefaultAsync(i => i.Id == id);

            return _mapper.Map<ImageDTO>(image);
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

        public async Task<Image?> Update(ImageDTO image)
        {
            var existingImage = await _applicationDbContext.Images.FirstOrDefaultAsync(i => i.UserId == image.UserId && i.FileName == image.FileName);

            if (existingImage != null)
            {
                _applicationDbContext.Entry(existingImage).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();

                return existingImage;
            }

            return null;
        }
    }
}

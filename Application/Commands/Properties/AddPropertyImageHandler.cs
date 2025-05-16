using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Properties
{
    public class AddPropertyImageHandler : IRequestHandler<AddPropertyImageCommand, string>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyImageRepository _propertyImageRepository;
        private readonly IFileStorageService _fileStorage;

        public AddPropertyImageHandler(
            IPropertyRepository propertyRepository,
            IPropertyImageRepository propertyImageRepository,
            IFileStorageService fileStorage)
        {
            _propertyRepository = propertyRepository;
            _propertyImageRepository = propertyImageRepository;
            _fileStorage = fileStorage;
        }

        public async Task<string> Handle(AddPropertyImageCommand request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(request.PropertyId);
            if (property is null)
                throw new KeyNotFoundException($"No se encontró una propiedad con ID: {request.PropertyId}");

            byte[] imageBytes = Convert.FromBase64String(request.Base64Image);
            using var stream = new MemoryStream(imageBytes);

            var imageUrl = await _fileStorage.UploadFileAsync(request.Base64Image);

            var image = new PropertyImage
            {
                IdProperty = request.PropertyId,
                File = imageUrl,
                Enabled = true
            };

            await _propertyImageRepository.InsertAsync(image);
            return image.IdPropertyImage;
        }
    }
}

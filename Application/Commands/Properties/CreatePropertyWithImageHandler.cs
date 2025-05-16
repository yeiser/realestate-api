using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Properties
{
    public class CreatePropertyWithImageHandler : IRequestHandler<CreatePropertyWithImageCommand, PropertyDto>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IPropertyImageRepository _imageRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;

        public CreatePropertyWithImageHandler(
            IPropertyRepository propertyRepository,
            IOwnerRepository ownerRepository,
            IPropertyImageRepository imageRepository,
            IFileStorageService fileStorageService,
            IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _ownerRepository = ownerRepository;
            _imageRepository = imageRepository;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        public async Task<PropertyDto> Handle(CreatePropertyWithImageCommand request, CancellationToken cancellationToken)
        {
            var property = _mapper.Map<Property>(request);
            var owner = await _ownerRepository.GetByIdAsync(request.IdOwner);
            if (owner == null)
                throw new KeyNotFoundException($"No se encontró un propietario con ID: {request.IdOwner}");

            await _propertyRepository.InsertAsync(property);

            if (!string.IsNullOrEmpty(request.ImageBase64))
            {
                var imageUrl = await _fileStorageService.UploadFileAsync(request.ImageBase64);

                var propertyImage = new PropertyImage
                {
                    IdProperty = property.IdProperty,
                    File = imageUrl,
                    Enabled = true
                };

                await _imageRepository.InsertAsync(propertyImage);
            }

            return _mapper.Map<PropertyDto>(property);
        }
    }
}

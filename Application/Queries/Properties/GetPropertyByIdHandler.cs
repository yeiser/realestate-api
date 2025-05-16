using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Queries.Properties
{
    public class GetPropertyByIdHandler : IRequestHandler<GetPropertyByIdQuery, PropertyDetailDto>
    {
        private readonly IPropertyRepository _repository;
        private readonly IMapper _mapper;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IPropertyImageRepository _imageRepository;

        public GetPropertyByIdHandler(IPropertyRepository repository, 
            IMapper mapper,
            IOwnerRepository ownerRepository,
            IPropertyImageRepository imageRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _ownerRepository = ownerRepository;
            _imageRepository = imageRepository;
        }

        public async Task<PropertyDetailDto> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var property = await _repository.GetByIdAsync(request.Id);

            if (property == null)
                throw new KeyNotFoundException($"No se encontró la propiedad con ID: {request.Id}");

            var owner = await _ownerRepository.GetByIdAsync(property.IdOwner);
            var images = await _imageRepository.GetImagesByPropertyId(property.IdProperty);

            var dto = new PropertyDetailDto
            {
                IdProperty = property.IdProperty,
                Name = property.Name,
                Address = property.Address,
                Price = property.Price,
                CodeInternal = property.CodeInternal,
                IdOwner = property.IdOwner,
                Owner = owner,
                Images = images?.ToArray()
            };

            return dto;
        }
    }
}

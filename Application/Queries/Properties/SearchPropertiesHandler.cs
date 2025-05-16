using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Queries.Properties
{
    public class SearchPropertiesHandler : IRequestHandler<SearchPropertiesQuery, List<PropertyDto>>
    {
        private readonly IPropertyRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPropertyImageRepository _imageRepository;

        public SearchPropertiesHandler(IPropertyRepository repository, IMapper mapper, IPropertyImageRepository imageRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _imageRepository = imageRepository;
        }

        public async Task<List<PropertyDto>> Handle(SearchPropertiesQuery request, CancellationToken cancellationToken)
        {
            var properties = await _repository.SearchAsync(request.Name, request.Address, request.PriceFrom, request.PriceTo);
            var propertyDtos = _mapper.Map<List<PropertyDto>>(properties);

            foreach (var dto in propertyDtos)
            {
                var images = await _imageRepository.GetImagesByPropertyId(dto.IdProperty);
                dto.Image = images.FirstOrDefault()?.File;
            }

            return propertyDtos;
        }
    }
}

using Application.DTOs;
using Application.Interfaces;
using Application.Queries.Properties;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Queries.PropertyImages
{
    public class GetImagesByPropertyIdHandler : IRequestHandler<GetImagesByPropertyIdQuery, List<PropertyImage>>
    {
        private readonly IPropertyImageRepository _repository;
        private readonly IMapper _mapper;

        public GetImagesByPropertyIdHandler(IPropertyImageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PropertyImage>> Handle(GetImagesByPropertyIdQuery request, CancellationToken cancellationToken)
        {
            var imagenes = await _repository.GetImagesByPropertyId(request.Id);
            return _mapper.Map<List<PropertyImage>>(imagenes);
        }
    }
}

using Application.Commands.Owners;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Queries.Owners
{
    public class GetOwnerByNameHandler : IRequestHandler<GetOwnerByNameQuery, List<OwnerDto>>
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public GetOwnerByNameHandler(IOwnerRepository ownerRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        public async Task<List<OwnerDto>> Handle(GetOwnerByNameQuery request, CancellationToken cancellationToken)
        {
            var lstOwner = await _ownerRepository.FindByPropertyAsync(x => x.Name, request.Name);

            if (lstOwner == null || !lstOwner.Any())
                throw new KeyNotFoundException($"No se encontró ningún propietario con el nombre: {request.Name}");

            return _mapper.Map<List<OwnerDto>>(lstOwner);
        }
    }
}

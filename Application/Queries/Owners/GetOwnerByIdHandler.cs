using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Queries.Owners
{
    public class GetOwnerByIdHandler : IRequestHandler<GetOwnerByIdQuery, Owner>
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public GetOwnerByIdHandler(IOwnerRepository ownerRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        public async Task<Owner> Handle(GetOwnerByIdQuery request, CancellationToken cancellationToken)
        {
            var owner = await _ownerRepository.GetByIdAsync(request.Id);

            if (owner == null)
                throw new KeyNotFoundException($"No se encontró un propietario con ID: {request.Id}");

            return owner;
        }
    }
}

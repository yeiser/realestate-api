using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Commands.Owners
{
    public class UpdateOwnerHandler : IRequestHandler<UpdateOwnerCommand, OwnerDto>
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public UpdateOwnerHandler(IOwnerRepository ownerRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        public async Task<OwnerDto> Handle(UpdateOwnerCommand request, CancellationToken cancellationToken)
        {
            var owner = await _ownerRepository.GetByIdAsync(request.IdOwner);

            if (owner == null)
                throw new KeyNotFoundException($"No se encontró el propietario con ID: {request.IdOwner}");

            // Actualizar los campos
            if(!string.IsNullOrEmpty(request.Name))
                owner.Name = request.Name!;

            if(!string.IsNullOrEmpty(request.Address))
                owner.Address = request.Address!;

            if(request.Birthday != null)
                owner.Birthday = (DateTime)request.Birthday!;

            await _ownerRepository.UpdateAsync(request.IdOwner, owner);

            return _mapper.Map<OwnerDto>(owner);
        }
    }
}

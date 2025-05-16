using Application.Interfaces;
using MediatR;

namespace Application.Commands.Properties
{
    public class UpdatePropertyHandler : IRequestHandler<UpdatePropertyCommand, bool>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IOwnerRepository _ownerRepository;

        public UpdatePropertyHandler(IPropertyRepository propertyRepository, IOwnerRepository ownerRepository)
        {
            _propertyRepository = propertyRepository;
            _ownerRepository = ownerRepository;
        }

        public async Task<bool> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(request.IdProperty);
            if (property == null)
                throw new KeyNotFoundException($"No se encontró la propiedad con ID: {request.IdProperty}");

            if (request.IdOwner != null)
            {
                var ownerExists = await _ownerRepository.ExistsAsync(request.IdOwner);
                if (!ownerExists)
                    throw new KeyNotFoundException($"No existe un propietario con ID: {request.IdOwner}");
                property.IdOwner = request.IdOwner;
            }

            if (request.Name != null) property.Name = request.Name;
            if (request.Address != null) property.Address = request.Address;
            if (request.Price.HasValue) property.Price = request.Price.Value;
            if (request.CodeInternal != null) property.CodeInternal = request.CodeInternal;
            if (request.Year > 0) property.Year = request.Year;
            if (request.IdOwner != null) property.IdOwner = request.IdOwner;

            await _propertyRepository.UpdateAsync(request.IdProperty, property);

            return true;
        }
    }
}

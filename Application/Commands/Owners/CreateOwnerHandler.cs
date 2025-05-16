using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Owners
{
    public class CreateOwnerHandler : IRequestHandler<CreateOwnerCommand, OwnerDto>
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IFileStorageService _fileStorageService;

        public CreateOwnerHandler(IOwnerRepository ownerRepository, IFileStorageService fileStorageService)
        {
            _ownerRepository = ownerRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<OwnerDto> Handle(CreateOwnerCommand request, CancellationToken cancellationToken)
        {
            var photoUrl = await _fileStorageService.UploadFileAsync(request.Photo);

            var owner = new Owner
            {
                Name = request.Name,
                Address = request.Address,
                Birthday = request.Birthday,
                Photo = photoUrl
            };

            await _ownerRepository.InsertAsync(owner);

            return new OwnerDto
            {
                IdOwner = owner.IdOwner,
                Name = request.Name,
            };
        }
    }
}

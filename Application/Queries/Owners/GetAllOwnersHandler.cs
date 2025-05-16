using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Queries.Owners
{
    public class GetAllOwnersHandler : IRequestHandler<GetAllOwnersQuery, List<OwnerDto>>
    {
        private readonly IOwnerRepository _repository;
        private readonly IMapper _mapper;

        public GetAllOwnersHandler(IOwnerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<OwnerDto>> Handle(GetAllOwnersQuery request, CancellationToken cancellationToken)
        {
            var owners = await _repository.GetAllAsync();
            return _mapper.Map<List<OwnerDto>>(owners);
        }
    }
}

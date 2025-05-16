using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Commands.PropertyTraces
{
    public class CreatePropertyTraceHandler : IRequestHandler<CreatePropertyTraceCommand, string>
    {
        private readonly IPropertyTraceRepository _tracePropertyRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyRepository _propertyRepository;

        public CreatePropertyTraceHandler(IPropertyTraceRepository tracePropertyRepository, IMapper mapper, IPropertyRepository propertyRepository)
        {
            _tracePropertyRepository = tracePropertyRepository;
            _mapper = mapper;
            _propertyRepository = propertyRepository;
        }

        public async Task<string> Handle(CreatePropertyTraceCommand request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(request.IdProperty);
            if (property is null)
                throw new KeyNotFoundException($"No se encontró una propiedad con ID: {request.IdProperty}");

            var trace = _mapper.Map<PropertyTrace>(request);
            await _tracePropertyRepository.InsertAsync(trace);
            return trace.IdPropertyTrace;
        }
    }
}

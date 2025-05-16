using Application.Commands.PropertyTraces;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests.Commands.PropertyTraces
{
    public class CreatePropertyTraceHandlerTests
    {
        private readonly Mock<IPropertyTraceRepository> _traceRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPropertyRepository> _propertyRepositoryMock;
        private readonly CreatePropertyTraceHandler _handler;

        public CreatePropertyTraceHandlerTests()
        {
            _traceRepositoryMock = new Mock<IPropertyTraceRepository>();
            _mapperMock = new Mock<IMapper>();
            _propertyRepositoryMock = new Mock<IPropertyRepository>();
            _handler = new CreatePropertyTraceHandler(
                _traceRepositoryMock.Object,
                _mapperMock.Object,
                _propertyRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldInsertTraceProperty_WhenValidRequest()
        {
            // Arrange
            var command = new CreatePropertyTraceCommand(
                IdProperty: "property123",
                DateSale: new DateTime(2024, 1, 1),
                Name: "Test Trace",
                Value: 150000,
                Tax: 5000
            );

            var property = new Property { IdProperty = command.IdProperty };

            var traceProperty = new PropertyTrace
            {
                IdPropertyTrace = "trace789",
                IdProperty = command.IdProperty,
                DateSale = command.DateSale,
                Name = command.Name,
                Value = command.Value,
                Tax = command.Tax
            };

            _propertyRepositoryMock.Setup(r => r.GetByIdAsync(command.IdProperty))
                                   .ReturnsAsync(property);

            _mapperMock.Setup(m => m.Map<PropertyTrace>(command))
                       .Returns(traceProperty);

            _traceRepositoryMock.Setup(r => r.InsertAsync(traceProperty))
                                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(traceProperty.IdPropertyTrace);
            _propertyRepositoryMock.Verify(r => r.GetByIdAsync(command.IdProperty), Times.Once);
            _traceRepositoryMock.Verify(r => r.InsertAsync(traceProperty), Times.Once);
        }
    }
}

using Application.DTOs;
using Application.Interfaces;
using Application.Queries.Properties;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace Application.Tests.Queries.Properties
{
    public class GetPropertyByIdHandlerTests
    {
        private readonly Mock<IPropertyRepository> _propertyRepositoryMock;
        private readonly Mock<IOwnerRepository> _ownerRepositoryMock;
        private readonly Mock<IPropertyImageRepository> _propertyImageRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetPropertyByIdHandler _handler;

        public GetPropertyByIdHandlerTests()
        {
            _propertyRepositoryMock = new Mock<IPropertyRepository>();
            _ownerRepositoryMock = new Mock<IOwnerRepository>();
            _propertyImageRepositoryMock = new Mock<IPropertyImageRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetPropertyByIdHandler(_propertyRepositoryMock.Object, _mapperMock.Object, _ownerRepositoryMock.Object, _propertyImageRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPropertyDto_WhenPropertyExists()
        {
            // Arrange
            var propertyId = "prop123";
            var property = new Property
            {
                IdProperty = propertyId,
                Name = "Casa Bonita",
                Address = "Calle Falsa 123",
                Price = 100000,
                IdOwner = "owner123"
            };

            var propertyDto = new PropertyDto
            {
                IdProperty = propertyId,
                Name = "Casa Bonita",
                Address = "Calle Falsa 123",
                Price = 100000
            };

            _propertyRepositoryMock.Setup(repo => repo.GetByIdAsync(propertyId))
                .ReturnsAsync(property);

            _mapperMock.Setup(m => m.Map<PropertyDto>(property))
                .Returns(propertyDto);

            var query = new GetPropertyByIdQuery { Id = propertyId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(propertyDto.IdProperty, result.IdProperty);
            Assert.Equal(propertyDto.Name, result.Name);
            _propertyRepositoryMock.Verify(r => r.GetByIdAsync(propertyId), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenPropertyDoesNotExist()
        {
            // Arrange
            var propertyId = "nonexistent";
            _propertyRepositoryMock.Setup(repo => repo.GetByIdAsync(propertyId))
                .ReturnsAsync((Property)null);

            var query = new GetPropertyByIdQuery { Id = propertyId };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));

            Assert.Contains(propertyId, exception.Message);
            _propertyRepositoryMock.Verify(r => r.GetByIdAsync(propertyId), Times.Once);
        }
    }
}

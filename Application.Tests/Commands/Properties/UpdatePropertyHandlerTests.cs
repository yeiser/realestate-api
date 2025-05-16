using Application.Commands.Properties;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace Application.Tests.Commands.Properties
{
    public class UpdatePropertyHandlerTests
    {
        private readonly Mock<IPropertyRepository> _propertyRepositoryMock;
        private readonly Mock<IOwnerRepository> _ownerRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdatePropertyHandler _handler;

        public UpdatePropertyHandlerTests()
        {
            _propertyRepositoryMock = new Mock<IPropertyRepository>();
            _ownerRepositoryMock = new Mock<IOwnerRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new UpdatePropertyHandler(
                _propertyRepositoryMock.Object,
                _ownerRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Update_Property_When_Valid_Request()
        {
            // Arrange
            var command = new UpdatePropertyCommand
            {
                IdProperty = "property123",
                Name = "Updated Property",
                Address = "New Address",
                Price = 250000,
                IdOwner = "owner123"
            };

            var existingProperty = new Property { IdProperty = command.IdProperty, IdOwner = command.IdOwner };
            var updatedProperty = new Property { IdProperty = command.IdProperty, Name = command.Name, Address = command.Address, Price = command.Price.Value };

            _propertyRepositoryMock.Setup(r => r.GetByIdAsync(command.IdProperty))
                .ReturnsAsync(existingProperty);

            _ownerRepositoryMock.Setup(r => r.ExistsAsync(command.IdOwner))
                .ReturnsAsync(true);

            _mapperMock.Setup(m => m.Map(command, existingProperty))
                .Returns(updatedProperty);

            _propertyRepositoryMock.Setup(r => r.UpdateAsync(command.IdProperty, updatedProperty))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _propertyRepositoryMock.Verify(r => r.UpdateAsync(command.IdProperty, It.Is<Property>(p =>
                p.IdProperty == command.IdProperty &&
                p.Name == command.Name &&
                p.Address == command.Address &&
                p.Price == command.Price
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_Property_Not_Found()
        {
            // Arrange
            var command = new UpdatePropertyCommand { IdProperty = "notfound" };

            _propertyRepositoryMock.Setup(r => r.GetByIdAsync(command.IdProperty))
                .ReturnsAsync((Property)null);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"No se encontró la propiedad con ID: {command.IdProperty}");
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_Owner_Not_Exists()
        {
            // Arrange
            var command = new UpdatePropertyCommand { IdProperty = "property123", IdOwner = "invalidOwner" };

            _propertyRepositoryMock.Setup(r => r.GetByIdAsync(command.IdProperty))
                .ReturnsAsync(new Property());

            _ownerRepositoryMock.Setup(r => r.ExistsAsync(command.IdOwner))
                .ReturnsAsync(false);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"No existe un propietario con ID: {command.IdOwner}");
        }
    }
}

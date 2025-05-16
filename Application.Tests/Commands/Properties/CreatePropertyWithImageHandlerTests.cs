using Application.Commands.Properties;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests.Commands.Properties
{
    public class CreatePropertyWithImageHandlerTests
    {
        private readonly Mock<IPropertyRepository> _propertyRepositoryMock;
        private readonly Mock<IOwnerRepository> _ownerRepositoryMock;
        private readonly Mock<IPropertyImageRepository> _imageRepositoryMock;
        private readonly Mock<IFileStorageService> _firebaseServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreatePropertyWithImageHandler _handler;

        public CreatePropertyWithImageHandlerTests()
        {
            _propertyRepositoryMock = new Mock<IPropertyRepository>();
            _ownerRepositoryMock = new Mock<IOwnerRepository>();
            _imageRepositoryMock = new Mock<IPropertyImageRepository>();
            _firebaseServiceMock = new Mock<IFileStorageService>();
            _mapperMock = new Mock<IMapper>();

            _handler = new CreatePropertyWithImageHandler(
                _propertyRepositoryMock.Object,
                _ownerRepositoryMock.Object,
                _imageRepositoryMock.Object,
                _firebaseServiceMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldCreatePropertyAndUploadImage()
        {
            // Arrange
            var command = new CreatePropertyWithImageCommand
            {
                Name = "Casa nueva",
                Address = "Av. Siempre Viva",
                Price = 500000,
                IdOwner = "owner123",
                ImageBase64 = "iVBORw0KGgoAAAANSUhEUgAAA..."
            };

            var property = new Property { IdProperty = "prop123" };
            var propertyDto = new PropertyDto { IdProperty = "prop123" };

            _ownerRepositoryMock
                .Setup(r => r.GetByIdAsync("owner123"))
                .ReturnsAsync(new Owner { IdOwner = "owner123", Name = "Juan Pérez" });

            _firebaseServiceMock
                .Setup(s => s.UploadFileAsync(command.ImageBase64))
                .ReturnsAsync("https://firebase.com/images/prop123.jpg");

            _mapperMock
                .Setup(m => m.Map<Property>(command))
                .Returns(property);

            _mapperMock
                .Setup(m => m.Map<PropertyDto>(property))
                .Returns(propertyDto);

            _propertyRepositoryMock
                .Setup(r => r.InsertAsync(It.IsAny<Property>()))
                .Returns(Task.CompletedTask);

            _imageRepositoryMock
                .Setup(r => r.InsertAsync(It.IsAny<PropertyImage>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IdProperty.Should().Be("prop123");

            _firebaseServiceMock.Verify(f => f.UploadFileAsync(It.IsAny<string>()), Times.Once);
            _propertyRepositoryMock.Verify(r => r.InsertAsync(property));
            _imageRepositoryMock.Verify(r => r.InsertAsync(It.IsAny<PropertyImage>()), Times.Once);
        }
    }
}

using Application.Commands.Owners;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests.Commands.Owners
{
    public class CreateOwnerHandlerTests
    {
        private readonly Mock<IOwnerRepository> _repositoryMock;
        private readonly Mock<IFileStorageService> _storageServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateOwnerHandler _handler;

        public CreateOwnerHandlerTests()
        {
            _repositoryMock = new Mock<IOwnerRepository>();
            _storageServiceMock = new Mock<IFileStorageService>();
            _mapperMock = new Mock<IMapper>();

            _handler = new CreateOwnerHandler(
                _repositoryMock.Object,
                _storageServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateOwner_WhenDataIsValid()
        {
            // Arrange
            var base64 = "fakebase64string";
            var imageUrl = "https://fakeurl.com/photo.png";

            var command = new CreateOwnerCommand
            {
                Name = "Carlos",
                Address = "Calle Falsa 123",
                Photo = base64
            };

            var owner = new Owner { IdOwner = "1", Name = "Carlos", Address = "Calle Falsa 123", Photo = imageUrl };
            var dto = new OwnerDto { IdOwner = "1", Name = "Carlos"};

            _storageServiceMock.Setup(s => s.UploadFileAsync(base64)).ReturnsAsync(imageUrl);
            _mapperMock.Setup(m => m.Map<Owner>(command)).Returns(owner);
            _repositoryMock.Setup(r => r.InsertAsync(owner)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<OwnerDto>(owner)).Returns(dto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Carlos");
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUploadFails()
        {
            // Arrange
            var base64 = "invalidbase64";
            var command = new CreateOwnerCommand { Name = "Carlos", Photo = base64 };

            _storageServiceMock.Setup(s => s.UploadFileAsync(base64))
                .ThrowsAsync(new Exception("Upload failed"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Upload failed");
        }
    }
}

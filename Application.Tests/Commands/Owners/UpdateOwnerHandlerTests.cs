using Application.Commands.Owners;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests.Commands.Owners
{
    public class UpdateOwnerHandlerTests
    {
        private readonly Mock<IOwnerRepository> _ownerRepositoryMock;
        private readonly IMapper _mapper;
        private readonly UpdateOwnerHandler _handler;

        public UpdateOwnerHandlerTests()
        {
            _ownerRepositoryMock = new Mock<IOwnerRepository>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Owner, OwnerDto>();
            });

            _mapper = mapperConfig.CreateMapper();
            _handler = new UpdateOwnerHandler(_ownerRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldUpdateOwner_AndReturnDto_WhenOwnerExists()
        {
            // Arrange
            var command = new UpdateOwnerCommand
            {
                IdOwner = "1",
                Name = "John Updated",
                Address = "New Address"
            };

            var existingOwner = new Owner
            {
                IdOwner = "1",
                Name = "John",
                Address = "Old Address"
            };

            _ownerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(command.IdOwner))
                .ReturnsAsync(existingOwner);

            _ownerRepositoryMock
                .Setup(repo => repo.UpdateAsync(command.IdOwner, It.IsAny<Owner>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IdOwner.Should().Be(command.IdOwner);
            result.Name.Should().Be(command.Name);

            _ownerRepositoryMock.Verify(repo => repo.UpdateAsync(command.IdOwner, It.IsAny<Owner>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenOwnerDoesNotExist()
        {
            // Arrange
            var command = new UpdateOwnerCommand
            {
                IdOwner = "notfound",
                Name = "Someone",
                Address = "Somewhere"
            };

            _ownerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(command.IdOwner))
                .ReturnsAsync((Owner?)null);

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"No se encontró el propietario con ID: {command.IdOwner}");

            _ownerRepositoryMock.Verify(repo => repo.UpdateAsync(command.IdOwner, It.IsAny<Owner>()), Times.Never);
        }
    }
}

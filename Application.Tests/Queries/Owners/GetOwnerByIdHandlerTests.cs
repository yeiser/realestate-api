using Application.Commands.Owners;
using Application.DTOs;
using Application.Interfaces;
using Application.Queries.Owners;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests.Queries.Owners
{
    public class GetOwnerByIdHandlerTests
    {
        private readonly Mock<IOwnerRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetOwnerByIdHandler _handler;

        public GetOwnerByIdHandlerTests()
        {
            _repositoryMock = new Mock<IOwnerRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetOwnerByIdHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnOwnerDto_WhenOwnerExists()
        {
            // Arrange
            var ownerId = "123";
            var owner = new Owner { IdOwner = ownerId, Name = "Juan" };
            var ownerDto = new OwnerDto { Name = "Juan" };

            _repositoryMock.Setup(r => r.GetByIdAsync(ownerId)).ReturnsAsync(owner);
            _mapperMock.Setup(m => m.Map<OwnerDto>(owner)).Returns(ownerDto);

            var request = new GetOwnerByIdQuery(ownerId);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Juan");
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenOwnerDoesNotExist()
        {
            // Arrange
            var ownerId = "123";
            _repositoryMock.Setup(r => r.GetByIdAsync(ownerId)).ReturnsAsync((Owner)null!);

            var request = new GetOwnerByIdQuery(ownerId);

            // Act
            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"No se encontró un propietario con ID: {ownerId}");
        }
    }
}

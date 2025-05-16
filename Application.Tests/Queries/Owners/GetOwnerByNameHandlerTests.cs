using Application.DTOs;
using Application.Interfaces;
using Application.Queries.Owners;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests.Queries.Owners
{
    public class GetOwnerByNameHandlerTests
    {
        private readonly Mock<IOwnerRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetOwnerByNameHandler _handler;

        public GetOwnerByNameHandlerTests()
        {
            _repositoryMock = new Mock<IOwnerRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetOwnerByNameHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnOwnerDtos_WhenMatchingOwnersExist()
        {
            // Arrange
            var name = "Luis";
            var owners = new List<Owner>
            {
                new Owner { IdOwner = "1", Name = "Luis Perez" },
                new Owner { IdOwner = "2", Name = "Luis Gómez" }
            };

            var dtos = new List<OwnerDto>
            {
                new OwnerDto { Name = "Luis Perez" },
                new OwnerDto { Name = "Luis Gómez" }
            };

            _repositoryMock
                .Setup(r => r.FindByPropertyAsync(o => o.Name, name))
                .ReturnsAsync(owners);

            _mapperMock
                .Setup(m => m.Map<List<OwnerDto>>(owners))
                .Returns(dtos);

            var request = new GetOwnerByNameQuery(name);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(x => x.Name == "Luis Perez");
            result.Should().Contain(x => x.Name == "Luis Gómez");
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenNoOwnersFound()
        {
            // Arrange
            var name = "Inexistente";
            _repositoryMock
                .Setup(r => r.FindByPropertyAsync(o => o.Name, name))
                .ReturnsAsync(new List<Owner>());

            var request = new GetOwnerByNameQuery(name);

            // Act
            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"No se encontró ningún propietario con el nombre: {name}");
        }
    }
}

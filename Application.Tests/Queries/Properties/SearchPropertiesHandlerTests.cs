using Application.DTOs;
using Application.Interfaces;
using Application.Queries.Properties;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace Application.Tests.Queries.Properties
{
    public class SearchPropertiesHandlerTests
    {
        private readonly Mock<IPropertyRepository> _repositoryMock;
        private readonly Mock<IPropertyImageRepository> _repositoryImageMock;
        private readonly IMapper _mapper;
        private readonly SearchPropertiesHandler _handler;

        public SearchPropertiesHandlerTests()
        {
            _repositoryMock = new Mock<IPropertyRepository>();
            _repositoryImageMock = new Mock<IPropertyImageRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Property, PropertyDto>();
            });

            _mapper = config.CreateMapper();
            _handler = new SearchPropertiesHandler(_repositoryMock.Object, _mapper, _repositoryImageMock.Object);
        }

        
        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoMatches()
        {
            // Arrange
            var query = new SearchPropertiesQuery
            {
                Name = "Inexistente"
            };

            _repositoryMock
                .Setup(r => r.SearchAsync(query.Name, null, null, null))
                .ReturnsAsync(new List<Property>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}

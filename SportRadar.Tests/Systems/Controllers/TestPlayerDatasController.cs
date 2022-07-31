using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportRadar.API.Controllers;
using SportRadar.API.Models;
using SportRadar.API.Services.Interfaces;
using SportRadar.API.Utilities.Interfaces;
using SportRadar.Tests.Fixtures;

namespace SportRadar.Tests.Systems.Controllers
{
    public class TestPlayerDatasController
    {

        [Fact]
        public async Task GetByIdAndSeason_OnSuccess_ReturnsFileContentResult()
        {
            // Arrange

            var mockPlayerDataService = new Mock<IPlayerDataService>();
            mockPlayerDataService.Setup(service => service.GetPlayerDataByIdAndSeason(It.IsAny<long>(), It.IsAny<int>())).ReturnsAsync(PlayerDatasFixture.GetTestPlayerData());

            var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);
            mockSerializer.Setup(serializer => serializer.SerializeAsync(It.IsAny<PlayerData>())).ReturnsAsync(PlayerDatasFixture.GetTestPlayerCsv());

            var sut = new PlayerDatasController(mockPlayerDataService.Object, mockSerializer.Object);

            // Act

            var result = await sut.GetByIdAndSeason(It.IsAny<long>(), It.IsAny<int>());

            // Assert

            result.Should().BeOfType<FileContentResult>();
        }

        [Fact]
        public async Task GetByIdAndSeason_OnSuccess_ReturnsCsvContentType()
        {
            // Arrange

            var mockPlayerDataService = new Mock<IPlayerDataService>();
            mockPlayerDataService.Setup(service => service.GetPlayerDataByIdAndSeason(It.IsAny<long>(), It.IsAny<int>())).ReturnsAsync(PlayerDatasFixture.GetTestPlayerData());

            var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);
            mockSerializer.Setup(serializer => serializer.SerializeAsync(It.IsAny<PlayerData>())).ReturnsAsync(PlayerDatasFixture.GetTestPlayerCsv());

            var sut = new PlayerDatasController(mockPlayerDataService.Object, mockSerializer.Object);

            // Act

            var result = await sut.GetByIdAndSeason(It.IsAny<long>(), It.IsAny<int>());

            // Assert

            result.Should().BeOfType<FileContentResult>();
            var fileContentResult = (FileContentResult)result;
            fileContentResult.ContentType.Should().Be("text/csv");
        }

        [Fact]
        public async Task GetByIdAndSeason_OnNoPlayerDataFound_Returns404()
        {
            // Arrange

            var mockPlayerDataService = new Mock<IPlayerDataService>();
            mockPlayerDataService.Setup(service => service.GetPlayerDataByIdAndSeason(It.IsAny<long>(), It.IsAny<int>())).ReturnsAsync(new PlayerData());

            var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);
            mockSerializer.Setup(serializer => serializer.SerializeAsync(It.IsAny<PlayerData>())).ReturnsAsync(String.Empty);

            var sut = new PlayerDatasController(mockPlayerDataService.Object, mockSerializer.Object);

            // Act

            var result = await sut.GetByIdAndSeason(It.IsAny<long>(), It.IsAny<int>());

            // Assert

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetByIdAndSeason_OnSuccess_InvokesPlayerDataServiceExactlyOnce()
        {
            // Arrange

            var mockPlayerDataService = new Mock<IPlayerDataService>();
            mockPlayerDataService.Setup(service => service.GetPlayerDataByIdAndSeason(It.IsAny<long>(), It.IsAny<int>())).ReturnsAsync(PlayerDatasFixture.GetTestPlayerData());

            var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);
            mockSerializer.Setup(serializer => serializer.SerializeAsync(It.IsAny<PlayerData>())).ReturnsAsync(PlayerDatasFixture.GetTestPlayerCsv());

            var sut = new PlayerDatasController(mockPlayerDataService.Object, mockSerializer.Object);

            // Act

            await sut.GetByIdAndSeason(It.IsAny<long>(), It.IsAny<int>());

            // Assert

            mockPlayerDataService.Verify(service => service.GetPlayerDataByIdAndSeason(It.IsAny<long>(), It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public async Task GetByIdAndSeason_OnSuccess_InvokesCsvSerializerExactlyOnce()
        {
            // Arrange

            var mockPlayerDataService = new Mock<IPlayerDataService>();
            mockPlayerDataService.Setup(service => service.GetPlayerDataByIdAndSeason(It.IsAny<long>(), It.IsAny<int>())).ReturnsAsync(PlayerDatasFixture.GetTestPlayerData());

            var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);
            mockSerializer.Setup(serializer => serializer.SerializeAsync(It.IsAny<PlayerData>())).ReturnsAsync(PlayerDatasFixture.GetTestPlayerCsv());

            var sut = new PlayerDatasController(mockPlayerDataService.Object, mockSerializer.Object);

            // Act

            await sut.GetByIdAndSeason(It.IsAny<long>(), It.IsAny<int>());

            // Assert

            mockSerializer.Verify(serializer => serializer.SerializeAsync(It.IsAny<PlayerData>()), Times.Once());
        }

        [Fact]
        public async Task GetByIdAndSeason_OnSuccess_Returns147ByteFile()
        {
            // Arrange

            var mockPlayerDataService = new Mock<IPlayerDataService>();
            mockPlayerDataService.Setup(service => service.GetPlayerDataByIdAndSeason(It.IsAny<long>(), It.IsAny<int>())).ReturnsAsync(PlayerDatasFixture.GetTestPlayerData());

            var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);
            mockSerializer.Setup(serializer => serializer.SerializeAsync(It.IsAny<PlayerData>())).ReturnsAsync(PlayerDatasFixture.GetTestPlayerCsv());

            var sut = new PlayerDatasController(mockPlayerDataService.Object, mockSerializer.Object);

            // Act

            var result = await sut.GetByIdAndSeason(It.IsAny<long>(), It.IsAny<int>());

            // Assert

            result.Should().BeOfType<FileContentResult>();
            var fileContentResult = (FileContentResult)result;
            fileContentResult.ContentType.Should().Be("text/csv");
            fileContentResult.FileContents.Length.Should().Be(147);
        }

    }
}

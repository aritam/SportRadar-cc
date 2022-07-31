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
    public class TestTeamDatasController
    {

        [Fact]
        public async Task GetByIdAndSeason_OnSuccess_ReturnsFileContentResult()
        {
            // Arrange

            var mockTeamDataService = new Mock<ITeamDataService>();
            mockTeamDataService.Setup(service => service.GetTeamDataByIdAndSeason(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(TeamDatasFixture.GetTestTeamData());

            var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);
            mockSerializer.Setup(serializer => serializer.SerializeAsync(It.IsAny<TeamData>())).ReturnsAsync(TeamDatasFixture.GetTestTeamCsv());

            var sut = new TeamDatasController(mockTeamDataService.Object, mockSerializer.Object);

            // Act

            var result = await sut.GetByIdAndSeason(It.IsAny<int>(), It.IsAny<int>());

            // Assert

            result.Should().BeOfType<FileContentResult>();
        }

        [Fact]
        public async Task GetByIdAndSeason_OnSuccess_ReturnsCsvContentType()
        {
            // Arrange

            var mockTeamDataService = new Mock<ITeamDataService>();
            mockTeamDataService.Setup(service => service.GetTeamDataByIdAndSeason(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(TeamDatasFixture.GetTestTeamData());

            var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);
            mockSerializer.Setup(serializer => serializer.SerializeAsync(It.IsAny<TeamData>())).ReturnsAsync(TeamDatasFixture.GetTestTeamCsv());

            var sut = new TeamDatasController(mockTeamDataService.Object, mockSerializer.Object);

            // Act

            var result = await sut.GetByIdAndSeason(It.IsAny<int>(), It.IsAny<int>());

            // Assert

            result.Should().BeOfType<FileContentResult>();
            var fileContentResult = (FileContentResult)result;
            fileContentResult.ContentType.Should().Be("text/csv");
        }

        [Fact]
        public async Task GetByIdAndSeason_OnNoTeamDataFound_Returns404()
        {
            // Arrange

            var mockTeamDataService = new Mock<ITeamDataService>();
            mockTeamDataService.Setup(service => service.GetTeamDataByIdAndSeason(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new TeamData());

            var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);
            mockSerializer.Setup(serializer => serializer.SerializeAsync(It.IsAny<TeamData>())).ReturnsAsync(String.Empty);

            var sut = new TeamDatasController(mockTeamDataService.Object, mockSerializer.Object);

            // Act

            var result = await sut.GetByIdAndSeason(It.IsAny<int>(), It.IsAny<int>());

            // Assert

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetByIdAndSeason_OnSuccess_InvokesTeamDataServiceExactlyOnce()
        {
            // Arrange

            var mockTeamDataService = new Mock<ITeamDataService>();
            mockTeamDataService.Setup(service => service.GetTeamDataByIdAndSeason(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(TeamDatasFixture.GetTestTeamData());

            var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);
            mockSerializer.Setup(serializer => serializer.SerializeAsync(It.IsAny<TeamData>())).ReturnsAsync(TeamDatasFixture.GetTestTeamCsv());

            var sut = new TeamDatasController(mockTeamDataService.Object, mockSerializer.Object);

            // Act

            await sut.GetByIdAndSeason(It.IsAny<int>(), It.IsAny<int>());

            // Assert

            mockTeamDataService.Verify(service => service.GetTeamDataByIdAndSeason(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public async Task GetByIdAndSeason_OnSuccess_InvokesCsvSerializerExactlyOnce()
        {
            // Arrange

            var mockTeamDataService = new Mock<ITeamDataService>();
            mockTeamDataService.Setup(service => service.GetTeamDataByIdAndSeason(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(TeamDatasFixture.GetTestTeamData());

            var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);
            mockSerializer.Setup(serializer => serializer.SerializeAsync(It.IsAny<TeamData>())).ReturnsAsync(TeamDatasFixture.GetTestTeamCsv());

            var sut = new TeamDatasController(mockTeamDataService.Object, mockSerializer.Object);

            // Act

            await sut.GetByIdAndSeason(It.IsAny<int>(), It.IsAny<int>());

            // Assert

            mockSerializer.Verify(serializer => serializer.SerializeAsync(It.IsAny<TeamData>()), Times.Once());
        }

        [Fact]
        public async Task GetByIdAndSeason_OnSuccess_Returns180ByteFile()
        {
            // Arrange

            var mockTeamDataService = new Mock<ITeamDataService>();
            mockTeamDataService.Setup(service => service.GetTeamDataByIdAndSeason(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(TeamDatasFixture.GetTestTeamData());

            var mockSerializer = new Mock<ISerializer>(MockBehavior.Strict);
            mockSerializer.Setup(serializer => serializer.SerializeAsync(It.IsAny<TeamData>())).ReturnsAsync(TeamDatasFixture.GetTestTeamCsv());

            var sut = new TeamDatasController(mockTeamDataService.Object, mockSerializer.Object);

            // Act

            var result = await sut.GetByIdAndSeason(It.IsAny<int>(), It.IsAny<int>());

            // Assert

            result.Should().BeOfType<FileContentResult>();
            var fileContentResult = (FileContentResult)result;
            fileContentResult.ContentType.Should().Be("text/csv");
            fileContentResult.FileContents.Length.Should().Be(180);
        }
    }
}

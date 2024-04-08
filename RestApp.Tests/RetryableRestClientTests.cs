using Moq;
using System.Net;
using Microsoft.Extensions.Logging;
using RestApp.Interface;

namespace RestApp.Tests
{
    public class RetryableRestClientTests
    {
        [Fact]
        public async Task Get_Successful_First_Attempt()
        {
            // Arrange
            var mockRestClient = new Mock<IRestClient>();
            var mockLogger = new Mock<ILogger>();

            var retryableRestClient = new RetryableRestClient(mockRestClient.Object, mockLogger.Object);

            var expectedResult = "success";
            mockRestClient.Setup(r => r.Get<string>(It.IsAny<string>())).ReturnsAsync(expectedResult);

            // Act
            var result = await retryableRestClient.Get<string>("testUrl");

            // Assert
            Assert.Equal(expectedResult, result);
            mockRestClient.Verify(r => r.Get<string>("testUrl"), Times.Once);
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Never);
        }

        [Fact]
        public async Task Get_Successful_After_Retries()
        {
            // Arrange
            var mockRestClient = new Mock<IRestClient>();
            var mockLogger = new Mock<ILogger>();
            var expectedResult = "success";

            mockRestClient.SetupSequence(r => r.Get<string>(It.IsAny<string>()))
                          .Throws(new WebException())
                          .Throws(new WebException())
                          .ReturnsAsync(expectedResult);

            var retryableRestClient = new RetryableRestClient(mockRestClient.Object, mockLogger.Object);

            // Act
            var result = await retryableRestClient.Get<string>("testUrl");

            // Assert
            Assert.Equal(expectedResult, result);
            mockRestClient.Verify(r => r.Get<string>("testUrl"), Times.Exactly(3));
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Exactly(2));
        }

        [Fact]
        public async Task Put_Successful_First_Attempt()
        {
            // Arrange
            var mockRestClient = new Mock<IRestClient>();
            var mockLogger = new Mock<ILogger>();

            var retryableRestClient = new RetryableRestClient(mockRestClient.Object, mockLogger.Object);

            var expectedResult = "success";
            mockRestClient.Setup(r => r.Put<string>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(expectedResult);

            // Act
            var result = await retryableRestClient.Put<string>("testUrl", "testModel");

            // Assert
            Assert.Equal(expectedResult, result);
            mockRestClient.Verify(r => r.Put<string>("testUrl", "testModel"), Times.Once);
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Never);
        }

        [Fact]
        public async Task Put_Successful_After_Retries()
        {
            // Arrange
            var mockRestClient = new Mock<IRestClient>();
            var mockLogger = new Mock<ILogger>();

            var expectedResult = "success";
            mockRestClient.SetupSequence(r => r.Put<string>(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new WebException())
                .Throws(new WebException())
                .ReturnsAsync(expectedResult);

            var retryableRestClient = new RetryableRestClient(mockRestClient.Object, mockLogger.Object);

            // Act
            var result = await retryableRestClient.Put<string>("testUrl", "testModel");

            // Assert
            Assert.Equal(expectedResult, result);
            mockRestClient.Verify(r => r.Put<string>("testUrl", "testModel"), Times.Exactly(3));
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Exactly(2));
        }

        [Fact]
        public async Task Post_Successful_First_Attempt()
        {
            // Arrange
            var mockRestClient = new Mock<IRestClient>();
            var mockLogger = new Mock<ILogger>();

            var retryableRestClient = new RetryableRestClient(mockRestClient.Object, mockLogger.Object);

            var expectedResult = "success";
            mockRestClient.Setup(r => r.Post<string>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(expectedResult);

            // Act
            var result = await retryableRestClient.Post<string>("testUrl", "testModel");

            // Assert
            Assert.Equal(expectedResult, result);
            mockRestClient.Verify(r => r.Post<string>("testUrl", "testModel"), Times.Once);
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Never);
        }

        [Fact]
        public async Task Post_Successful_After_Retries()
        {
            // Arrange
            var mockRestClient = new Mock<IRestClient>();
            var mockLogger = new Mock<ILogger>();

            var expectedResult = "success";
            mockRestClient.SetupSequence(r => r.Post<string>(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new WebException())
                .Throws(new WebException())
                .ReturnsAsync(expectedResult);

            var retryableRestClient = new RetryableRestClient(mockRestClient.Object, mockLogger.Object);

            // Act
            var result = await retryableRestClient.Post<string>("testUrl", "testModel");

            // Assert
            Assert.Equal(expectedResult, result);
            mockRestClient.Verify(r => r.Post<string>("testUrl", "testModel"), Times.Exactly(3));
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Exactly(2));
        }

        [Fact]
        public async Task Delete_Successful_First_Attempt()
        {
            // Arrange
            var mockRestClient = new Mock<IRestClient>();
            var mockLogger = new Mock<ILogger>();

            var retryableRestClient = new RetryableRestClient(mockRestClient.Object, mockLogger.Object);

            var expectedResult = "success";
            mockRestClient.Setup(r => r.Delete<string>(It.IsAny<int>())).ReturnsAsync(expectedResult);

            // Act
            var result = await retryableRestClient.Delete<string>(123);

            // Assert
            Assert.Equal(expectedResult, result);
            mockRestClient.Verify(r => r.Delete<string>(123), Times.Once);
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Never);
        }

        [Fact]
        public async Task Delete_Successful_After_Retries()
        {
            // Arrange
            var mockRestClient = new Mock<IRestClient>();
            var mockLogger = new Mock<ILogger>();

            var expectedResult = "success";
            mockRestClient.SetupSequence(r => r.Delete<string>(It.IsAny<int>()))
                .Throws(new WebException())
                .Throws(new WebException())
                .ReturnsAsync(expectedResult);

            var retryableRestClient = new RetryableRestClient(mockRestClient.Object, mockLogger.Object);

            // Act
            var result = await retryableRestClient.Delete<string>(123);

            // Assert
            Assert.Equal(expectedResult, result);
            mockRestClient.Verify(r => r.Delete<string>(123), Times.Exactly(3));
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Exactly(2));
        }
    }
}

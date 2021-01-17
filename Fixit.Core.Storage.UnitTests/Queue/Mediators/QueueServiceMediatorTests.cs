﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.Storage.Queue.Adapters;
using Fixit.Core.Storage.Queue.Mediators;
using Fixit.Core.Storage.Queue.Mediators.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fixit.Core.Storage.UnitTests.Queue.Mediators
{
  [TestClass]
  public class QueueServiceMediatorTests : TestBase
  {
    private IQueueServiceMediator _queueServiceMediator;

    #region TestInitialize
    [TestInitialize]
    public void TestInitialize()
    {
      _queueServiceAdapter = new Mock<IQueueServiceAdapter>();
      _queueAdapter = new Mock<IQueueAdapter>();

      _queueServiceMediator = new QueueServiceMediator(_queueServiceAdapter.Object, _mapperConfiguration.CreateMapper());
    }
    #endregion

    #region CreateQueueAsync
    [TestMethod]
    [DataRow(null, DisplayName = "Null_QueueName")]
    public async Task CreateQueueAsync_QueueNameNullOrWhiteSpace_ThrowsArgumentNullException(string queueName)
    {
      // Arrange
      // Act
      // Assert
      await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _queueServiceMediator.CreateQueueAsync(queueName));
    }

    [TestMethod]
    [DataRow("queue", DisplayName = "Any_QueueName")]
    public async Task CreateQueueAsync_CreateQueueSuccess_ReturnsSuccess(string queueName)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;

      _queueServiceAdapter.Setup(queueServiceAdapter => queueServiceAdapter.CreateQueueAsync(queueName, null, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(_queueAdapter.Object);

      // Act
      var actionResult = await _queueServiceMediator.CreateQueueAsync(queueName);

      // Assert
      Assert.IsNotNull(actionResult);
    }
    #endregion

    #region DeleteQueueAsync
    [TestMethod]
    [DataRow(null, DisplayName = "Null_QueueName")]
    public async Task DeleteQueueAsync_QueueNameNullOrWhiteSpace_ThrowsArgumentNullException(string queueName)
    {
      // Arrange
      // Act
      // Assert
      await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _queueServiceMediator.DeleteQueueAsync(queueName));
    }

    [TestMethod]
    [DataRow("queue", DisplayName = "Any_QueueName")]
    public async Task DeleteQueueAsync_DeleteQueueException_ReturnsException(string queueName)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;

      _queueServiceAdapter.Setup(queueServiceAdapter => queueServiceAdapter.DeleteQueueAsync(queueName, It.IsAny<CancellationToken>()))
                          .Throws(new Exception());

      // Act
      var actionResult = await _queueServiceMediator.DeleteQueueAsync(queueName);

      // Assert
      Assert.IsNotNull(actionResult.OperationException);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("queue", DisplayName = "Any_QueueName")]
    public async Task DeleteQueueAsync_DeleteQueueFailure_ReturnsFailure(string queueName)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;

      _queueServiceAdapter.Setup(queueServiceAdapter => queueServiceAdapter.DeleteQueueAsync(queueName, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(404);

      // Act
      var actionResult = await _queueServiceMediator.DeleteQueueAsync(queueName);

      // Assert
      Assert.IsNull(actionResult.OperationException);
      Assert.IsNotNull(actionResult.OperationMessage);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("queue", DisplayName = "Any_QueueName")]
    public async Task DeleteQueueAsync_DeleteQueueSuccess_ReturnsSuccess(string queueName)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;

      _queueServiceAdapter.Setup(queueServiceAdapter => queueServiceAdapter.DeleteQueueAsync(queueName, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(200);

      // Act
      var actionResult = await _queueServiceMediator.DeleteQueueAsync(queueName);

      // Assert
      Assert.IsNull(actionResult.OperationException);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
    }
    #endregion

    #region GetQueueClient
    [TestMethod]
    [DataRow(null, DisplayName = "Null_QueueName")]
    public void GetQueueClient_QueueNameNullOrWhiteSpace_ThrowsArgumentNullException(string queueName)
    {
      // Arrange
      // Act
      // Assert
      Assert.ThrowsException<ArgumentNullException>(() => _queueServiceMediator.GetQueueClient(queueName));
    }

    [TestMethod]
    [DataRow("queue", DisplayName = "Any_QueueName")]
    public void GetQueueClient_GetQueueSuccess_ReturnsSuccess(string queueName)
    {
      // Arrange
      _queueServiceAdapter.Setup(queueServiceAdapter => queueServiceAdapter.GetQueueClient(queueName))
                          .Returns(_queueAdapter.Object);

      // Act
      var actionResult = _queueServiceMediator.GetQueueClient(queueName);

      // Assert
      Assert.IsNotNull(actionResult);
    }
    #endregion

    #region TestCleanup
    [TestCleanup]
    public void TestCleanup()
    {
      // Clean-up mock objects
      _queueServiceAdapter.Reset();
      _queueAdapter.Reset();

      // Clean-up data objects
    }
    #endregion
  }
}

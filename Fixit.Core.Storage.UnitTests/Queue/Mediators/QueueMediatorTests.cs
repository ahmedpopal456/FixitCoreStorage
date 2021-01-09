using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Azure;
using Azure.Storage.Queues.Models;
using Fixit.Core.Storage.DataContracts.Queue;
using Fixit.Core.Storage.Queue.Adapters;
using Fixit.Core.Storage.Queue.Mediators;
using Fixit.Core.Storage.Queue.Mediators.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fixit.Core.Storage.UnitTests.Queue.Mediators
{
  [TestClass]
  public class QueueMediatorTests : TestBase
  {
    private IQueueMediator _queueMediator;

    private Mock<IMapper> _mapper;
    private Mock<QueueMessage> _queueMessage;
    private Mock<Response<SendReceipt>> _sendResponse;
    private Mock<Response<UpdateReceipt>> _updateResponse;

    private IEnumerable<MessageDto> _fakeMessageDtos;

    #region TestInitialize
    [TestInitialize]
    public void TestInitialize()
    {
      _queueAdapter = new Mock<IQueueAdapter>();

      _mapper = new Mock<IMapper>();

      _queueMessage = new Mock<QueueMessage>();
      _sendResponse = new Mock<Response<SendReceipt>>();
      _updateResponse = new Mock<Response<UpdateReceipt>>();

      // Create Seeders
      var fakeMessageDtoSeeder = fakeDtoSeederFactory.CreateFakeSeeder<MessageDto>();

      // Create fake data objects
      _fakeMessageDtos = fakeMessageDtoSeeder.SeedFakeDtos();

      _queueMediator = new QueueMediator(_queueAdapter.Object, _mapper.Object);
    }
    #endregion

    #region DeleteMessageAsync
    [TestMethod]
    [DataRow(null, "123", DisplayName = "Null_MessageId")]
    public async Task DeleteMessageAsync_MessageIdNullOrWhiteSpace_ThrowsArgumentNullException(string messageId, string popReceipt)
    {
      // Arrange
      // Act
      // Assert
      await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _queueMediator.DeleteMessageAsync(messageId, popReceipt));
    }

    [TestMethod]
    [DataRow("123", null, DisplayName = "Null_PopReceipt")]
    public async Task DeleteMessageAsync_PopReceiptNullOrWhiteSpace_ThrowsArgumentNullException(string messageId, string popReceipt)
    {
      // Arrange
      // Act
      // Assert
      await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _queueMediator.DeleteMessageAsync(messageId, popReceipt));
    }

    [TestMethod]
    [DataRow("123", "123", DisplayName = "Any_MessageId_PopReceipt")]
    public async Task DeleteMessageAsync_DeleteMessageException_ReturnsException(string messageId, string popReceipt)
    {
      // Arrange
      _queueAdapter.Setup(queueAdapter => queueAdapter.DeleteMessageAsync(messageId, popReceipt, It.IsAny<CancellationToken>()))
                   .Throws(new Exception());

      // Act
      var actionResult = await _queueMediator.DeleteMessageAsync(messageId, popReceipt);

      // Assert
      Assert.IsNotNull(actionResult.OperationException);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("123", "123", DisplayName = "Any_MessageId_PopReceipt")]
    public async Task DeleteMessageAsync_DeleteMessageFailure_ReturnsFailure(string messageId, string popReceipt)
    {
      // Arrange
      _queueAdapter.Setup(queueAdapter => queueAdapter.DeleteMessageAsync(messageId, popReceipt, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(HttpStatusCode.NotFound);

      // Act
      var actionResult = await _queueMediator.DeleteMessageAsync(messageId, popReceipt);

      // Assert
      Assert.IsNull(actionResult.OperationException);
      Assert.IsNotNull(actionResult.OperationMessage);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("123", "123", DisplayName = "Any_MessageId_PopReceipt")]
    public async Task DeleteMessageAsync_DeleteMessageSuccess_ReturnsSuccess(string messageId, string popReceipt)
    {
      // Arrange
      _queueAdapter.Setup(queueAdapter => queueAdapter.DeleteMessageAsync(messageId, popReceipt, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(HttpStatusCode.OK);

      // Act
      var actionResult = await _queueMediator.DeleteMessageAsync(messageId, popReceipt);

      // Assert
      Assert.IsNull(actionResult.OperationException);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
    }
    #endregion

    #region ReceiveMessageAsync
    [TestMethod]
    public async Task ReceiveMessageAsync_ReceiveMessageException_ReturnsException()
    {
      // Arrange
      _queueAdapter.Setup(queueAdapter => queueAdapter.ReceiveMessageAsync(null, It.IsAny<CancellationToken>()))
                   .Throws(new Exception());

      // Act
      var actionResult = await _queueMediator.ReceiveMessageAsync();

      // Assert
      Assert.IsNotNull(actionResult.OperationException);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    public async Task ReceiveMessageAsync_ReceiveMessageSuccess_ReturnsSuccess()
    {
      // Arrange
      _queueAdapter.Setup(queueAdapter => queueAdapter.ReceiveMessageAsync(null, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(_queueMessage.Object);
      _mapper.Setup(mapper => mapper.Map<QueueMessage, MessageDto>(It.IsAny<QueueMessage>()))
             .Returns(_fakeMessageDtos.First());

      // Act
      var actionResult = await _queueMediator.ReceiveMessageAsync();

      // Assert
      Assert.IsNotNull(actionResult.Message);
      Assert.IsNull(actionResult.OperationException);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
    }
    #endregion

    #region ReceiveMessagesAsync
    [TestMethod]
    public async Task ReceiveMessagesAsync_ReceiveMessagesException_ReturnsException()
    {
      // Arrange
      _queueAdapter.Setup(queueAdapter => queueAdapter.ReceiveMessagesAsync(null, null, It.IsAny<CancellationToken>()))
                   .Throws(new Exception());

      // Act
      var actionResult = await _queueMediator.ReceiveMessagesAsync();

      // Assert
      Assert.IsNotNull(actionResult.OperationException);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    public async Task ReceiveMessagesAsync_ReceiveMessagesSuccess_ReturnsSuccess()
    {
      // Arrange
      _queueAdapter.Setup(queueAdapter => queueAdapter.ReceiveMessagesAsync(null, null, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new QueueMessage[] { _queueMessage.Object, _queueMessage.Object });
      _mapper.Setup(mapper => mapper.Map<QueueMessage, MessageDto>(It.IsAny<QueueMessage>()))
             .Returns(_fakeMessageDtos.First());

      // Act
      var actionResult = await _queueMediator.ReceiveMessagesAsync();

      // Assert
      Assert.IsNotNull(actionResult.Messages);
      Assert.AreEqual(actionResult.Messages.Count, 2);
      Assert.IsNull(actionResult.OperationException);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
    }
    #endregion

    #region SendMessageAsync
    [TestMethod]
    [DataRow(null, DisplayName = "Null_MessageText")]
    public async Task SendMessageAsync_MessageTextNullOrEmpty_ThrowsArgumentNullException(string messageText)
    {
      // Arrange
      // Act
      // Assert
      await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _queueMediator.SendMessageAsync(messageText));
    }

    [TestMethod]
    [DataRow("Hello World", DisplayName = "Any_MessageText")]
    public async Task SendMessageAsync_SendMessageException_ReturnsException(string messageText)
    {
      // Arrange
      _queueAdapter.Setup(queueAdapter => queueAdapter.SendMessageAsync(messageText, null, null, It.IsAny<CancellationToken>()))
                   .Throws(new Exception());

      // Act
      var actionResult = await _queueMediator.SendMessageAsync(messageText);

      // Assert
      Assert.IsNotNull(actionResult.OperationException);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("Hello World", DisplayName = "Any_MessageText")]
    public async Task SendMessageAsync_SendMessageFailure_ReturnsFailure(string messageText)
    {
      // Arrange
      _queueAdapter.Setup(queueAdapter => queueAdapter.SendMessageAsync(messageText, null, null, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(_sendResponse.Object);
      _sendResponse.Setup(response => response.GetRawResponse().Status)
                   .Returns(400);

      // Act
      var actionResult = await _queueMediator.SendMessageAsync(messageText);

      // Assert
      Assert.IsNull(actionResult.OperationException);
      Assert.IsNotNull(actionResult.OperationMessage);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("Hello World", DisplayName = "Any_MessageText")]
    public async Task SendMessageAsync_SendMessageSuccess_ReturnsSuccess(string messageText)
    {
      // Arrange
      _queueAdapter.Setup(queueAdapter => queueAdapter.SendMessageAsync(messageText, null, null, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(_sendResponse.Object);
      _sendResponse.Setup(response => response.GetRawResponse().Status)
                   .Returns(201);

      // Act
      var actionResult = await _queueMediator.SendMessageAsync(messageText);

      // Assert
      Assert.IsNull(actionResult.OperationException);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
    }
    #endregion

    #region SendMessageAsync BinaryData
    [TestMethod]
    public async Task SendMessageAsyncBinaryData_MessageNullOrEmpty_ThrowsArgumentNullException()
    {
      // Arrange
      BinaryData message = null;

      // Act
      // Assert
      await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _queueMediator.SendMessageAsync(message));
    }

    [TestMethod]
    [DataRow("Hello World", DisplayName = "Any_MessageText")]
    public async Task SendMessageAsyncBinaryData_SendMessageException_ReturnsException(string messageText)
    {
      // Arrange
      BinaryData message = new BinaryData(messageText);

      _queueAdapter.Setup(queueAdapter => queueAdapter.SendMessageAsync(message, null, null, It.IsAny<CancellationToken>()))
                   .Throws(new Exception());

      // Act
      var actionResult = await _queueMediator.SendMessageAsync(message);

      // Assert
      Assert.IsNotNull(actionResult.OperationException);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("Hello World", DisplayName = "Any_MessageText")]
    public async Task SendMessageAsyncBinaryData_SendMessageFailure_ReturnsFailure(string messageText)
    {
      // Arrange
      BinaryData message = new BinaryData(messageText);

      _queueAdapter.Setup(queueAdapter => queueAdapter.SendMessageAsync(message, null, null, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(_sendResponse.Object);
      _sendResponse.Setup(response => response.GetRawResponse().Status)
                   .Returns(400);

      // Act
      var actionResult = await _queueMediator.SendMessageAsync(message);

      // Assert
      Assert.IsNull(actionResult.OperationException);
      Assert.IsNotNull(actionResult.OperationMessage);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("Hello World", DisplayName = "Any_MessageText")]
    public async Task SendMessageAsyncBinaryData_SendMessageSuccess_ReturnsSuccess(string messageText)
    {
      // Arrange
      BinaryData message = new BinaryData(messageText);

      _queueAdapter.Setup(queueAdapter => queueAdapter.SendMessageAsync(message, null, null, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(_sendResponse.Object);
      _sendResponse.Setup(response => response.GetRawResponse().Status)
                   .Returns(200);
      
      // Act
      var actionResult = await _queueMediator.SendMessageAsync(message);

      // Assert
      Assert.IsNull(actionResult.OperationException);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
    }
    #endregion

    #region UpdateMessageAsync
    [TestMethod]
    [DataRow(null, "123", DisplayName = "Null_MessageId")]
    public async Task UpdateMessageAsync_MessageIdNullOrEmpty_ThrowsArgumentNullException(string messageId, string popReceipt)
    {
      // Arrange
      // Act
      // Assert
      await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _queueMediator.UpdateMessageAsync(messageId, popReceipt));
    }

    [TestMethod]
    [DataRow("123", null, DisplayName = "Null_PopReceipt")]
    public async Task UpdateMessageAsync_PopReceiptNullOrEmpty_ThrowsArgumentNullException(string messageId, string popReceipt)
    {
      // Arrange
      // Act
      // Assert
      await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _queueMediator.UpdateMessageAsync(messageId, popReceipt));
    }

    [TestMethod]
    [DataRow("123", "123", DisplayName = "Any_MessageId_PopReceipt")]
    public async Task UpdateMessageAsync_UpdateMessageException_ReturnsException(string messageId, string popReceipt)
    {
      // Arrange
      string messageText = "Hello World";

      _queueAdapter.Setup(queueAdapter => queueAdapter.UpdateMessageAsync(messageId, popReceipt, messageText, It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()))
                   .Throws(new Exception());

      // Act
      var actionResult = await _queueMediator.UpdateMessageAsync(messageId, popReceipt, messageText);

      // Assert
      Assert.IsNotNull(actionResult.OperationException);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("123", "123", DisplayName = "Any_MessageId_PopReceipt")]
    public async Task UpdateMessageAsync_UpdateMessageFailure_ReturnsFailure(string messageId, string popReceipt)
    {
      // Arrange
      string messageText = "Hello World";

      _queueAdapter.Setup(queueAdapter => queueAdapter.UpdateMessageAsync(messageId, popReceipt, messageText, It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(_updateResponse.Object);
      _updateResponse.Setup(response => response.GetRawResponse().Status)
                   .Returns(400);

      // Act
      var actionResult = await _queueMediator.UpdateMessageAsync(messageId, popReceipt, messageText);

      // Assert
      Assert.IsNull(actionResult.OperationException);
      Assert.IsNotNull(actionResult.OperationMessage);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("123", "123", DisplayName = "Any_MessageId_PopReceipt")]
    public async Task UpdateMessageAsync_UpdateMessageSuccess_ReturnsSuccess(string messageId, string popReceipt)
    {
      // Arrange
      string messageText = "Hello World";

      _queueAdapter.Setup(queueAdapter => queueAdapter.UpdateMessageAsync(messageId, popReceipt, messageText, It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(_updateResponse.Object);
      _updateResponse.Setup(response => response.GetRawResponse().Status)
                   .Returns(200);

      // Act
      var actionResult = await _queueMediator.UpdateMessageAsync(messageId, popReceipt, messageText);

      // Assert
      Assert.IsNull(actionResult.OperationException);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
    }
    #endregion

    #region UpdateMessageAsync BinaryData
    [TestMethod]
    [DataRow(null, "123", DisplayName = "Null_MessageId")]
    public async Task UpdateMessageAsyncBinaryData_MessageIdNullOrEmpty_ThrowsArgumentNullException(string messageId, string popReceipt)
    {
      // Arrange
      BinaryData message = new BinaryData("Hello World");

      // Act
      // Assert
      await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _queueMediator.UpdateMessageAsync(messageId, popReceipt, message));
    }

    [TestMethod]
    [DataRow("123", null, DisplayName = "Null_PopReceipt")]
    public async Task UpdateMessageAsyncBinaryData_PopReceiptNullOrEmpty_ThrowsArgumentNullException(string messageId, string popReceipt)
    {
      // Arrange
      BinaryData message = new BinaryData("Hello World");

      // Act
      // Assert
      await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _queueMediator.UpdateMessageAsync(messageId, popReceipt, message));
    }

    [TestMethod]
    [DataRow("123", "123", DisplayName = "Any_MessageId_PopReceipt")]
    public async Task UpdateMessageAsyncBinaryData_MessageNullOrEmpty_ThrowsArgumentNullException(string messageId, string popReceipt)
    {
      // Arrange
      BinaryData message = null;

      // Act
      // Assert
      await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _queueMediator.UpdateMessageAsync(messageId, popReceipt, message));
    }

    [TestMethod]
    [DataRow("123", "123", DisplayName = "Any_MessageId_PopReceipt")]
    public async Task UpdateMessageAsyncBinaryData_UpdateMessageException_ReturnsException(string messageId, string popReceipt)
    {
      // Arrange
      BinaryData message = new BinaryData("Hello World");

      _queueAdapter.Setup(queueAdapter => queueAdapter.UpdateMessageAsync(messageId, popReceipt, message, It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()))
                   .Throws(new Exception());

      // Act
      var actionResult = await _queueMediator.UpdateMessageAsync(messageId, popReceipt, message);

      // Assert
      Assert.IsNotNull(actionResult.OperationException);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("123", "123", DisplayName = "Any_MessageId_PopReceipt")]
    public async Task UpdateMessageAsyncBinaryData_UpdateMessageFailure_ReturnsFailure(string messageId, string popReceipt)
    {
      // Arrange
      BinaryData message = new BinaryData("Hello World");

      _queueAdapter.Setup(queueAdapter => queueAdapter.UpdateMessageAsync(messageId, popReceipt, message, It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(_updateResponse.Object);
      _updateResponse.Setup(response => response.GetRawResponse().Status)
                   .Returns(400);

      // Act
      var actionResult = await _queueMediator.UpdateMessageAsync(messageId, popReceipt, message);

      // Assert
      Assert.IsNull(actionResult.OperationException);
      Assert.IsNotNull(actionResult.OperationMessage);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("123", "123", DisplayName = "Any_MessageId_PopReceipt")]
    public async Task UpdateMessageAsyncBinaryData_UpdateMessageSuccess_ReturnsSuccess(string messageId, string popReceipt)
    {
      // Arrange
      BinaryData message = new BinaryData("Hello World");

      _queueAdapter.Setup(queueAdapter => queueAdapter.UpdateMessageAsync(messageId, popReceipt, message, It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(_updateResponse.Object);
      _updateResponse.Setup(response => response.GetRawResponse().Status)
                   .Returns(200);

      // Act
      var actionResult = await _queueMediator.UpdateMessageAsync(messageId, popReceipt, message);

      // Assert
      Assert.IsNull(actionResult.OperationException);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
    }
    #endregion

    #region TestCleanup
    [TestCleanup]
    public void TestCleanup()
    {
      // Clean-up mock objects
      _queueAdapter.Reset();

      // Clean-up data objects
    }
    #endregion
  }
}

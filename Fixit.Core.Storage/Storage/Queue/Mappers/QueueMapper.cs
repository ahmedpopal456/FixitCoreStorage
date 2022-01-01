using AutoMapper;
using Azure.Storage.Queues.Models;
using Fixit.Core.Storage.DataContracts.Queue;

namespace Fixit.Core.Storage.Storage.Queue.Mappers
{
  public class QueueMapper : Profile
  {
    public QueueMapper()
    {
      CreateMap<QueueMessage, QueueMessageDto>()
        .ForMember(messageDto => messageDto.Body, opts => opts.MapFrom(queueMessage => queueMessage.Body))
        .ForMember(messageDto => messageDto.DequeueCount, opts => opts.MapFrom(queueMessage => queueMessage.DequeueCount))
        .ForMember(messageDto => messageDto.ExpiresOnUtc, opts => opts.MapFrom(queueMessage => queueMessage.ExpiresOn))
        .ForMember(messageDto => messageDto.InsertedOnUtc, opts => opts.MapFrom(queueMessage => queueMessage.InsertedOn))
        .ForMember(messageDto => messageDto.MessageId, opts => opts.MapFrom(queueMessage => queueMessage.MessageId))
        .ForMember(messageDto => messageDto.NextVisibleOnUtc, opts => opts.MapFrom(queueMessage => queueMessage.NextVisibleOn))
        .ForMember(messageDto => messageDto.PopReceipt, opts => opts.MapFrom(queueMessage => queueMessage.PopReceipt))
        .ReverseMap();
    }
  }
}

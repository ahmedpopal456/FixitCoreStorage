using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Azure.Storage.Queues.Models;
using Fixit.Core.Storage.DataContracts.Queue;

namespace Fixit.Core.Storage.Queue.Mappers
{
  public class QueueMapper : Profile
  {
    public QueueMapper()
    {
      CreateMap<PeekedMessage, MessageDto>()
        .ForMember(messageDto => messageDto.Body, opts => opts.MapFrom(peekedMessage => peekedMessage.Body))
        .ForMember(messageDto => messageDto.DequeueCount, opts => opts.MapFrom(peekedMessage => peekedMessage.DequeueCount))
        .ForMember(messageDto => messageDto.ExpiresOnUtc, opts => opts.MapFrom(peekedMessage => peekedMessage.ExpiresOn))
        .ForMember(messageDto => messageDto.InsertedOnUtc, opts => opts.MapFrom(peekedMessage => peekedMessage.InsertedOn))
        .ForMember(messageDto => messageDto.MessageId, opts => opts.MapFrom(peekedMessage => peekedMessage.MessageId))
        .ReverseMap();

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

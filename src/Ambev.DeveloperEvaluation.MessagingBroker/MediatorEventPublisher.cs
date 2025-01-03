﻿using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Services;
using MediatR;
using Newtonsoft.Json;

namespace Ambev.DeveloperEvaluation.MessagingBroker;

public class MediatorEventPublisher : IEventPublisher
{
    private readonly IMessageService _messageService;
    private readonly IMediator _mediator;
    
    private static readonly JsonSerializerSettings _serializerOptions = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        DefaultValueHandling = DefaultValueHandling.Ignore,
        MissingMemberHandling = MissingMemberHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };

    public MediatorEventPublisher(IMessageService messageService,
        IMediator mediator)
    {
        _messageService = messageService;
        _mediator = mediator;
    }

    public async Task PublishEventAsync(IEvent eventToPublish, CancellationToken cancellationToken = default)
    {
        //publish events for domain
        await _mediator.Publish(eventToPublish, cancellationToken);


        //publish events for external services
        await _messageService.SendMessageAsync(JsonConvert.SerializeObject(eventToPublish,_serializerOptions), cancellationToken);
    }
}
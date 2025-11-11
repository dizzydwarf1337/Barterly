using Application.Commands.Chat.AcceptPropose;
using Application.Commands.Chat.PayPropose;
using Application.Commands.Chat.ReadMessage;
using Application.Commands.Chat.RejectPropose;
using Application.Commands.Chat.SendMessage;
using Application.Commands.Chat.SendPropose;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Application.Hub;

public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public ChatHub(IMediator mediator, ILogger<ChatHub> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    public override Task OnConnectedAsync()
    {
        _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
    
    public async Task SendMessage(HubDto.CommonMessage message)
    {
        var savedMessage = await _mediator.Send(new SendMessageCommand
        {
            Message = message
        });
        
        var messageDto = new HubDto.CommonMessage(
            SenderId: savedMessage.SenderId,
            ReceiverId: savedMessage.ReceiverId,
            Content: savedMessage.Content,
            ChatId: savedMessage.ChatId,
            PostId: savedMessage.PostId,
            MessageId: savedMessage.Id
        );
        
        await Clients.Users(savedMessage.ReceiverId.ToString(), savedMessage.SenderId.ToString())
            .SendAsync("ReceiveMessage", messageDto);
    }

    public async Task SendPropose(HubDto.ProposalMessage message)
    {
        var savedMessage = await _mediator.Send(new SendProposeCommand
        {
            Message = message
        });
        
        var proposalDto = new HubDto.ProposalMessage(
            SenderId: savedMessage.SenderId,
            ReceiverId: savedMessage.ReceiverId,
            Content: savedMessage.Content,
            Price: savedMessage.Price ?? 0,
            PostId: savedMessage.PostId,
            ChatId: savedMessage.ChatId,
            MessageId: savedMessage.Id
        );
        
        await Clients.Users(savedMessage.ReceiverId.ToString(), savedMessage.SenderId.ToString())
            .SendAsync("ReceivePropose", proposalDto);
    }

    public async Task AcceptPropose(HubDto.AcceptProposal accept)
    {
        await _mediator.Send(new AcceptProposeCommand
        {
            AcceptPropose = accept
        });
        await Clients.Users(accept.ReceiverId.ToString(), accept.SenderId.ToString())
            .SendAsync("ProposeAccepted", accept);
    }

    public async Task RejectPropose(HubDto.RejectProposal reject)
    {
        await _mediator.Send(new RejectProposeCommand
        {
            Reject = reject
        });
        await Clients.Users(reject.ReceiverId.ToString(), reject.SenderId.ToString())
            .SendAsync("ProposeRejected", reject);
    }
    
    public async Task PayPropose(HubDto.PayProposal payment)
    {
        await _mediator.Send(new PayProposeCommand
        {
            MessageId = payment.MessageId
        });
        
        await Clients.Group(payment.ChatId.ToString())
            .SendAsync("ProposePaid", new { messageId = payment.MessageId, chatId = payment.ChatId });
    }

    public async Task ReadMessage(HubDto.ReadMessage message)
    {
        await _mediator.Send(new ReadMessageCommand()
        {
            ReadMessage = message
        });
        await Clients.User(message.SenderId.ToString())
            .SendAsync("ReadMessage", message);
    }
}
using Application.Commands.Chat.AcceptPropose;
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
        await _mediator.Send(new SendMessageCommand
        {
            Message = message
        });
        await Clients.Users(message.ReceiverId.ToString(), message.SenderId.ToString()).SendAsync("ReceiveMessage", message);
    }

    public async Task SendPropose(HubDto.ProposalMessage message)
    {
            await _mediator.Send(new SendProposeCommand
            {
                Message = message
            });
            await Clients.User(message.ReceiverId.ToString()).SendAsync("ReceivePropose", message);
    }

    public async Task AcceptPropose(HubDto.AcceptProposal accept)
    {
        await _mediator.Send(new AcceptProposeCommand
        {
            AcceptPropose = accept
        });
        await Clients.User(accept.ReceiverId.ToString()).SendAsync("ProposeAccepted", accept);
    }

    public async Task RejectPropose(HubDto.RejectProposal reject)
    {
        await _mediator.Send(new RejectProposeCommand
        {
            Reject = reject
        });
        await Clients.User(reject.ReceiverId.ToString()).SendAsync("ProposeRejected", reject);
    }

    public async Task ReadMessage(HubDto.ReadMessage message)
    {
        await _mediator.Send(new ReadMessageCommand()
        {
            ReadMessage = message
        });
        await  Clients.User(message.SenderId.ToString()).SendAsync("ReceiveMessage", message);
    }
}
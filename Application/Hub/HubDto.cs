namespace Application.Hub;

public class HubDto
{
    public record CommonMessage(
        Guid SenderId, 
        Guid ReceiverId, 
        string Content, 
        Guid? ChatId, 
        Guid? PostId = null,
        Guid? MessageId = null); 
    
    public record ProposalMessage(
        Guid SenderId, 
        Guid ReceiverId, 
        string Content, 
        decimal Price, 
        Guid? PostId, 
        Guid? ChatId,
        Guid? MessageId = null); 

    public record AcceptProposal(Guid MessageId, Guid SenderId, Guid ReceiverId, Guid? ChatId);

    public record RejectProposal(Guid MessageId, Guid SenderId, Guid ReceiverId, Guid? ChatId);

    public record ReadMessage(Guid MessageId, Guid ReadBy, Guid SenderId, Guid? ChatId);
}
namespace Application.Core.Validators.Interfaces;

public interface IMessageProfanityValidator
{
    public string Censore(string message);
}
namespace Application.Core.HangfireJobs.PromotionCancelling;

public interface IPromotionCancellingJob
{
    Task Execute();
}
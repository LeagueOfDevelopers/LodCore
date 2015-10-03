namespace NotificationService
{
    public interface IDistributionPolicy
    {
        int[] ReceiversIds { get; } 
    }
}
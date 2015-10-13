namespace NotificationService
{
    public interface IDistributionPolicyFactory
    {
        DistributionPolicy GetAllPolicy();

        DistributionPolicy GetProjectRelatedPolicy(int projectId);

        DistributionPolicy GetAdminRelatedPolicy(int projectId);

        DistributionPolicy GetUserSpecifiedPolicy(params int[] userIds);
    }
}
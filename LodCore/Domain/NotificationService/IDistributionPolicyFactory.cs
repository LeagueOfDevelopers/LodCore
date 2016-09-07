namespace NotificationService
{
    public interface IDistributionPolicyFactory
    {
        DistributionPolicy GetAllPolicy();

        DistributionPolicy GetVerificatedDevelopersPolicy();

        DistributionPolicy GetProjectRelatedPolicy(int projectId);

        DistributionPolicy GetAdminRelatedPolicy();

        DistributionPolicy GetUserSpecifiedPolicy(params int[] userIds);
    }
}
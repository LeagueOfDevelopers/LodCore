﻿namespace LodCoreLibraryOld.Domain.NotificationService
{
    public interface IDistributionPolicyFactory
    {
        DistributionPolicy GetAllPolicy();

        DistributionPolicy GetProjectRelatedPolicy(int projectId);

        DistributionPolicy GetAdminRelatedPolicy();

        DistributionPolicy GetUserSpecifiedPolicy(params int[] userIds);
    }
}
namespace ServiceRequestManagement.Domain.ServiceRequestAggregate
{
    /// <summary>
    /// Enum for the current status of the ServiceRequest entity.
    /// </summary>
    public enum CurrentStatus
    {
        NotApplicable,
        Created,
        InProgress,
        Complete,
        Canceled
    }
}

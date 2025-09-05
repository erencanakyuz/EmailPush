namespace EmailPush.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public class CampaignNotFoundException : DomainException
{
    public CampaignNotFoundException(Guid id) : base($"Campaign with id {id} was not found")
    {
    }
}

public class CampaignAlreadyStartedException : DomainException
{
    public CampaignAlreadyStartedException(Guid id) : base($"Campaign with id {id} has already been started")
    {
    }
}

public class InvalidCampaignStatusException : DomainException
{
    public InvalidCampaignStatusException(string operation, string currentStatus) 
        : base($"Cannot perform operation '{operation}' on campaign with status '{currentStatus}'")
    {
    }
}
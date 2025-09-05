using EmailPush.Domain.Exceptions;

namespace EmailPush.Domain.ValueObjects;

public sealed class RecipientList : IEquatable<RecipientList>
{
    private readonly HashSet<Email> _recipients;

    public IReadOnlyCollection<Email> Recipients => _recipients.ToList().AsReadOnly();
    public int Count => _recipients.Count;

    private RecipientList(IEnumerable<Email> recipients)
    {
        _recipients = recipients.ToHashSet();
    }

    public static RecipientList Create(IEnumerable<string> emailAddresses)
    {
        if (emailAddresses == null)
        {
            throw new DomainException("Recipients cannot be null");
        }

        var emails = emailAddresses.Select(Email.Create).ToList();

        if (emails.Count == 0)
        {
            throw new DomainException("At least one recipient is required");
        }

        if (emails.Count > 10000) // Business rule: max 10,000 recipients per campaign
        {
            throw new DomainException("Cannot have more than 10,000 recipients per campaign");
        }

        return new RecipientList(emails);
    }

    public static RecipientList Create(params string[] emailAddresses)
    {
        return Create(emailAddresses.AsEnumerable());
    }

    public RecipientList Add(string emailAddress)
    {
        var email = Email.Create(emailAddress);
        var newRecipients = _recipients.ToList();
        newRecipients.Add(email);
        
        return new RecipientList(newRecipients);
    }

    public RecipientList Remove(string emailAddress)
    {
        var email = Email.Create(emailAddress);
        var newRecipients = _recipients.Where(r => r != email).ToList();
        
        if (newRecipients.Count == 0)
        {
            throw new DomainException("Cannot remove all recipients from campaign");
        }
        
        return new RecipientList(newRecipients);
    }

    public bool Contains(string emailAddress)
    {
        var email = Email.Create(emailAddress);
        return _recipients.Contains(email);
    }

    public IEnumerable<string> ToStringList()
    {
        return _recipients.Select(r => r.Value);
    }

    public bool Equals(RecipientList? other)
    {
        return other is not null && _recipients.SetEquals(other._recipients);
    }

    public override bool Equals(object? obj)
    {
        return obj is RecipientList other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _recipients.Aggregate(0, (hash, email) => hash ^ email.GetHashCode());
    }

    public static bool operator ==(RecipientList? left, RecipientList? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(RecipientList? left, RecipientList? right)
    {
        return !Equals(left, right);
    }
}
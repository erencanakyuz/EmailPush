using EmailPush.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace EmailPush.Domain.ValueObjects;

public sealed class Email : IEquatable<Email>
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("Email cannot be empty");
        }

        if (!EmailRegex.IsMatch(email))
        {
            throw new DomainException($"Invalid email format: {email}");
        }

        return new Email(email.ToLowerInvariant());
    }

    public static implicit operator string(Email email) => email.Value;
    
    public static explicit operator Email(string email) => Create(email);

    public bool Equals(Email? other)
    {
        return other is not null && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Email other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString() => Value;

    public static bool operator ==(Email? left, Email? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Email? left, Email? right)
    {
        return !Equals(left, right);
    }
}
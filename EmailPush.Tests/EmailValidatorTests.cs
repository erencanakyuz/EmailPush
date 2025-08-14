using EmailPush.Application.Utils;
using NUnit.Framework;

namespace EmailPush.Tests;

[TestFixture]
public class EmailValidatorTests
{
    [TestCase("test@example.com", true, "Valid email")]
    [TestCase("invalid-email", false, "No @ symbol")]
    [TestCase("", false, "Empty string")]
    [TestCase(null, false, "Null value")]
    public void IsValidEmail_ReturnsCorrectResult(string email, bool expectedResult, string description)
    {
        // Act
        var result = EmailValidator.IsValidEmail(email);

        // Assert
        Assert.That(result, Is.EqualTo(expectedResult), description);
    }

}
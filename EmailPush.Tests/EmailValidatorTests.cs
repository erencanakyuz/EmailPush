using EmailPush.Application.Utils;
using NUnit.Framework;

namespace EmailPush.Tests;

[TestFixture]
public class EmailValidatorTests
{
    [TestCase("test@example.com", true, "Geçerli standart email")]
    [TestCase("user@mail.example.com", true, "Geçerli subdomain'li email")]
    [TestCase("test123@example123.com", true, "Geçerli sayı içeren email")]
    [TestCase("user.name@domain.com", true, "Geçerli nokta içeren email")]
    [TestCase("invalid-email", false, "Eksik @ sembolü")]
    [TestCase("test@", false, "Eksik domain")]
    [TestCase("@example.com", false, "Eksik kullanıcı adı")]
    [TestCase("", false, "Boş string")]
    [TestCase("   ", false, "Sadece boşluk")]
    [TestCase(null, false, "Null değer")]
    [TestCase("test@@example.com", false, "Çoklu @ sembolü")]
    [TestCase("test.example.com", false, "@ sembolü tamamen yok")]
    public void IsValidEmail_ShouldReturnExpectedResult_ForGivenInput(string email, bool expectedResult, string description)
    {
        // Act
        var result = EmailValidator.IsValidEmail(email);

        // Assert
        Assert.That(result, Is.EqualTo(expectedResult), $"Test açıklaması: {description}");
    }

    [Test]
    public void GetInvalidEmails_WithMixedEmails_ReturnsOnlyInvalidOnes()
    {
        // Arrange
        var emails = new List<string>
        {
            "valid@example.com",
            "invalid-email",
            "another.valid@domain.com",
            "test@",
            "@domain.com"
        };

        // Act
        var invalidEmails = EmailValidator.GetInvalidEmails(emails);

        // Assert
        Assert.AreEqual(3, invalidEmails.Count);
        Assert.Contains("invalid-email", invalidEmails);
        Assert.Contains("test@", invalidEmails);
        Assert.Contains("@domain.com", invalidEmails);
    }

    [Test]
    public void GetInvalidEmails_WithAllValidEmails_ReturnsEmptyList()
    {
        // Arrange
        var emails = new List<string>
        {
            "user1@example.com",
            "user2@example.com",
            "user3@domain.com"
        };

        // Act
        var invalidEmails = EmailValidator.GetInvalidEmails(emails);

        // Assert
        Assert.IsEmpty(invalidEmails);
    }

    [Test]
    public void AreAllEmailsValid_WithAllValidEmails_ReturnsTrue()
    {
        // Arrange
        var emails = new List<string>
        {
            "user1@example.com",
            "user2@example.com",
            "user3@domain.com"
        };

        // Act
        var result = EmailValidator.AreAllEmailsValid(emails);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void AreAllEmailsValid_WithSomeInvalidEmails_ReturnsFalse()
    {
        // Arrange
        var emails = new List<string>
        {
            "valid@example.com",
            "invalid-email",
            "another@domain.com"
        };

        // Act
        var result = EmailValidator.AreAllEmailsValid(emails);

        // Assert
        Assert.IsFalse(result);
    }
}
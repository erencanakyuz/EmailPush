using EmailPush.Application.Utils;
using NUnit.Framework;

namespace EmailPush.Tests;

[TestFixture]
public class CampaignServiceTests
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
        var result = EmailValidator.IsValidEmail(email!);

        // Assert
        Assert.That(result, Is.EqualTo(expectedResult), $"Test açıklaması: {description}");
    }
}
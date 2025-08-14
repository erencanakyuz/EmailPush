# Unit Test Kullanım Kılavuzu

## Test'leri Çalıştırma

Visual Studio'da:
```
1. Solution Explorer'da EmailPush.Tests projesine sağ tıkla
2. "Run Tests" seç
3. Test Explorer açılacak ve sonuçları gösterecek
```

Command Line'da:
```bash
# Proje klasöründe
dotnet test

# Veya sadece test projesi için
dotnet test EmailPush.Tests/EmailPush.Tests.csproj

# Verbose output ile
dotnet test --logger "console;verbosity=detailed"
```

## Test Coverage

### ✅ Yazılan Test'ler:

**CampaignServiceTests.cs** - `IsValidEmail` method testing:
- ✅ Valid email formats  
- ✅ Invalid email formats
- ✅ Edge cases (null, empty, whitespace)
- ✅ Special cases (multiple @, no domain)

**CampaignEntityTests.cs** - `Campaign` entity testing:
- ✅ TotalRecipients property calculation
- ✅ Default values validation
- ✅ Property setting validation

**MapToDtoTests.cs** - `MapToDto` method testing:
- ✅ Complete property mapping
- ✅ Status enum to string conversion
- ✅ Null handling (StartedAt)
- ✅ Collection handling (Recipients)

### 📊 Test Statistics:
- **Total Tests**: ~25 test methods
- **Coverage**: Basic utility methods and entity properties
- **Framework**: NUnit with .NET 8

## Mentora Gösterebileceğin:

1. **Test Organization**: Logical test classes ve descriptive test names
2. **AAA Pattern**: Arrange-Act-Assert pattern consistently used
3. **Edge Cases**: Null, empty, invalid inputs tested
4. **Business Logic**: Email validation ve DTO mapping tested

## İlerde Eklenebilecek Test'ler:

- Repository integration tests (with in-memory database)
- Controller integration tests  
- Service layer business logic tests
- Validation tests for DTOs
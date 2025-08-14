# EmailPush - Email Campaign Management System

A simple email campaign management API built with ASP.NET Core and Clean Architecture principles.

## 🚀 Features

- **Campaign CRUD Operations**: Create, read, update, and delete email campaigns
- **Email Sending**: Start email campaigns with background processing
- **Statistics**: Track campaign performance and email delivery stats
- **Clean Architecture**: Organized in Domain, Application, Infrastructure layers
- **Background Processing**: Worker service for email simulation
- **API Documentation**: Comprehensive Swagger documentation

## 🏗️ Architecture

```
EmailPush/                 # Web API Layer
EmailPush.Application/     # Application Services & DTOs
EmailPush.Domain/          # Domain Entities & Interfaces  
EmailPush.Infrastructure/  # Data Access & Repositories
EmailPush.Worker/          # Background Service
```
## 🔧 Technologies

- **ASP.NET Core 8.0** - Web API Framework
- **Entity Framework Core** - Data Access with SQLite
- **MassTransit** - Message Queue (Optional)
- **Clean Architecture** - Domain-Driven Design
- **Generic Repository Pattern** - Data Access Pattern
- **Dependency Injection** - IoC Container
- **Swagger** - API Documentation

Projede Uygulanan Temel Yazılım geliştirme Kavramları ve Prensipleri

1-Clean Architecture 

 (Domain, Application, Infrastructure, API): Her katmanın belirli bir sorumluluğu var ve diğer katmanlara olan bağımlılığı minimize ediliyor.

Bağımlılıkların yönü: Bağımlılıklar daima içe doğru, yani Domain katmanına doğru. Dış katmanlar iç katmanları tanır, ancak tam tersi olmaz.
+-------------------------------------------------------------+
|                                                             |
|   +-----------+      +------------+      +----------------+ |
|   |    API    |----->| Application|----->|     Domain     | |
|   +-----------+      +------------+      +----------------+ |
|                            ^                      ^         |
|                            |                      |         |
|   +-----------+      +----------------+           |         |
|   |  Worker   |----->| Infrastructure |-----------+         |
|   +-----------+      +----------------+                     |
|                                                             |
+-------------------------------------------------------------+


2-Domain-Driven Design (DDD)

Domain katmanı: İş kurallarını ve varlıklarını (entity) temsil eden sınıflar burada yer alır.
Ubiquitous Language (Her Yerde Aynı Dil): Projede kullanılan terimlerin (örneğin "Campaign", "Draft", "Ready") iş alanıyla uyumlu olması.
Kullanım Alanları: Özellikle Domain katmanında, iş kurallarının ve varlıkların modellenmesinde.

EmailPush (API) ve EmailPush.Worker → Presentation Layer
EmailPush.Application → Application Layer
EmailPush.Domain → Domain Layer
EmailPush.Infrastructure → Infrastructure Layer

Ortak Dil: Kodda kullandığım Campaign, Status (Draft, Ready vb.) gibi isimlerin, işi tarif eden gerçek dünyadaki kavramlarla birebir aynı olmasına özen gösterdim. Bu, kodun amacını daha anlaşılır kılıyor.
Katmanlı Yapı: DDD'nin önerdiği gibi, işin kalbi olan Domain katmanını (EmailPush.Domain) tüm teknoloji detaylarından izole ettim. İş akışlarını Application katmanında (CampaignService) yönettin.
Model: Projenin ana nesnesi olan Campaign'i, kendine ait bir kimliği olan bir Entity olarak modelledim.
Soyutlama: Veritabanı erişimini ise Repository Pattern kullanarak tamamen soyutladım. Bu sayede CampaignService gibi iş mantığı sınıflarım, veritabanının ne olduğundan habersiz bir şekilde çalışabiliyor."

3- Dependency Injection

Program.cs'deki builder.Services.AddScoped<...> satırları: Hangi interface'in hangi somut sınıfı kullanacağını tanımlar.
Controller'ların constructor'ları: Bağımlılıklar (örneğin ICampaignService) constructor aracılığıyla enjekte edilir.
Kullanım Alanları: Tüm projede, sınıfların birbirine olan bağımlılığını yönetmek için.


4- Test-Driven Development (TDD)

Unit testing
Üç aşamalı test yapısı (Arrange, Act, Assert)

5- Dikkat edilen SOLID Presinbleri:

-Single Responsibility Principle (SRP) (Tek Sorumluluk Prensibi): Her sınıfın (veya metodun) tek bir sorumluluğu olması. (Örn: EmailValidator, CampaignMapper)

-Open/Closed Principle (OCP) (Açık/Kapalı Prensibi): Bir sınıfın genişlemeye açık, ancak değiştirmeye kapalı olması. (Mevcut kod değişmeden yeni özellikler eklenebilmesi)

-Liskov Substitution Principle (LSP) (Liskov Yerine Geçme Prensibi): Bir sınıfın alt sınıfları, ana sınıfın yerine geçebilmeli ve aynı davranışı sergilemeli.

-Interface Segregation Principle (ISP) (Arayüz Ayrımı Prensibi): Bir sınıf, kullanmadığı arayüzleri uygulamaya zorlanmamalı. (Daha küçük ve odaklı arayüzler kullanmak)

-Dependency Inversion Principle (DIP) (Bağımlılık Ters Çevirme Prensibi): Yüksek seviyeli modüller (örneğin API), düşük seviyeli modüllere (örneğin veritabanı) doğrudan bağımlı olmamalı. İkisi de soyutlamalara (arayüzlere) bağımlı olmalı. (DI prensibiyle yakından ilişkili)

Notlar:
"Clean Architecture principles'ı uyguladım ama tamamen değil. Domain core'u saf tutdum, dependency 
  direction doğru, ama Program.cs'de DI setup ve Worker'da background processing için pragmatic approach aldım.
   Microsoft'un best practices'ine uygun."
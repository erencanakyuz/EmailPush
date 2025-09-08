# EmailPush2 - Email Campaign Management System

A clean email campaign management API built with ASP.NET Core following Clean Architecture and Domain-Driven Design principles.

## 🚀 Features

- **Campaign CRUD Operations**: Create, read, update (PUT/PATCH), and delete email campaigns
- **Status Filtering**: Filter campaigns by status (Draft, Ready, Sending, Completed, Failed)
- **Email Validation**: Automatic email address validation
- **Email Sending**: Start email campaigns with background processing simulation
- **Statistics**: Track campaign performance and email delivery stats
- **Clean Architecture**: Well-organized layers with proper dependency direction
- **RESTful API**: Full REST compliance with proper HTTP verbs and status codes
- **API Documentation**: Comprehensive Swagger documentation

## Architecture

The project follows Clean Architecture principles with a clear separation of concerns:

- **API Layer**: REST controllers that handle HTTP requests
- **Application Layer**: MediatR handlers that implement business logic
- **Domain Layer**: Core entities, interfaces, and domain rules
- **Infrastructure Layer**: Data access implementation and external services

### Key Patterns

- **CQRS**: Command and Query separation using MediatR
- **Repository Pattern**: Abstracted data access
- **Dependency Injection**: Inversion of control using ASP.NET Core's built-in container
- **Middleware**: Global error handling and request logging

```
EmailPush.Api/             # Web API Layer (Presentation)
EmailPush.Application/     # Application Services & DTOs (Business Logic)
EmailPush.Domain/          # Domain Entities & Interfaces (Core)
EmailPush.Infrastructure/  # Data Access & Repositories (Infrastructure)
EmailPush.Worker/          # Background Service (Infrastructure)
```
## 🔧 Technologies

- **ASP.NET Core 8.0** - Web API Framework
- **Entity Framework Core** - Data Access with SQLite
- **MassTransit** - Message Queue (Optional)
- **Clean Architecture** - Domain-Driven Design
- **Generic Repository Pattern** - Data Access Pattern
- **Dependency Injection** - IoC Container
- **Swagger** - API Documentation


## 🏛️ Software Development Principles Applied

### 1. Clean Architecture

**Layer Separation**: Each layer has specific responsibilities with minimized dependencies between layers.

**Dependency Direction**: Dependencies always point inward toward the Domain layer. Outer layers know about inner layers, but not vice versa.

```
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
```

**Layer Responsibilities**:
- **EmailPush.Api** → Presentation Layer (HTTP concerns)
- **EmailPush.Application** → Application Layer (Business logic orchestration)
- **EmailPush.Domain** → Domain Layer (Core business entities and rules)
- **EmailPush.Infrastructure** → Infrastructure Layer (Data access, external services)

### 3. Dependency Injection

**IoC Container**: ASP.NET Core's built-in container manages object lifecycles and dependencies.

**Constructor Injection**: Dependencies (like IMediator) injected through constructors.

**Interface Abstractions**: Services depend on interfaces, not concrete implementations.

### 4. Test-Driven Development (TDD) // only for Learning intentions it wont use in project!1

**Unit Testing**: Comprehensive test coverage with NUnit.

**AAA Pattern**: Tests follow Arrange-Act-Assert structure for clarity.

### 5. SOLID Principles

**Single Responsibility Principle (SRP)**: Each class has one reason to change (EmailValidator, CampaignMapper).

**Open/Closed Principle (OCP)**: Classes open for extension, closed for modification.

**Liskov Substitution Principle (LSP)**: Derived classes replaceable with base classes.

**Interface Segregation Principle (ISP)**: Focused, smaller interfaces rather than large monolithic ones.

**Dependency Inversion Principle (DIP)**: High-level modules don't depend on low-level modules; both depend on abstractions.

### Architecture Notes

*"Applied Clean Architecture principles with pragmatic Microsoft best practices. Domain core kept pure, dependency direction maintained, with practical approach in Program.cs DI setup and Worker background processing."*



## 📋 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET    | `/api/campaigns` | Get all campaigns |
| GET    | `/api/campaigns?status={status}` | Filter campaigns by status (0=Draft, 1=Ready, 2=Sending, 3=Completed, 4=Failed) |
| GET    | `/api/campaigns/{id}` | Get campaign by ID |
| POST   | `/api/campaigns` | Create new campaign |
| PUT    | `/api/campaigns/{id}` | Update campaign (full replacement) |
| PATCH  | `/api/campaigns/{id}` | Partially update campaign |
| DELETE | `/api/campaigns/{id}` | Delete campaign (draft only) |
| POST   | `/api/campaigns/{id}/start` | Start sending emails |
| GET    | `/api/campaigns/stats` | Get statistics |


## 📝 Example Usage

### Create a Campaign
```json
POST /api/campaigns
{
  "name": "Welcome Campaign",
  "subject": "Welcome to our platform!",
  "content": "Hello! Welcome to our platform. This is a test email campaign.",
  "recipients": ["test@example.com", "user@example.com"]
}
```

### Update Campaign (Full Replacement - PUT)
```json
PUT /api/campaigns/{id}
{
  "name": "Updated Campaign Name",
  "subject": "Updated subject line",
  "content": "Updated email content",
  "recipients": ["new@example.com", "updated@example.com"]
}
```

### Partially Update Campaign (PATCH)
```json
PATCH /api/campaigns/{id}
{
  "name": "Only updating the name"
}
```

### Filter Campaigns by Status
```
GET /api/campaigns?status=0    # Draft campaigns
GET /api/campaigns?status=1    # Ready campaigns
```

### Start Email Sending
```json
POST /api/campaigns/{id}/start
```

## 🗄️ Database

- **Provider**: SQLite
- **File**: `EmailPush.db`
- **Auto-Migration**: Database created on first run

## 🔄 Background Processing

- **Simulation Mode**: Emails logged to console (default)
- **Queue Support**: MassTransit + RabbitMQ (optional)
- **Status Tracking**: Campaign status updates automatically

## 📊 Campaign Status Flow

```
Draft → Ready → Sending → Completed
              ↓
            Failed
```

## 📄 Pagination Support

**Before**: All campaigns loaded at once, causing performance issues with large datasets
**Now**: Efficient pagination with configurable page sizes and navigation

### Key Benefits:
- **Performance**: Only loads required records from database
- **Memory Efficiency**: Reduces server and client memory consumption  
- **Network Optimization**: Smaller payloads for faster API responses
- **Scalability**: Handles thousands of campaigns without degradation
- **User Experience**: Smooth navigation through large result sets

### Usage:
```
GET /api/campaigns?pageNumber=1&pageSize=10
GET /api/campaigns?status=0&pageNumber=2&pageSize=5
```

**Default Settings**: 10 items per page, maximum 100 items, starts at page 1

## 🛠️ Configuration

Key settings in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=EmailPush.db"
  }
}
```


**PHASE 2



- **AutoMapper** - Replace manual CampaignMapper 

- **Polly** - Microsoft's retry library

- **FluentValidation** - Replace custom  EmailValidator with ready  validation lib

- **Pagination** - Simple custom paggination added
- **Error Handling** -  middleware custom simple!

# 1. Rate Limiting Test (PowerShell)
1..15 | ForEach-Object { 
    Write-Host "İstek $_"; 
    Invoke-RestMethod -Uri "http://localhost:5129/api/campaigns" 
}

# 2. Exception Handling Test
Invoke-RestMethod -Uri "http://localhost:5129/api/nonexistent"

# 3. Basit test (tek istek)
Invoke-RestMethod -Uri "http://localhost:5129/api/campaigns"

4- monitoring

curl http://localhost:5129/api/campaigns
status 200 !! 
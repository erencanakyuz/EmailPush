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

## 🚦 Getting Started

### Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code

### Running the Application

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd EmailPush2
   ```

2. **Start the API**
   ```bash
   cd EmailPush
   dotnet run
   ```

3. **Start the Worker (Optional)**
   ```bash
   cd EmailPush.Worker
   dotnet run
   ```

4. **Access Swagger UI**
   ```
   https://localhost:7xxx/swagger
   ```

## 📋 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET    | `/api/campaigns` | Get all campaigns |
| GET    | `/api/campaigns/{id}` | Get campaign by ID |
| POST   | `/api/campaigns` | Create new campaign |
| PUT    | `/api/campaigns/{id}` | Update campaign |
| DELETE | `/api/campaigns/{id}` | Delete campaign |
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

## 🛠️ Configuration

Key settings in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=EmailPush.db"
  }
}
```

## 🧪 Testing

Use the included `test-api.http` file with VS Code REST Client or test via Swagger UI.

## 📈 Monitoring

- **Logging**: Built-in ASP.NET Core logging
- **Error Handling**: Global exception middleware
- **Health Checks**: API status monitoring

## 🎯 Project Goals (Phase 1)

✅ Campaign CRUD operations  
✅ Email sending simulation  
✅ Delivery statistics  
✅ Clean Architecture implementation  
✅ Generic Repository Pattern  
✅ Background Worker Service  
✅ Comprehensive API documentation  

---

*Built as a learning project demonstrating Clean Architecture, Entity Framework, and background processing concepts.*
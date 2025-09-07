# Pagination Implementation Test

This document demonstrates how the pagination implementation would work in the EmailPush project.

## API Usage Examples

### 1. Get All Campaigns with Pagination

```
GET /api/campaigns?pageNumber=1&pageSize=10
```

Response:
```json
{
  "data": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "name": "Welcome Campaign",
      "subject": "Welcome to our platform!",
      "content": "Hello! Welcome to our platform.",
      "recipients": ["test@example.com"],
      "status": "Draft",
      "createdAt": "2023-01-01T00:00:00Z",
      "startedAt": null,
      "totalRecipients": 1,
      "sentCount": 0
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalItems": 25,
  "totalPages": 3,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

### 2. Get Campaigns by Status with Pagination

```
GET /api/campaigns?status=0&pageNumber=2&pageSize=5
```

Response:
```json
{
  "data": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "name": "Welcome Campaign",
      "subject": "Welcome to our platform!",
      "content": "Hello! Welcome to our platform.",
      "recipients": ["test@example.com"],
      "status": "Draft",
      "createdAt": "2023-01-01T00:00:00Z",
      "startedAt": null,
      "totalRecipients": 1,
      "sentCount": 0
    }
  ],
  "pageNumber": 2,
  "pageSize": 5,
  "totalItems": 12,
  "totalPages": 3,
  "hasPreviousPage": true,
  "hasNextPage": true
}
```

## Implementation Details

### 1. Repository Layer Changes

The repository layer now supports pagination with two new methods:

```csharp
// In IGenericRepository<T>
Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);
Task<int> GetTotalCountAsync();

// In ICampaignRepository
Task<IEnumerable<Campaign>> GetPagedByStatusAsync(CampaignStatus status, int pageNumber, int pageSize);
```

### 2. Query Layer Changes

The query models now include pagination parameters:

```csharp
public class GetAllCampaignsQuery : IRequest<PagedResponseDto<CampaignDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetCampaignsByStatusQuery : IRequest<PagedResponseDto<CampaignDto>>
{
    public CampaignStatus Status { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
```

### 3. Controller Layer Changes

The controller now accepts pagination parameters:

```csharp
[HttpGet]
public async Task<ActionResult<PagedResponseDto<CampaignDto>>> GetCampaigns(
    [FromQuery] CampaignStatus? status = null,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
```

## Benefits of This Implementation

1. **Performance**: Only loads the required number of records from the database
2. **Memory Efficiency**: Reduces memory consumption on both server and client
3. **Network Efficiency**: Transfers smaller payloads over the network
4. **Scalability**: Handles large datasets without performance degradation
5. **User Experience**: Provides better navigation through large result sets

## Configuration

The pagination implementation includes sensible defaults:
- Default page size: 10 items
- Maximum page size: 100 items (to prevent excessive loads)
- Default page number: 1

These values can be adjusted based on the specific needs of the application.
using EmailPush.Domain.Entities;
using EmailPush.Domain.Interfaces;
using EmailPush.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using EmailPush.Domain.Enums;

namespace EmailPush.Infrastructure.Repositories;

public class CampaignRepository : GenericRepository<Campaign>, ICampaignRepository
{
    public CampaignRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Campaign>> GetActiveCampaignsAsync()
    {
        return await _context.Set<Campaign>()
            .Where(c => c.Status == CampaignStatus.Ready || c.Status == CampaignStatus.Sending)
            .ToListAsync();
    }

    public async Task<IEnumerable<Campaign>> GetByStatusAsync(CampaignStatus status)
    {
        return await _context.Set<Campaign>()
            .Where(c => c.Status == status)
            .ToListAsync();
    }

    public async Task<Campaign?> GetWithRecipientsAsync(Guid id)
    {
        return await _context.Set<Campaign>()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    // Pagination support
    public async Task<IEnumerable<Campaign>> GetPagedByStatusAsync(CampaignStatus status, int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;
        return await _context.Set<Campaign>()
            .Where(c => c.Status == status)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
    }
    
    public async Task<int> GetCountByStatusAsync(CampaignStatus status)
    {
        return await _context.Set<Campaign>()
            .CountAsync(c => c.Status == status);
    }
}
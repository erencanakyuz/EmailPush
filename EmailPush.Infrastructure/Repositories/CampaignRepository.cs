using EmailPush.Domain.Entities;
using EmailPush.Domain.Interfaces;
using EmailPush.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Campaign?> GetWithRecipientsAsync(Guid id)
    {
        return await _context.Set<Campaign>()
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
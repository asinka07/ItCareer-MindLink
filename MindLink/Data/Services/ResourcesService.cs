using Microsoft.EntityFrameworkCore;
using MindLink.Data.Models;
using static MudBlazor.CategoryTypes;

namespace MindLink.Data.Services
{
    public class ResourcesService
    {
        private readonly MindLinkDbContext _context;

        public ResourcesService(MindLinkDbContext context)
        {
            _context = context;
        }

        public async Task<List<Resource>> GetAllResources()
        {
            return await _context.Resources.OrderByDescending(r => r.CreatedOn)
                                         .ToListAsync();
        }

        public async Task CreateResource(Resource resource)
        {
            resource.CreatedOn = DateTime.Now;
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateResource(Resource item)
        {
            Resource resource = await _context.Resources.SingleAsync(x => x.Id == item.Id);
            resource.Title = item.Title;
            resource.Content = item.Content;
            resource.IsVisible = item.IsVisible;
            resource.Emotion = item.Emotion;
            await _context.SaveChangesAsync();
        }

        public async Task ChangeVisibility(Resource item)
        {
            Resource resource = await _context.Resources.SingleAsync(x => x.Id == item.Id);
            resource.IsVisible = !item.IsVisible;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteResource(Resource resource)
        {
            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public class ResponseRepository : IResponseRepository
    {
        private readonly ShopWebDbContext _context;

        public ResponseRepository(ShopWebDbContext context)
        {
            _context = context;
        }

        public async Task<List<Response>> GetAllResponsesAsync()
        {
            return await _context.Responses.ToListAsync();
        }

        public async Task<Response> GetResponseByIdAsync(Guid id)
        {
            return await _context.Responses.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Response>> GetResponsesByUserIdAsync(Guid userId)
        {
            return await _context.Responses.Where(r => r.UserId == userId).ToListAsync();
        }

        public async Task<Response> AddResponseAsync(Response response)
        {
            _context.Responses.Add(response);
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<Response> UpdateResponseAsync(Response response)
        {
            _context.Entry(response).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<bool> DeleteResponseAsync(Guid id)
        {
            var response = await _context.Responses.FindAsync(id);
            if (response == null)
                return false;

            _context.Responses.Remove(response);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

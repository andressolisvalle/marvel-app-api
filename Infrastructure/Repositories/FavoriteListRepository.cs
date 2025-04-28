using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class FavoriteListRepository : IFavoriteListRepository
    {
        private readonly MarvelDbContext _context;

        public FavoriteListRepository(MarvelDbContext context)
        {
            _context = context;
        }

        public async Task AddFavoriteAsync(FavoriteComic favoriteComic)
        {
            await _context.FavoriteComics.AddAsync(favoriteComic);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FavoriteComic>> GetFavoritesByUserIdAsync(Guid userId)
        {
            return await _context.FavoriteComics
                     .Where(f => f.UserId == userId)
                     .Include(f => f.User) 
                     .ToListAsync();
        }

        public async Task RemoveFavoriteAsync(Guid favoriteId)
        {
            var favorite = await _context.FavoriteComics.FindAsync(favoriteId);
            if (favorite != null)
            {
                _context.FavoriteComics.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }
    }
}

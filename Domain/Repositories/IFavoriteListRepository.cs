using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IFavoriteListRepository
    {
        Task<IEnumerable<FavoriteComic>> GetFavoritesByUserIdAsync(Guid userId);
        Task AddFavoriteAsync(FavoriteComic favoriteComic);
        Task RemoveFavoriteAsync(Guid favoriteId);
    }
}

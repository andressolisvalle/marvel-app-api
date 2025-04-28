using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IFavoriteService
    {
        Task<bool> AddFavoriteAsync(AddFavoriteComicDto addFavoriteComicDto);
        Task<List<FavoriteComicDto>> GetFavoritesAsync();
        Task DeleteFavoriteAsync(Guid favoriteId);
    }
}

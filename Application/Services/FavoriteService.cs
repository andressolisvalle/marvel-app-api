using Application.DTOs;
using Application.Interface;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteListRepository _favoriteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public FavoriteService(IFavoriteListRepository favoriteRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _favoriteRepository = favoriteRepository;
            _httpContextAccessor = httpContextAccessor;

        }
        private Guid GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new Exception("Usuario no autenticado.");
            }
            return Guid.Parse(userIdClaim.Value);
        }
        public async Task<bool> AddFavoriteAsync(AddFavoriteComicDto addFavoriteComicDto)
        {
            var userId = GetUserId();

            // Verificar si ya existe este cómic en favoritos
            var existingFavorites = await _favoriteRepository.GetFavoritesByUserIdAsync(userId);

            if (existingFavorites.Any(f => f.ComicId == addFavoriteComicDto.ComicId))
            {
                return false; 
            }
            var newFavorite = new FavoriteComic
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ComicId = addFavoriteComicDto.ComicId,
                Title = addFavoriteComicDto.Title,
                ImageUrl = addFavoriteComicDto.ImageUrl
            };

            await _favoriteRepository.AddFavoriteAsync(newFavorite);

            return true;
        }
        public async Task<List<FavoriteComicDto>> GetFavoritesAsync()
        {
            var userId = GetUserId();
            var favorites = await _favoriteRepository.GetFavoritesByUserIdAsync(userId);

            var mappedFavorites = new List<FavoriteComicDto>();

            foreach (var favorite in favorites)
            {
                if (favorite == null)
                    continue;

                mappedFavorites.Add(new FavoriteComicDto
                {
                    Id = favorite.Id,
                    ComicId = favorite.ComicId,
                    Title = favorite.Title ?? string.Empty,
                    ImageUrl = favorite.ImageUrl ?? string.Empty
                });
            }

            return mappedFavorites;
        }


        public async Task DeleteFavoriteAsync(Guid favoriteId)
        {
            await _favoriteRepository.RemoveFavoriteAsync(favoriteId);
        }
    }
}

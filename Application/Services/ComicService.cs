using Application.DTOs;
using Application.Interface;
using AutoMapper;
using Azure;
using Domain.Entities;
using Infrastructure.MarvelApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace Application.Services
{
    public class ComicService: IComicService
    {
        private readonly MarvelApiClient _marvelApiClient;
        private readonly IMapper _mapper;
        public ComicService(MarvelApiClient marvelApiClient, IMapper mapper)
        {
            _marvelApiClient = marvelApiClient;
            _mapper = mapper;
        }

        public async Task<ComicDto> GetComicByIdAsync(int id)
        {
            var comicsJson = await _marvelApiClient.GetComicByIdAsync(id);

            var comicsWrapper = JsonSerializer.Deserialize<ComicDataWrapperDto>(comicsJson);

            var comic = comicsWrapper?.data?.results.FirstOrDefault();

            if (comic == null)
                throw new Exception("Cómic no encontrado.");


            return _mapper.Map<ComicDto>(comic); 
        }

        public async Task<List<ComicDto>> GetComicsAsync()
        {
            var comicsJson = await _marvelApiClient.GetComicsAsync();

            var comicsWrapper = JsonSerializer.Deserialize<ComicDataWrapperDto>(comicsJson);

            var comics = comicsWrapper?.data?.results;

            var mappedComics = _mapper.Map<List<ComicDto>>(comics);

            return mappedComics;
        }
    }
}

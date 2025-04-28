using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MarvelApi
{
    public class MarvelApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _publicKey;
        private readonly string _privateKey;
        private readonly string _baseUrl;

        public MarvelApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _publicKey = configuration["MarvelApi:PublicKey"];
            _privateKey = configuration["MarvelApi:PrivateKey"];
            _baseUrl = configuration["MarvelApi:BaseUrl"];
        }

        private string GenerateHash(string timestamp)
        {
            var input = timestamp + _privateKey + _publicKey;
            using var md5 = MD5.Create();
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder();
            foreach (var b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        public async Task<string> GetComicsAsync()
        {
            var ts = DateTime.UtcNow.Ticks.ToString();
            var hash = GenerateHash(ts);

            var url = $"{_baseUrl}comics?ts={ts}&apikey={_publicKey}&hash={hash}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error al consultar la API de Marvel.");
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetComicByIdAsync(int id)
        {
            var ts = DateTime.UtcNow.Ticks.ToString();
            var hash = GenerateHash(ts);

            var url = $"{_baseUrl}comics/{id}?ts={ts}&apikey={_publicKey}&hash={hash}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error al consultar la API de Marvel.");
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}

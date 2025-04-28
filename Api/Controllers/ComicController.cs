using Application.DTOs;
using Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ComicController : ControllerBase
    {
        private readonly IComicService _comicService;
        private readonly IFavoriteService _favoriteService;

        public ComicController(IComicService comicService, IFavoriteService favoriteService)
        {
            _comicService = comicService;
            _favoriteService = favoriteService;
        }

        /// <summary>
        /// Obtener lista de cómics disponibles.
        /// </summary>
        /// <returns>Listado de cómics.</returns>
        /// <response code="200">Lista de cómics obtenida exitosamente.</response>
        /// <response code="400">Error al obtener la lista de cómics.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetComics()
        {
            try
            {
                var comics = await _comicService.GetComicsAsync();

                return Ok(new
                {
                    message = "Lista de cómics obtenida exitosamente.",
                    data = comics
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// HU02_CA02 - Obtener detalles de un cómic específico.
        /// </summary>
        /// <param name="id">ID del cómic a consultar.</param>
        /// <returns>Datos del cómic.</returns>
        /// <response code="200">Cómic obtenido exitosamente.</response>
        /// <response code="204">No se encontró el cómic solicitado.</response>
        /// <response code="400">Error al obtener el cómic.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetComicById(int id)
        {
            try
            {
                var comic = await _comicService.GetComicByIdAsync(id);
                if (comic == null)
                {
                    return NoContent();
                }

                return Ok(new
                {
                    message = "Cómic obtenido exitosamente.",
                    data = comic
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // --- FAVORITES --- //


        /// <summary>
        /// Agregar un cómic a favoritos.
        /// </summary>
        /// <param name="favoriteDto">Información del cómic a agregar a favoritos.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="201">Cómic agregado a favoritos exitosamente.</response>
        /// <response code="409">El cómic ya estaba en favoritos.</response>
        /// <response code="400">Error al agregar el cómic a favoritos.</response>
        [HttpPost("favorite")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteComicDto favoriteDto)
        {
            try
            {
                var added = await _favoriteService.AddFavoriteAsync(favoriteDto);

                if (!added)
                {
                    return Conflict(new { error = "Este cómic ya está en tu lista de favoritos." }); 
                }

                return Created(string.Empty, new { message = "Cómic agregado exitosamente a favoritos." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Obtener la lista de cómics favoritos del usuario.
        /// </summary>
        /// <returns>Lista de cómics favoritos.</returns>
        /// <response code="200">Lista de favoritos obtenida exitosamente.</response>
        /// <response code="400">Error al obtener los favoritos.</response>
        [HttpGet("favorite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFavorites()
        {
            try
            {
                var favorites = await _favoriteService.GetFavoritesAsync();
                
                return Ok(new
                {
                    message = "Lista de favoritos obtenida exitosamente.",
                    data = favorites
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Eliminar un cómic de la lista de favoritos.
        /// </summary>
        /// <param name="id">ID del favorito a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="204">Favorito eliminado exitosamente.</response>
        /// <response code="400">Error al eliminar el favorito.</response>
        [HttpDelete("favorite/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteFavorite(Guid id)
        {
            try
            {
                await _favoriteService.DeleteFavoriteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}

using Application.DTOs;
using Application.Interface;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserSevice _userService;
        public AuthController(IUserSevice userSevice)
        {
            _userService = userSevice;

        }

        /// <summary>
        /// Registro de nuevo usuario.
        /// </summary>
        /// <param name="registerUserDto">Objeto con los datos del usuario a registrar (nombre, email, contraseña, identificación).</param>
        /// <returns>Resultado del registro del usuario.</returns>
        /// <response code="200">Usuario registrado exitosamente.</response>
        /// <response code="409">El correo electrónico ya está registrado.</response>
        /// <response code="400">Error de validación o error al registrar el usuario.</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                var success = await _userService.RegisterAsync(registerUserDto);

                if (!success)
                {
                    return Conflict(new { error = "El correo electrónico ya está registrado." });
                }

                return Ok(new { message = "Usuario registrado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        /// <summary>
        /// Inicio de sesión de usuario.
        /// </summary>
        /// <param name="loginDto">Objeto que contiene las credenciales del usuario (email y contraseña).</param>
        /// <returns>Devuelve un token JWT si la autenticación es exitosa.</returns>
        /// <response code="200">Login exitoso. Se devuelve el token JWT.</response>
        /// <response code="401">Credenciales inválidas. No autorizado.</response>
        /// <response code="400">Error de validación en los datos enviados.</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(object))]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = await _userService.LoginAsync(loginDto);

                if (token == null)
                {
                    return Unauthorized(new { error = "Credenciales inválidas." });
                }

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }
    }

}
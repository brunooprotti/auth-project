using auth_backend.Bussiness.DTOs;
using auth_backend.Bussiness.Repository.IRepository;
using auth_backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace auth_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        public AuthController(ILogger<AuthController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            List<ResponseUserDto> listaUsuarios = await _userRepository.GetAll();
            return Ok(listaUsuarios);
        }

        // GET api/<AuthController>/email
        [HttpGet("{email}")]
        public async Task<IActionResult> Get(string email, string documentNumber)
        {
            var user = await _userRepository.UserExists(email,documentNumber);
            return Ok(user);
        }

        // POST api/<AuthController>
        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError(ModelState.ToString());
                    return BadRequest("Invalid information.");
                }

                var userExists = await _userRepository.UserExists(value.Email!, value.DocumentNumber);

                if (userExists != null  && userExists.Email == value.Email) return BadRequest("Email is already in use.");
                if (userExists != null  && userExists.DocumentNumber == value.DocumentNumber) return BadRequest("User is already created.");

                var result = await _userRepository.Register(value);

                if (!result) return Problem("User can't be created.");

                return CreatedAtAction("Register", new object { });
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrio un error al crear un usuario", ex.Message);
                throw ex;
            }
        }

        // POST api/<AuthController>/Register
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginUserDto value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

                //El modelState tiene los resultados de las validaciones del UsuarioLoginDto
            }

            var respuestaLogin = await _userRepository.Login(value);

            if (respuestaLogin.UserInfo == null || string.IsNullOrEmpty(respuestaLogin.Token))
            {
                return BadRequest("Email or password are invalid.");
            }

            return Ok(respuestaLogin);
        }

        // PUT api/<AuthController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

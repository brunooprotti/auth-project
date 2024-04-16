using auth_backend.Bussiness.Repository.IRepository;
using auth_backend.DTOs;
using Microsoft.AspNetCore.Mvc;

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
            List<UserDto> listaUsuarios = await _userRepository.GetAll();
            return Ok(listaUsuarios);
        }

        // GET api/<AuthController>/5
        [HttpGet("{email}")]
        public async Task<IActionResult> Get(string email)
        {
            UserDto user = await _userRepository.GetByEmail(email);
            return Ok(user);
        }

        // POST api/<AuthController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] UserDto value)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrio un error al crear un usuario", ex.Message);
                throw ex;
            }
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

using Asp.Versioning;
using AutoMapper;
using Fidenz.Customers.Data.Common.Interfaces;
using Fidenz.Customers.Data.Models;
using Fidenz.Customers.Data.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fidenz.Customers.Presentation.Controllers
{
    [ApiVersion(1)]
    [ApiVersion(2.2)]
    [Route("[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public APIController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Invalid login request");
            }
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                // User authenticated successfully
                var roles = await _userManager.GetRolesAsync(user);

                // Use the JwtTokenGenerator to create a JWT token
                var token = _jwtTokenGenerator.GenerateJwtToken(user.UserName, roles.FirstOrDefault());

                return Ok(token);
            }

            return Unauthorized("Invalid email or password");
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpPut("{id}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateUser(string id, [FromBody] EditUserDto editUserDto)
        {
            if (editUserDto == null || id != editUserDto.Id)
            {
                return BadRequest();
            }

            var user = _unitOfWork.User.GetUserByIdAsync(id).Result;

            _mapper.Map(editUserDto, user);

            _unitOfWork.User.UpdateUser(user);
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpGet("{id}", Name = "GetDistance")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDistance(string id, double Latitude, double Longitude)
        {
            if (id == null || Latitude == null || Longitude == null)
            {
                return BadRequest();
            }

            var distance = await _unitOfWork.User.CalculateDistanceAsync(id, Latitude, Longitude);
            return Ok(distance + "Km");
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<User>>> Search(string word)
        {
            var result = await _unitOfWork.User.SearchUsersAsync(word);
            if (result.Any())
            {
                return Ok(result);
            }
            return NotFound();
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpGet("groupedbyzipcode")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UsersByZipCodeDto>>> GetUsersGroupedByZipCode()
        {
            var result = await _unitOfWork.User.GetUsersGroupedByZipCodeAsync();
            if (result.Any())
            {
                return Ok(result);
            }
            return NotFound();
        }
    }
}

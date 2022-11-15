using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ShoppingNotes.Data;
using ShoppingNotes.Dtos;
using ShoppingNotes.Models;
using ShoppingNotes.Services;

namespace ShoppingNotes.Controllers
{
    /// <summary>
    /// The controller for the actions related to Users
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private const string IdType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Users Constructor
        /// </summary>
        public UsersController(IUserRepo userRepo, IMapper mapper, IAuthService authService, IConfiguration configuration)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Gets the user info for the logged in user
        /// </summary>
        /// <returns>The user</returns>
        /// <response code="200">Ok - The user was fetched successfully</response>
        /// <response code="400">Bad Request - Invalid user input</response>
        /// <response code="401">Not Authorized - The user is not logged in</response>
        /// <response code="404">Not Found - The user object is not found</response>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserReadDto>> GetUser()
        {
            int userId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == IdType)?.Value!);

            var user = await _userRepo.GetUserByIdAsync(userId);

            if (user == null) 
            { 
                return NotFound();
            }

            var userReadDto = _mapper.Map<UserReadDto>(user);

            return Ok(userReadDto);
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="userCreateDto">The user object</param>
        /// <returns>The created user</returns>
        /// <response code="201">Created - The user was created successfully</response>
        /// <response code="400">Bad Request - Invalid user input</response>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserReadDto>> CreateUser(UserCreateDto userCreateDto)
        {
            userCreateDto.UserName = userCreateDto.UserName!.ToLower();

            var userExists = await _userRepo.GetUserByUserNameAsync(userCreateDto.UserName!);

            if (userExists != null)
            {
                return BadRequest("Username already exists");
            }

            var user = _mapper.Map<User>(userCreateDto);
            
            // Add the hashed credentials to the user object
            _authService.CreatePasswordHash(userCreateDto.Password!, out byte[] hash, out byte[] salt);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            await _userRepo.CreateUserAsync(user);
            await _userRepo.SaveChangesAsync();

            var userReadDto = _mapper.Map<UserReadDto>(user);

            return CreatedAtRoute("", userReadDto);
        }

        /// <summary>
        /// Updates the logged in users password
        /// </summary>
        /// <param name="userUpdateDto">The update body</param>
        /// <returns>An ActionResult (NoContent)</returns>
        /// <response code="204">NoContent - The user was updated successfully</response>
        /// <response code="400">Bad Request - Invalid user input</response>
        /// <response code="401">Unauthorized - Invalid password or not logged in</response>
        /// <response code="404">Not Found - Invalid password</response>
        [HttpPut]
        [Authorize]
        public async Task<ActionResult> UpdatePassword(UserUpdateDto userUpdateDto)
        {
            if (userUpdateDto.NewPassword != userUpdateDto.RepeatNewPassword)
            {
                return BadRequest("Passwords must match.");
            }

            int userId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == IdType)?.Value!);

            var user = await _userRepo.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            bool verifyPassword = _authService.VerifyPasswordHash(userUpdateDto.CurrentPassword!, user.PasswordHash!, user.PasswordSalt!);

            if (!verifyPassword)
            {
                return Unauthorized("Invalid password");
            }

            _authService.CreatePasswordHash(userUpdateDto.NewPassword!, out byte[] hash, out byte[] salt);

            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            await _userRepo.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes the currently logged in user
        /// </summary>
        /// <returns>An ActionResult (NoContent)</returns>
        /// <response code="204">No Content - The user was deleted successfully</response>
        /// <response code="401">Unauthorized - The user is not logged in</response>
        /// <response code="404">Not Found - The user is not found</response>
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeleteUser()
        {
            int userId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == IdType)?.Value!);

            var user = await _userRepo.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            _userRepo.DeleteUser(user);

            await _userRepo.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Logs the user in to the API
        /// </summary>
        /// <param name="userAuthDto">The login object containing userName and password</param>
        /// <returns>A new JWT to authenticate the user</returns>
        /// <response code="200">Ok - The user was authenticated successfully</response>
        /// <response code="400">Bad Request - Invalid user input</response>
        /// <response code="401">Unauthorized - Wrong user name or password</response>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login(UserAuthDto userAuthDto)
        {
            userAuthDto.UserName = userAuthDto.UserName!.ToLower();

            var user = await _userRepo.GetUserByUserNameAsync(userAuthDto.UserName!);

            if (user == null)
            {
                return Unauthorized();
            }

            bool passwordIsValid = _authService.VerifyPasswordHash(userAuthDto.Password!, user.PasswordHash!, user.PasswordSalt!);

            if (!passwordIsValid)
            {
                return Unauthorized();
            }

            UserReadDto userReadDto = _mapper.Map<UserReadDto>(user);

            return Ok(_authService.CreateToken(userReadDto));

        }
    }
}
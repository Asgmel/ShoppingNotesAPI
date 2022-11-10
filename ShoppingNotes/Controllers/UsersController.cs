using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ShoppingNotes.Data;
using ShoppingNotes.Dtos;
using ShoppingNotes.Models;

namespace ShoppingNotes.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly ISListRepo _sListRepo;
        private readonly INoteRepo _noteRepo;
        private readonly IMapper _mapper;

        public UsersController(IUserRepo userRepo, ISListRepo sListRepo, INoteRepo noteRepo, IMapper mapper)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _sListRepo = sListRepo ?? throw new ArgumentNullException(nameof(sListRepo));
            _noteRepo = noteRepo ?? throw new ArgumentNullException(nameof(noteRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets a single user by ID
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <returns>The user</returns>
        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<ActionResult<UserReadDto>> GetUserById(int id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);

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
        [HttpPost]
        public async Task<ActionResult<UserReadDto>> CreateUser(UserCreateDto userCreateDto)
        {
            var user = _mapper.Map<User>(userCreateDto);
            await _userRepo.CreateUserAsync(user);
            await _userRepo.SaveChangesAsync();

            var userReadDto = _mapper.Map<UserReadDto>(user);

            return CreatedAtRoute(nameof(GetUserById), new { Id = userReadDto.Id }, userReadDto);
        }

        /// <summary>
        /// Updates a user by ID with the PUT method
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <param name="userUpdateDto">The update body</param>
        /// <returns>An ActionResult (NoContent)</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, UserUpdateDto userUpdateDto)
        {
            var user = await _userRepo.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _mapper.Map(userUpdateDto, user);

            await _userRepo.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Updates a user by ID with the PATCH method
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <param name="patchDocument">The patchDocument body</param>
        /// <returns>An ActionResult (NoContent)</returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdateUser(int id, JsonPatchDocument<UserUpdateDto> patchDocument)
        {
            var user = await _userRepo.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userUpdateDto = _mapper.Map<UserUpdateDto>(user);

            patchDocument.ApplyTo(userUpdateDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!TryValidateModel(userUpdateDto))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(userUpdateDto, user);
            
            await _userRepo.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes an user and all their lists and notes.
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <returns>An ActionResult (NoContent)</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            // Delete all the users notes
            var notes = await _noteRepo.GetAllNotesAsync(user.Id);
            _noteRepo.DeleteNotes(notes);

            // Delete all the users lists
            var sLists = await _sListRepo.GetAllListsAsync(id);
            _sListRepo.DeleteLists(sLists);

            // Delete the user
            _userRepo.DeleteUser(user);

            await _userRepo.SaveChangesAsync();

            return NoContent();
        }
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ShoppingNotes.Data;
using ShoppingNotes.Dtos;
using ShoppingNotes.Models;

namespace ShoppingNotes.Controllers
{
    /// <summary>
    /// The controller for the routes related to SLists
    /// </summary>
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class SListsController : ControllerBase
    {
        private const string IdType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        private readonly IUserRepo _userRepo;
        private readonly ISListRepo _sListRepo;
        private readonly IMapper _mapper;

        /// <summary>
        /// ListsController Constructor
        /// </summary>
        public SListsController(ISListRepo sListRepo, IUserRepo userRepo, IMapper mapper)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _sListRepo = sListRepo ?? throw new ArgumentNullException(nameof(sListRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets all lists for a user
        /// </summary>
        /// <returns>A list of lists</returns>
        /// <response code="200">Ok - List get request successfull</response>
        /// <response code="400">Bad Request - Invalid user input</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SListReadDto>>> GetAllLists()
        {
            int userId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == IdType)?.Value!);

            var sLists = await _sListRepo.GetAllListsAsync(userId);

            var sListsReadDto = _mapper.Map<IEnumerable<SListReadDto>>(sLists);

            return Ok(sListsReadDto);
        }

        /// <summary>
        /// Gets a single list by ID
        /// </summary>
        /// <param name="id">The list ID</param>
        /// <returns>A single list</returns>
        /// <response code="200">Ok - List get request successfull</response>
        /// <response code="400">Bad Request - Invalid user input</response>
        /// <response code="403">Forbidden - User is not authorized to access the list</response>
        /// <response code="404">Not Found - The supplied list was not found</response>
        [HttpGet("{id}", Name = "GetListById")]
        public async Task<ActionResult<SListReadDto>> GetListById(int id)
        {
            var sList = await _sListRepo.GetListByIdAsync(id);

            if (sList == null)
            {
                return NotFound();
            }

            int userId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == IdType)?.Value!);

            if (sList.UserId != userId)
            {
                return Forbid();
            }

            var sListReadDto = _mapper.Map<SListReadDto>(sList);

            return Ok(sListReadDto);
        }

        /// <summary>
        /// Creates a new list
        /// </summary>
        /// <param name="sListCreateDto">The list body</param>
        /// <returns>The created list</returns>
        /// <response code="201">Created - The list was created successfully</response>
        /// <response code="400">Bad Request - Invalid user input</response>
        /// <response code="404">NotFound - The user was not found</response>
        [HttpPost]
        public async Task<ActionResult<SListReadDto>> CreateList(SListCreateDto sListCreateDto)
        {
            int userId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == IdType)?.Value!);

            var user = await _userRepo.GetUserByIdAsync(userId);

            var sList = _mapper.Map<SList>(sListCreateDto);
            sList.UserId = userId;
            sList.User = user;

            await _sListRepo.CreateListAsync(sList);
            await _sListRepo.SaveChangesAsync();

            var sListReadDto = _mapper.Map<SListReadDto>(sList);

            return CreatedAtRoute(nameof(GetListById), new { Id = sListReadDto.Id }, sListReadDto);
        }

        /// <summary>
        /// Updates a list by ID with the PUT method
        /// </summary>
        /// <param name="id">The id of the list to update</param>
        /// <param name="sListUpdateDto">The update body</param>
        /// <returns>An ActionResult (NoContent)</returns>
        /// <response code="204">NoContent - The list was updated successfully</response>
        /// <response code="400">Bad Request - Invalid user input</response>
        /// <response code="403">Forbidden - User is not authorized to update the list</response>
        /// <response code="404">Not Found - The supplied list was not found</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateList(int id, SListUpdateDto sListUpdateDto)
        {
            var sList = await _sListRepo.GetListByIdAsync(id);

            if (sList == null)
            {
                return NotFound();
            }

            int userId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == IdType)?.Value!);

            if (sList.UserId != userId)
            {
                return Forbid();
            }

            _mapper.Map(sListUpdateDto, sList);

            await _sListRepo.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Updates a list by ID with the PATCH method
        /// </summary>
        /// <param name="id">The list id</param>
        /// <param name="patchDocument">The patchdocument body</param>
        /// <returns>An ActionResult (NoContent)</returns>
        /// <response code="204">NoContent - The list is updated successfully</response>
        /// <response code="400">Bad Request - Invalid user input</response>
        /// <response code="403">Forbidden - User is not authorized to update the list</response>
        /// <response code="404">Not Found - The supplied list was not found</response>
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdateList(int id, JsonPatchDocument<SListUpdateDto> patchDocument)
        {
            var sList = await _sListRepo.GetListByIdAsync(id);

            if (sList == null)
            {
                return NotFound();
            }

            int userId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == IdType)?.Value!);

            if (sList.UserId != userId)
            {
                return Forbid();
            }

            var sListUpdateDto = _mapper.Map<SListUpdateDto>(sList);

            patchDocument.ApplyTo(sListUpdateDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(sListUpdateDto))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(sListUpdateDto, sList);

            await _sListRepo.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes a list and all its tasks from the DB
        /// </summary>
        /// <param name="id">The list ID</param>
        /// <returns>An ActionResult (NoContent)</returns>
        /// <response code="204">NoContent - The list was deleted successfully</response>
        /// <response code="400">Bad Request - Invalid user input</response>
        /// <response code="403">Forbidden - User is not authorized to delete the list</response>
        /// <response code="404">Not Found - The supplied list was not found</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteList(int id)
        {
            var sList = await _sListRepo.GetListByIdAsync(id);

            if (sList == null)
            {
                return NotFound();
            }

            int userId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == IdType)?.Value!);

            if (sList.UserId != userId)
            {
                return Forbid();
            }

            _sListRepo.DeleteList(sList);
            await _sListRepo.SaveChangesAsync();

            return NoContent();
        }
    }
}

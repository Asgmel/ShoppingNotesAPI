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
    public class SListsController : ControllerBase
    {
        private readonly ISListRepo _sListRepo;
        private readonly INoteRepo _noteRepo;
        private readonly IMapper _mapper;

        public SListsController(ISListRepo sListRepo, INoteRepo noteRepo, IMapper mapper)
        {
            _sListRepo = sListRepo ?? throw new ArgumentNullException(nameof(sListRepo));
            _noteRepo = noteRepo ?? throw new ArgumentNullException(nameof(noteRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets all lists for a user
        /// </summary>
        /// <param name="userId">The user ID - Will be replaced by authentication</param>
        /// <returns>A list of lists</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SListReadDto>>> GetAllLists(int userId)
        {
            var sLists = await _sListRepo.GetAllListsAsync(userId);

            var sListsReadDto = _mapper.Map<IEnumerable<SListReadDto>>(sLists);

            return Ok(sListsReadDto);
        }

        /// <summary>
        /// Gets a single list by ID
        /// </summary>
        /// <param name="id">The list ID</param>
        /// <param name="userId">The user ID - Will be replaced by authentication</param>
        /// <param name="includeNotes">true if the result should include the lists notes, false otherwise</param>
        /// <returns>A single list</returns>
        [HttpGet("{id}", Name = "GetListById")]
        public async Task<ActionResult<SListReadDto>> GetListById(int id, int userId, bool includeNotes)
        {
            var sList = await _sListRepo.GetListByIdAsync(id, includeNotes);

            if (sList == null)
            {
                return NotFound();
            }

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
        [HttpPost]
        public async Task<ActionResult<SListReadDto>> CreateList(SListCreateDto sListCreateDto)
        {
            var sList = _mapper.Map<SList>(sListCreateDto);
            await _sListRepo.CreateListAsync(sList);
            await _sListRepo.SaveChangesAsync();

            var sListReadDto = _mapper.Map<SListReadDto>(sList);

            return CreatedAtRoute(nameof(GetListById), new { Id = sListReadDto.Id }, sListReadDto);
        }

        /// <summary>
        /// Updates a list by ID with the PUT method
        /// </summary>
        /// <param name="id">The id of the list to update</param>
        /// <param name="userId">The user ID - Will be replaced by authentication</param>
        /// <param name="sListUpdateDto">The update body</param>
        /// <returns>An ActionResult (NoContent)</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateList(int id, int userId, SListUpdateDto sListUpdateDto)
        {
            var sList = await _sListRepo.GetListByIdAsync(id);

            if (sList == null)
            {
                return NotFound();
            }

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
        /// <param name="userId">The user ID - Will be replaced by authentication</param>
        /// <param name="patchDocument">The patchdocument body</param>
        /// <returns>An ActionResult (NoContent)</returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdateList(int id, int userId, JsonPatchDocument<SListUpdateDto> patchDocument)
        {
            var sList = await _sListRepo.GetListByIdAsync(id);

            if (sList == null)
            {
                return NotFound();
            }

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
        /// <param name="userId">The user ID - Will be replaced by authentication</param>
        /// <returns>An ActionResult (NoContent)</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteList(int id, int userId)
        {
            var sList = await _sListRepo.GetListByIdAsync(id);

            if (sList == null)
            {
                return NotFound();
            }

            if (sList.UserId != userId)
            {
                return Forbid();
            }

            // Delete all the lists notes
            var notes = await _noteRepo.GetAllNotesAsync(userId);
            _noteRepo.DeleteNotes(notes);

            // Delete the list
            _sListRepo.DeleteList(sList);
            await _sListRepo.SaveChangesAsync();

            return NoContent();
        }
    }
}

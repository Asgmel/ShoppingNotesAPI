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
    public class NotesController : ControllerBase
    {
        private readonly INoteRepo _noteRepo;
        private readonly ISListRepo _sListRepo;
        private readonly IMapper _mapper;

        /// <summary>
        /// A controller for actions related to notes
        /// </summary>
        public NotesController(INoteRepo noteRepo, ISListRepo sListRepo, IMapper mapper)
        {
            _noteRepo = noteRepo ?? throw new ArgumentNullException(nameof(noteRepo));
            _sListRepo = sListRepo ?? throw new ArgumentNullException(nameof(sListRepo));   
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets all notes for a user, or a list if listId is specified.
        /// </summary>
        /// <param name="listId">OPTIONAL - List listId to filter results by</param>
        /// <param name="userId">The user ID - Will be replaced by authentication</param>
        /// <returns>A list of notes</returns>
        /// <response code="200">OK - Returns the requested notes</response>
        /// <response code="400">Bad Request - Invalid user input</response>
        /// <response code="403">Forbidden - User is not authorized to access the supplied listId notes</response>
        /// <response code="404">Not Found - The supplied listId was not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<NoteReadDto>>> GetAllNotes(int? listId, int userId)
        {
            IEnumerable<Note> notes = new List<Note>();

            if (listId != null)
            {
                var sList = await _sListRepo.GetListByIdAsync((int)listId);

                if (sList == null)
                {
                    return NotFound();
                }

                if (sList.UserId != userId)
                {
                    return Forbid();
                }

                notes = await _noteRepo.GetAllNotesForListAsync((int)listId);
            }
            if (listId == null)
            {
                notes = await _noteRepo.GetAllNotesAsync(userId);
            }

            var notesReadDto = _mapper.Map<IEnumerable<NoteReadDto>>(notes);

            return Ok(notesReadDto);
        }

        /// <summary>
        /// Gets a single note by ID
        /// </summary>
        /// <param name="id">The ID of the note to fetch</param>
        /// <param name="userId">The user ID - Will be replaced by authentication</param>
        /// <returns>A note</returns>
        /// <response code="200">OK - Returns the requested note</response>
        /// <response code="400">Bad Request - Invalid user input</response>
        /// <response code="403">Forbidden - User is not authorized to access the note</response>
        /// <response code="404">Not Found - The supplied note was not found</response>
        [HttpGet("{id}", Name = "GetNoteById")]
        public async Task<ActionResult<NoteReadDto>> GetNoteById(int id, int userId)
        {
            var note = await _noteRepo.GetNoteByIdAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            var sList = await _sListRepo.GetListByIdAsync(note.SListId, false);

            if (sList == null)
            {
                return NotFound();
            }

            if (sList.UserId != userId)
            {
                return Forbid();
            }

            var noteReadDto = _mapper.Map<NoteReadDto>(note);

            return Ok(noteReadDto);
        }

        /// <summary>
        /// Create a new note
        /// </summary>
        /// <param name="userId">The user ID - Will be replaced by authentication</param>
        /// <param name="noteCreateDto">The note object</param>
        /// <returns>The created note</returns>
        /// <response code="200">OK - Creates the note</response>
        /// <response code="400">Bad Request - Invalid user input</response>
        /// <response code="403">Forbidden - User is not authorized to access the note</response>
        /// <response code="404">Not Found - The list was not found</response>
        [HttpPost]
        public async Task<ActionResult<SListReadDto>> CreateNote(int userId, NoteCreateDto noteCreateDto)
        {
            var sList = await _sListRepo.GetListByIdAsync(noteCreateDto.SListId, false);

            if (sList == null)
            {
                return NotFound();
            }

            if (sList.UserId != userId)
            {
                return Forbid();
            }

            var note = _mapper.Map<Note>(noteCreateDto);
            await _noteRepo.CreateNoteAsync(note);
            await _noteRepo.SaveChangesAsync();

            var noteReadDto = _mapper.Map<NoteReadDto>(note);

            return CreatedAtRoute(nameof(GetNoteById), new { Id = noteReadDto.Id }, noteReadDto);
        }

        /// <summary>
        /// Update a note with the PUT method
        /// </summary>
        /// <param name="id">The noteId to update</param>
        /// <param name="userId">The user ID - Will be replaced by authentication</param>
        /// <param name="noteUpdateDto">The note body with updated values</param>
        /// <returns>An ActionResult (NoContent)</returns>
        /// <response code="204">NoContent - The note was updated successfully</response>
        /// <response code="400">Bad Request - Invalid user input</response>
        /// <response code="403">Forbidden - User is not authorized to access the note</response>
        /// <response code="404">Not Found - The supplied note was not found</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateNote(int id, int userId, NoteUpdateDto noteUpdateDto)
        {
            var note = await _noteRepo.GetNoteByIdAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            var sList = await _sListRepo.GetListByIdAsync(note.SListId);

            if (sList == null)
            {
                return NotFound();
            }

            if (sList.UserId != userId)
            {
                return Forbid();
            }

            _mapper.Map(noteUpdateDto, note);

            await _noteRepo.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Update the note with the PATCH method
        /// </summary>
        /// <param name="id">The id of the note to update</param>
        /// <param name="userId">The user ID - Will be replaced by authentication</param>
        /// <param name="patchDocument">The patchdocument body</param>
        /// <returns>An Actionresult (NoContent)</returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdateNote(int id, int userId, JsonPatchDocument<NoteUpdateDto> patchDocument)
        {
            var note = await _noteRepo.GetNoteByIdAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            var sList = await _sListRepo.GetListByIdAsync(note.SListId);

            if (sList == null)
            {
                return NotFound();
            }

            if (sList.UserId != userId)
            {
                return Forbid();
            }

            var noteUpdateDto = _mapper.Map<NoteUpdateDto>(note);

            patchDocument.ApplyTo(noteUpdateDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(noteUpdateDto))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(noteUpdateDto, note);

            await _noteRepo.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes a note
        /// </summary>
        /// <param name="id">The id of the note to delete</param>
        /// <param name="userId">The user ID - Will be replaced by authentication</param>
        /// <returns>An ActionResult (NoContent)</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNote(int id, int userId)
        {
            var note = await _noteRepo.GetNoteByIdAsync(id);

            if (note == null)
            {
                return NotFound("Note not found");
            }
            var sList = await _sListRepo.GetListByIdAsync(note.SListId);

            if (sList == null)
            {
                return NotFound("List not found");
            }

            if (sList.UserId != userId)
            {
                return Forbid();
            }

            _noteRepo.DeleteNote(note);
            await _noteRepo.SaveChangesAsync();

            return NoContent();
        }
    }
}

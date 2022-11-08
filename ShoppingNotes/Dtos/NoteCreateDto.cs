﻿using System.ComponentModel.DataAnnotations;

namespace ShoppingNotes.Dtos
{
    public class NoteCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string? Content { get; set; }

        [Required]
        public int OwnerId { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ShoppingNotes.Data;
using ShoppingNotes.Dtos;
using System.Xml.Linq;

namespace ShoppingNotes.Services
{
    /// <summary>
    /// Interface for auth services
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Creates a password hash and salt based on the input plaintext password
        /// </summary>
        /// <param name="password">The users password in plaintext</param>
        /// <param name="hash">Contains the created hash based on the input password</param>
        /// <param name="salt">Contains the created salt based on the input password</param>
        public void CreatePasswordHash(string password, out byte[] hash, out byte[] salt);

        /// <summary>
        /// Verifies that the input password hash matches the users password hash
        /// </summary>
        /// <param name="password">The user plaintext password</param>
        /// <param name="passwordHash">The user hashed password</param>
        /// <param name="passwordSalt">The salt used to hash the users password</param>
        /// <returns>true if the passwords match, false otherwise.</returns>
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);

        /// <summary>
        /// Creates a JWT token for the user
        /// </summary>
        /// <returns>a string with the users JWT token</returns>
        public string CreateToken(UserReadDto userReadDto);

    }
}

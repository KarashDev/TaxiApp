using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Customer.Models;

namespace TaxiApp.Customer.Services
{
    public enum RegistrationResult
    {
        Success,
        EmptyInput,
        EmailAlreadyExists,
        UsernameAlreadyExists
    }

    public interface IAuthenticationService
    {
        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="username">The user's name.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="confirmPassword">The user's confirmed password.</param>
        /// <returns>The result of the registration.</returns>
        /// <exception cref="Exception">Thrown if the registration fails.</exception>
        Task<RegistrationResult> Register(string username, string password);

        /// <summary>
        /// Get an account for a user's credentials.
        /// </summary>
        /// <param name="username">The user's name.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The account for the user.</returns>
        /// <exception cref="UserNotFoundException">Thrown if the user does not exist.</exception>
        /// <exception cref="InvalidPasswordException">Thrown if the password is invalid.</exception>
        /// <exception cref="Exception">Thrown if the login fails.</exception>
        Task<Models.Customer> Login(string username, string password);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly ICustomerService _userService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationService(ICustomerService userService, IPasswordHasher passwordHasher)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        public async Task<Models.Customer> Login(string username, string password)
        {
            Models.Customer user = await _userService.GetByUsername(username);

            if (user == null)
            {
                throw new Exception("");
            }

            PasswordVerificationResult passwordResult = _passwordHasher.VerifyHashedPassword(user.PasswordHash, password);

            if (passwordResult != PasswordVerificationResult.Success)
            {
                throw new Exception("");
            }

            return user;
        }

        public async Task<RegistrationResult> Register(string username, string password)
        {
            RegistrationResult result = RegistrationResult.Success;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                result = RegistrationResult.EmptyInput;
            }

            Models.Customer usernameAccount = await _userService.GetByUsername(username);
            if (usernameAccount != null)
            {
                result = RegistrationResult.UsernameAlreadyExists;
            }

            if (result == RegistrationResult.Success)
            {
                string hashedPassword = _passwordHasher.HashPassword(password);

                Models.Customer user = new Models.Customer()
                {
                    Username = username,
                    PasswordHash = hashedPassword,
                    DatedJoined = DateTime.Now
                };

                await _userService.Create(user);
            }

            return result;
        }




    }




}

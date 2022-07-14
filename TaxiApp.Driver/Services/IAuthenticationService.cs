using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.SharedModels;
using TaxiApp.Db;

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
        /// <param name="username">Имя заказчика</param>
        /// <param name="password">Пароль заказчика</param>
        /// <returns>Результат попытки зарегистрировать нового заказчика</returns>
        Task<RegistrationResult> Register(string username, string password);

        /// <summary>
        /// Get an account for a user's credentials.
        /// </summary>
        /// <param name="username">Имя</param>
        /// <param name="password">Пароль</param>
        /// <returns>Данные заказчика</returns>
        /// <exception cref="Exception">Thrown if the login fails.</exception>
        Task<SharedModels.Driver> Login(string username, string password);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IDriverDbService _driverDbService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationService(IDriverDbService userService, IPasswordHasher passwordHasher)
        {
            _driverDbService = userService;
            _passwordHasher = passwordHasher;
        }

        public async Task<SharedModels.Driver> Login(string username, string password)
        {
            SharedModels.Driver driver = await _driverDbService.GetByUsername(username);

            if (driver == null)
                return null;

            PasswordVerificationResult passwordResult = _passwordHasher.VerifyHashedPassword(driver.passwordHash, password);
            if (passwordResult != PasswordVerificationResult.Success)
            {
                return null;
            }
            else
                return driver;
        }

        public async Task<RegistrationResult> Register(string username, string password)
        {
            RegistrationResult result = RegistrationResult.Success;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                result = RegistrationResult.EmptyInput;
            }

            SharedModels.Driver usernameAccount = await _driverDbService.GetByUsername(username);
            if (usernameAccount != null)
            {
                result = RegistrationResult.UsernameAlreadyExists;
            }

            if (result == RegistrationResult.Success)
            {
                string hashedPassword = _passwordHasher.HashPassword(password);

                SharedModels.Driver newDriver = new SharedModels.Driver()
                {
                    username = username,
                    passwordHash = hashedPassword,
                    datedJoined = DateTime.Now
                };

                await _driverDbService.Create(newDriver);
            }

            return result;
        }




    }




}

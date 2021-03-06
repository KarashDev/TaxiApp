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
        Task<RegistrationResult> Register(string username, string password);
        Task<SharedModels.Customer> Login(string username, string password);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly ICustomerDbService _customerDbService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationService(ICustomerDbService userService, IPasswordHasher passwordHasher)
        {
            _customerDbService = userService;
            _passwordHasher = passwordHasher;
        }

        public async Task<SharedModels.Customer> Login(string username, string password)
        {
            SharedModels.Customer customer = await _customerDbService.GetByUsername(username);

            if (customer == null)
                return null;

            PasswordVerificationResult passwordResult = _passwordHasher.VerifyHashedPassword(customer.passwordHash, password);
            if (passwordResult != PasswordVerificationResult.Success)
            {
                return null;
            }
            else
                return customer;
        }

        public async Task<RegistrationResult> Register(string username, string password)
        {
            RegistrationResult result = RegistrationResult.Success;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                result = RegistrationResult.EmptyInput;
            }

            SharedModels.Customer usernameAccount = await _customerDbService.GetByUsername(username);
            if (usernameAccount != null)
            {
                result = RegistrationResult.UsernameAlreadyExists;
            }

            if (result == RegistrationResult.Success)
            {
                string hashedPassword = _passwordHasher.HashPassword(password);

                SharedModels.Customer newCustomer = new SharedModels.Customer()
                {
                    username = username,
                    passwordHash = hashedPassword,
                    datedJoined = DateTime.Now
                };

                await _customerDbService.Create(newCustomer);
            }

            return result;
        }
    }
}

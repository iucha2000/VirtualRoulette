using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Application.Interfaces.Services;
using VirtualRoulette.Domain.Entities;

namespace VirtualRoulette.Infrastructure.Security
{
    //PasswordHashService to hash given password and verify its value
    public class PasswordHashService : IPasswordHashService
    {
        //Hash password with SHA256 algorythm
        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        //Verify if given password matches with given hashed value
        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var hashedProvidedPassword = HashPassword(providedPassword);
            return hashedPassword == hashedProvidedPassword;
        }
    }
}

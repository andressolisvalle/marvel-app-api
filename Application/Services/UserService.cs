using Application.DTOs;
using Application.Helpers;
using Application.Interface;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserSevice
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository, JwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return null; 
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return _jwtTokenGenerator.GenerateToken(user);
        }

        public async Task<bool> RegisterAsync(RegisterUserDto registerUserDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(registerUserDto.Email);
            if (existingUser != null)
            {
                return false;
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = registerUserDto.Name,
                Identification = registerUserDto.Identification,
                Email = registerUserDto.Email
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, registerUserDto.Password);

            await _userRepository.AddAsync(user);

            return true;

        }
    }
}

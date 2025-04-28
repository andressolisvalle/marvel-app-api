using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public  interface IUserSevice
    {
        Task<bool> RegisterAsync(RegisterUserDto registerUserDto);
        Task<string> LoginAsync(LoginDto loginDto);
    }
}

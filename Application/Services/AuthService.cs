using Application.DTOs;
using Application.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        public Task Login(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task LoginWithFaceBook()
        {
            throw new NotImplementedException();
        }

        public Task LoginWithGmail()
        {
            throw new NotImplementedException();
        }

        public Task LogOut()
        {
            throw new NotImplementedException();
        }
    }
}

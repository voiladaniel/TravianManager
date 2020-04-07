using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TravianManager.Core.Data;

namespace TravianManager.Core.Managers
{
    public interface IAccountManager
    {
        Task<bool> Login(User user);
    }
}

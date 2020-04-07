using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravianManager.Core.Data;

namespace TravianManager.Core.DataProvider
{
    public interface IUserDataProvider
    {
        Task<bool> Login(User user);
    }
}

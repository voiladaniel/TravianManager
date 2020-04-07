using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TravianManager.Core.Data;
using TravianManager.Core.DataProvider;
using TravianManager.Core.Managers;
using TravianManager.Data.Data;

namespace TravianManager.BusinessLogic.Managers
{
    public class AccountManager : IAccountManager
    {
        private readonly IAppSettingsPoco _appSettingsPoco;
        private readonly ILogger _logger;
        private readonly IUserDataProvider _userDataProvider;

        public AccountManager(IAppSettingsPoco appSettingsPoco, ILogger logger, IUserDataProvider userDataProvider)
        {
            _appSettingsPoco = appSettingsPoco;
            _logger = logger;
            _userDataProvider = userDataProvider;
        }

        public async Task<bool> Login(User user)
        {
            _logger.LogInformation("Starting Login");
            try
            {
                return await _userDataProvider.Login(user);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error Login");
                throw;
            }
        }
    }
}

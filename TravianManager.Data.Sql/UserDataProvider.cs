using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TravianManager.Core;
using TravianManager.Core.Context;
using TravianManager.Core.Data;
using TravianManager.Core.DataProvider;

namespace TravianManager.Data.Sql
{
    public class UserDataProvider : IUserDataProvider
    {
        private readonly IEntityFrameworkDbContext _entityFrameworkDbContext;

        private readonly IHelpers _helpers;
        /// <summary>
        /// The Logger.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// The Logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// /Initializes a new instance of the <see cref="AuthorizationDataProvider"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="connectionString">
        /// The Database context.
        /// </param>
        public UserDataProvider(ILogger logger, IEntityFrameworkDbContext entityFrameworkDbContext, IHelpers helpers)
        {
            this._logger = logger;
            this._entityFrameworkDbContext = entityFrameworkDbContext;
            this._helpers = helpers;
        }

        public async Task<bool> Login(User user)
        {

            using (var con = new SqlConnection(this._connectionString))
            {
                try
                {
                    var result =  _entityFrameworkDbContext.Set<User>() as IQueryable<User>;

                    var slectUser = result.Where(x => x.Password.Equals(_helpers.Encrypt(user.Password)) && x.Username.Equals(user.Username));

                    return slectUser.Count() > 0 ? true : false;
                }
                catch (Exception e)
                {
                    this._logger.LogError($"Getting the data from config.AuthorizationRoles on config database failed: {e.Message}.");
                    throw;
                }
            }
        }
    }
}

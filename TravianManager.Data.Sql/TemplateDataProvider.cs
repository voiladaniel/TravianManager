using Microsoft.EntityFrameworkCore;
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
    public class TemplateDataProvider : ITemplateDataProvider
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
        public TemplateDataProvider(ILogger logger, IEntityFrameworkDbContext entityFrameworkDbContext, IHelpers helpers)
        {
            this._logger = logger;
            this._entityFrameworkDbContext = entityFrameworkDbContext;
            this._helpers = helpers;
        }
        public bool AddDefender(Defender defender)
        {
                try
                {
                      _entityFrameworkDbContext.Set<Defender>().Add(defender);
                      _entityFrameworkDbContext.SaveChanges();

                    return true;
                }
                catch (Exception e)
                {
                    this._logger.LogError($"Getting the data from config.AuthorizationRoles on config database failed: {e.Message}.");
                    throw;
                }
        }

        public async Task DeleteDefender(int DefenderID)
        {
            try
            {
                var defender = _entityFrameworkDbContext.Set<Defender>().Where(x => x.DefenderID.Equals(DefenderID)).FirstOrDefault();
                _entityFrameworkDbContext.Set<Defender>().Remove(defender);
                await _entityFrameworkDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                this._logger.LogError($"Getting the data from config.AuthorizationRoles on config database failed: {e.Message}.");
                throw;
            }
        }

        public async Task UpdateAttacker(Attacker attacker)
        {
            try
            {
                var entity = _entityFrameworkDbContext.Set<Attacker>().Where(x => x.AttackerID.Equals(attacker.AttackerID)).FirstOrDefault();
                entity.NotBeforeTime = attacker.NotBeforeTime;
                entity.TroopSpeed = attacker.TroopSpeed;
                entity.TournamentSquare = attacker.TournamentSquare;

                await _entityFrameworkDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                this._logger.LogError($"Getting the data from config.AuthorizationRoles on config database failed: {e.Message}.");
                throw;
            }
        }

        public void UpdateDefender(Defender defender)
        {
            try
            {
                _entityFrameworkDbContext.Set<Defender>().Update(defender);
                _entityFrameworkDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                this._logger.LogError($"Getting the data from config.AuthorizationRoles on config database failed: {e.Message}.");
                throw;
            }
        }


        public async Task<IEnumerable<Attacker>> GetAttackers(int templateID, int userId)
        {
         
                try
                {
                    var attackers = _entityFrameworkDbContext.Set<Attacker>()
                        .Include(x => x.Account)
                        .Include(x => x.Defender).ThenInclude(x => x.Account)
                        .Where(x => x.TemplateID == templateID)
                        .ToListAsync();

                    return await attackers;
                }
                catch (Exception e)
                {
                    this._logger.LogError($"Getting the data from config.AuthorizationRoles on config database failed: {e.Message}.");
                    throw;
                }
        }

        public Attacker GetAttackerById(int templateID, int attackerID)
        {

            try
            {
                var attacker = _entityFrameworkDbContext.Set<Attacker>()
                    .Include(x => x.Account)
                    .Include(x => x.Defender).ThenInclude(x => x.Account)
                    .Where(x => x.TemplateID.Equals(templateID) && x.AttackerID.Equals(attackerID))
                    .FirstOrDefault();

                return attacker;
            }
            catch (Exception e)
            {
                this._logger.LogError($"Getting the data from config.AuthorizationRoles on config database failed: {e.Message}.");
                throw;
            }
        }

        public async Task<IEnumerable<Account>> GetDefenders(int templateID, int userId)
        {
                try
                {
                    var defenders = _entityFrameworkDbContext.Set<Account>()
                        .Where(x => x.AccountType.Equals(0))
                        .ToListAsync();

                    return await defenders;
                }
                catch (Exception e)
                {
                    this._logger.LogError($"Getting the data from config.AuthorizationRoles on config database failed: {e.Message}.");
                    throw;
                }
        }

        public Coordinate GetCoordinates(int accountID)
        {
            try
            {
                var account = _entityFrameworkDbContext.Set<Account>()
                    .Where(x => x.AccountID.Equals(accountID))
                    .FirstOrDefault();

                var coordinates = new Coordinate
                {
                    XCoordinate = Convert.ToInt32(account.XCoord),
                    YCoordinate = Convert.ToInt32(account.YCoord)
                };

                return coordinates;
            }
            catch (Exception e)
            {
                this._logger.LogError($"Getting the data from config.AuthorizationRoles on config database failed: {e.Message}.");
                throw;
            }
        }

        public int GetAccountIdByAttacker(int attackerID)
        {
            try
            {
                var account = _entityFrameworkDbContext.Set<Attacker>()
                    .Where(x => x.AttackerID.Equals(attackerID))
                    .Select(x => x.AccountID)
                    .FirstOrDefault();

                return account;
            }
            catch (Exception e)
            {
                this._logger.LogError($"Getting the data from config.AuthorizationRoles on config database failed: {e.Message}.");
                throw;
            }
        }

        public Attacker GetAttacker(int attackerID)
        {
            try
            {
                var account = _entityFrameworkDbContext.Set<Attacker>()
                    .Include(x => x.Account)
                    .Include(x => x.Defender).ThenInclude(x => x.Account)
                    .Where(x => x.AttackerID.Equals(attackerID))
                    .FirstOrDefault();

                return account;
            }
            catch (Exception e)
            {
                this._logger.LogError($"Getting the data from config.AuthorizationRoles on config database failed: {e.Message}.");
                throw;
            }
        }
    }
}

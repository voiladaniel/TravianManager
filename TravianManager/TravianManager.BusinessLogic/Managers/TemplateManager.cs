using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TravianManager.Core;
using TravianManager.Core.Data;
using TravianManager.Core.DataProvider;
using TravianManager.Core.Managers;
using TravianManager.Data.Data;

namespace TravianManager.BusinessLogic.Managers
{
    public class TemplateManager : ITemplateManager
    {
        private readonly ICalculator _calculator;
        private readonly ILogger _logger;
        private readonly ITemplateDataProvider _templateDataProvider;

        public TemplateManager(ICalculator calculator, ILogger logger, ITemplateDataProvider templateDataProvider)
        {
            _calculator = calculator;
            _logger = logger;
            _templateDataProvider = templateDataProvider;
        }

        public async Task<IEnumerable<Attacker>> GetAttackers(int templateId, int userId)
        {
            _logger.LogInformation("Starting Login");
            try
            {
                return await _templateDataProvider.GetAttackers(templateId, userId);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error Login");
                throw;
            }
        }

        public async Task<IEnumerable<Account>> GetDefenders(int templateId, int userId)
        {
            _logger.LogInformation("Starting Login");
            try
            {
                return await _templateDataProvider.GetDefenders(templateId, userId);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error Login");
                throw;
            }
        }

        public async Task<bool> AddDefender(Defender account)
        {
            _logger.LogInformation("Starting Login");
            try
            {
               
                return _templateDataProvider.AddDefender(account);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error Login");
                throw;
            }
        }

        public async Task UpdateAttacker(Attacker attacker)
        {
            _logger.LogInformation("Starting Login");
            try
            {

                await _templateDataProvider.UpdateAttacker(attacker);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error Login");
                throw;
            }
        }

        public async Task DeleteDefender(int DefenderID)
        {
            _logger.LogInformation("Starting Login");
            try
            {

                await _templateDataProvider.DeleteDefender(DefenderID);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error Login");
                throw;
            }
        }
    }
}

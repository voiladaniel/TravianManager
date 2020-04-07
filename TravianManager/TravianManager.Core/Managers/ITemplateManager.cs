using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TravianManager.Core.Data;

namespace TravianManager.Core.Managers
{
    public interface ITemplateManager
    {
        Task<IEnumerable<Attacker>> GetAttackers(int templateId, int userId);

        Task<IEnumerable<Account>> GetDefenders(int templateId, int userId);

        Task<bool> AddDefender(Defender account);

        Task DeleteDefender(int DefenderID);

        Task UpdateAttacker(Attacker attacker);
    }
}

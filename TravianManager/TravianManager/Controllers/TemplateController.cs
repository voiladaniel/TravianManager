using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TravianManager.Core;
using TravianManager.Core.Data;
using TravianManager.Core.Managers;

namespace TravianManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly ICalculator _calculator;
        private readonly ITemplateManager _templateManager;
        private readonly ILogger _logger;
        public TemplateController(ITemplateManager templateManager, ICalculator calculator, ILogger logger)
        {
            _calculator = calculator;
            _templateManager = templateManager;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAttackers")]
        public async Task<ActionResult> GetAttackers([FromQuery] int TemplateID, [FromQuery] int UserID)
        {
            try
            {
                var attackers = await _templateManager.GetAttackers(TemplateID, UserID);

                return Ok(attackers);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"ERROR ERROR ERROR ---> Error while trying to login, for user: ", ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetDefenders")]
        public async Task<ActionResult> GetDefenders([FromQuery] int TemplateID, [FromQuery] int UserID)
        {
            try
            {
                var attackers = await _templateManager.GetDefenders(TemplateID, UserID);

                return Ok(attackers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR ERROR ERROR ---> Error while trying to login, for user: ", ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("AddDefender")]
        public async Task<ActionResult> AddDefender([FromBody] Defender defender)
        {
            try
            {
                var attackers = await _templateManager.AddDefender(defender);
                _calculator.RefreshDataPerAttacker(defender.AttackerID);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR ERROR ERROR ---> Error while trying to login, for user: ", ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("UpdateAttacker")]
        public async Task<ActionResult> UpdateAttacker([FromBody] Attacker attacker)
        {
            try
            {
                await _templateManager.UpdateAttacker(attacker);
                _calculator.RefreshDataPerAttacker(attacker.AttackerID);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR ERROR ERROR ---> Error while trying to login, for user: ", ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("DeleteDefender")]
        public async Task<ActionResult> DeleteDefender([FromQuery] int DefenderID, [FromQuery] int AttackerID)
        {
            try
            {
                await _templateManager.DeleteDefender(DefenderID);
                _calculator.RefreshDataPerAttacker(AttackerID);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR ERROR ERROR ---> Error while trying to login, for user: ", ex.Message);
                return BadRequest(ex);
            }
        }
    }


}
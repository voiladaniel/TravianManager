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
    public class AccountController : ControllerBase
    {
        private readonly IAccountManager _accountManager;
        private readonly ILogger _logger;
        public AccountController(IAccountManager accountManager, ILogger logger)
        {

            _accountManager = accountManager;
            _logger = logger;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login([FromBody] User user)
        {
            try
            {
                var loginResult = await _accountManager.Login(user);

                if (!loginResult)
                    return Unauthorized();

                return Ok();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"ERROR ERROR ERROR ---> Error while trying to login, for user: {user.Username}", ex.Message);
                return BadRequest(ex);
            }
        }
    }
}
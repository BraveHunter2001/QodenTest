using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp
{
    // TODO 4: unauthorized users should receive 401 status code
    //  added a tag that is not unauthorized user
    [UnAuthorizationFilter]
    [Authorize]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize] 
        [HttpGet]
        public ValueTask<Account> Get()
        {
            var userId = User.Claims.FirstOrDefault(claim =>claim.Type == ClaimTypes.NameIdentifier).Value;
            return _accountService.LoadOrCreateAsync(userId /* TODO 3: Get user id from cookie */);
            
        }

        //TODO 5: Endpoint should works only for users with "Admin" Role
        [Authorize(Roles = "Admin")]
        [RoleAuthorizationFilter("Admin")]
        [HttpGet("{id}")]
        public Account GetByInternalId([FromRoute] int id)
        {
            return _accountService.GetFromCache(id);
        }

        [Authorize]
        [HttpPost("counter")]
        public async Task UpdateAccount()
        {
            //Update account in cache, don't bother saving to DB, this is not an objective of this task.
            var account = await Get();
            //account.Counter++;

            //TODO 6 ANSWER :
            //First, the getFromCache(id) function gets the data from AccountCache.
            //So we need to change data exactly in cache.
            //Basically, this is what it says in the comment above.

            //Second, the Get function retrieves data from the database and clones this object.
            //That's why even if we tried to change the database data using the "account.Counter++" line,
            //we wouldn't succeed. Since this field is already being changed from a cloned object.

            // So the solution is this. Use the Get function to get the current account.
            //Then use its InternalId to search for this account in the cache.
            //And in it already change the Counter field.And it will work, because we get object from cache by reference.
            
            var accountIntoCache = _accountService.GetFromCache(account.InternalId);
            accountIntoCache.Counter++;
        }
    }
}
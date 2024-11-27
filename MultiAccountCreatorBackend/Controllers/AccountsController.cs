using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiAccountCreatorBackend.Data;
using MultiAccountCreatorBackend.Data.DTO;

namespace MultiAccountCreatorBackend.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AccountsController(AccountsDataContext context) : Controller
    {
        [HttpGet]
        public IActionResult GetAllAccounts()
        {
            var accounts = context.Accounts.Include(acc => acc.Service);
            return Ok(accounts.Select(acc => new AccountDTO() { Name = acc.Name, Email = acc.Email, ServiceName = acc.Service != null ? acc.Service.Name : "" }));
        }

        [HttpGet("connections")]
        public async Task<IActionResult> GetConnections(string accountEmail)
        {
            var services = await context.Accounts
                            .Include(acc => acc.Service)
                            .Where(acc => acc.Email == accountEmail && acc.Service != null).ToListAsync();

            if(services.Count == 0)
                return NotFound();

            return Ok(services.Select(acc => acc.Service.Name).Distinct());
        }

        [HttpPost]
        public async Task<IActionResult> AddAccount(AccountDTO createAccountDTO)
        {
            Account acc = new() { Name = createAccountDTO.Name, Email = createAccountDTO.Email };
            if(createAccountDTO.ServiceName == null)
            {
                context.Accounts.Add(acc);
                await context.SaveChangesAsync();
                return Ok(new AccountCreatedDTO() { Name = acc.Name });
            }

            Service serviceFound = context.Services
                .Include(serv => serv.Accounts)
                .FirstOrDefault(service => service.Name == createAccountDTO.ServiceName);
            if(serviceFound == null)
                return BadRequest();

            acc.Service = serviceFound;
            Account accountFound = serviceFound.Accounts
                .FirstOrDefault(acc => acc.Name == createAccountDTO.Name &&
                                    acc.Email == createAccountDTO.Email &&
                                    acc.Service.Name == createAccountDTO.ServiceName);
            if (accountFound != null)
                return BadRequest();

            context.Accounts.Add(acc);
            await context.SaveChangesAsync();
            return Ok(new AccountCreatedDTO() { Name=acc.Name });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount(AccountDTO accountDTO)
        {
            List<Account> foundAccounts = await context.Accounts.Include(acc => acc.Service)
                .Where(acc => acc.Name == accountDTO.Name && acc.Email == accountDTO.Email)
                .ToListAsync();
            if (foundAccounts.Count == 0)
                return NotFound();

            foreach (Account account in foundAccounts)
                if(account.Service?.Name == accountDTO.ServiceName)
                    context.Remove(account);

            await context.SaveChangesAsync();
            return Ok();
        }
    }
}

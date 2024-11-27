using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiAccountCreatorBackend.Data;
using MultiAccountCreatorBackend.Data.DTO;

namespace MultiAccountCreatorBackend.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ServicesController(AccountsDataContext context) : Controller
    {
        [HttpGet]
        public IActionResult GetServices()
        {
            var services = context.Services.Include(ser => ser.Accounts).ToList();
            return Ok(services
                .Select(serv => new ServiceDTO() {
                    Name = serv.Name, Accounts = serv.Accounts
                                                    .Select(acc => new AccountDTO() { Email = acc.Email, Name = acc.Name }).ToList()}));
        }

        [HttpPost]
        public async Task<IActionResult> AddService(CreateServiceDTO createServiceDTO)
        {
            Service service = new() { Name = createServiceDTO.Name };
            context.Services.Add(service);
            await context.SaveChangesAsync();
            return Ok(new ServiceDTO() { Name = service.Name });
        }
    }
}

using ChurchWebApp.Data;
using ChurchWebApp.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChurchWebApp.Controllers
{
    [Authorize(AuthenticationSchemes =  JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : Controller
    {
        private readonly ApiDbContext _context;

        public EventController(ApiDbContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllEvents")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Events.ToListAsync());
        }

        [HttpPost]
        [Route("AddNewEvent")]
       public async Task<IActionResult> AddNewEvent(Event newEvent){
            _context.Events.Add(newEvent);

            await _context.SaveChangesAsync();
            return Ok(new {Success=true, msg="Event Added Succesfully"});
        
        }
        [HttpDelete]
        [Route("DeleteAnEvent")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var singleEvent =await _context.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (singleEvent ==null) {
                return NotFound("Event Not Found");

            }
             _context.Events.Remove(singleEvent);
            await _context.SaveChangesAsync();
            return Ok("Event Deleted");
        }
      
    }
}

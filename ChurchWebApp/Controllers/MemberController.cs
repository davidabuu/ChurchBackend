using ChurchWebApp.Data;
using ChurchWebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayStack.Net;

namespace ChurchWebApp.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : Controller
    {
        private readonly ApiDbContext _context;
        private PayStackApi PayStackApi { get; set; }
        private readonly IConfiguration _configuration;
        private readonly string token;
        public MemberController(ApiDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            token = _configuration["Payment:PaystackSdk"]!;
            PayStackApi = new PayStackApi(token);

        }

        [HttpGet]
        [Route("GetAllMembers")]
        public IActionResult Get()
        {
            var allMembers =  _context.Members.Where(x => x.Paid).ToList();
            return Ok(allMembers);
        }

        [HttpPost]
        [Route("AddNewMember")]
        public async Task<IActionResult> AddNewEvent(MembersRegistration membersRegistration) {
            
            TransactionInitializeRequest request = new()
            {
                AmountInKobo = 200 * 100,
                Email = membersRegistration.Email,
                Reference = Generate().ToString(),
               Bearer = token,
                Currency = "NGN",
                CallbackUrl = "http://localhost:27362/members/verify"

            };

            TransactionInitializeResponse response = PayStackApi.Transactions.Initialize(request);

            if (response.Status)
            {
                var member = new MembersRegistration()
                {
                    FirstName = membersRegistration.FirstName,
                    LastName = membersRegistration.LastName,
                    Email = membersRegistration.Email,
                    PhoneNumber = membersRegistration.PhoneNumber,
                    Organization = membersRegistration.Organization,
                    Sex = membersRegistration.Sex,
                    Age = membersRegistration.Age,
                    TnxRef = request.Reference
                };
               _context.Members.Add(member);
                await _context.SaveChangesAsync();
                Redirect(response.Data.AuthorizationUrl);
                return Ok(new {Success=true, msg="Member Added Succesfully", data=response});
            }
            return BadRequest(response.Message);


        }
        [HttpGet]
        [Route("Verify")]

        public async  Task<IActionResult> Verify(string response) {

            TransactionVerifyResponse verifyResponse = PayStackApi.Transactions.Verify(response);
            if(verifyResponse.Data.Status == "success")
            {
                var memberTransaction = await _context.Members.Where(x => x.TnxRef == response).FirstOrDefaultAsync();
                if(memberTransaction != null)
                {
                    memberTransaction.Paid = true;
                    _context.Members.Update(memberTransaction);
                    await _context.SaveChangesAsync();
                    return Ok("Success");
                }
                return BadRequest(verifyResponse.Data.GatewayResponse);
            }
            return Ok(verifyResponse);
        }
        [HttpDelete]
        [Route("DeleteAMember")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var singleMember = await _context.Members.FirstOrDefaultAsync(x => x.MemberId == id);
            if (singleMember == null) {
                return NotFound("Member Not Found");

            }
            _context.Members.Remove(singleMember);
            await _context.SaveChangesAsync();
            return Ok("Success");

        }
        public static  int Generate()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            return rand.Next(100000000, 999999999);
        }
    }
}

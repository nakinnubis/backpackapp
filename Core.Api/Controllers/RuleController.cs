using System.Linq;
using Core.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Api.Controllers
{

    [Produces("application/json")]
    [Route("api/Rule")]
    [ApiController]
    public class RuleController : ControllerBase
    {
        AppDbContext db = new AppDbContext();

        [HttpPost]
        [Authorize]
        [Route("CreateRule")]
        public IActionResult CreateRule([FromBody] Rule rule)
        {

            if (rule.description != null)
            {
                if (HttpContext.User.FindFirst("userId") != null)
                {
                    int userid = int.Parse(HttpContext.User.FindFirst("userId").Value);
                    rule.add_by = userid;
                    rule.isdeleted = false;
                    db.Rule.Add(rule);
                    db.SaveChanges();
                    return Ok(new { Rule_id = rule.id });
                }
            }
            return BadRequest(new { message = "Please,Check Data" });
        }

        [Authorize]
        [Route("GetAllRules")]
        public IActionResult GetAllRules()  //***
        {
            int userid = int.Parse(HttpContext.User.FindFirst("userId").Value);

            var Rules = db.Rule.Where(x=> (x.add_by == userid | x.add_by == null) & x.isdeleted == false).
                                          Select(x => new { x.id,
                                                            x.description,
                                                            intial = x.add_by == null ? true : false }).ToList();
            if (Rules != null)
                return Ok(Rules);
            else
                return NoContent();
        }


        [Route("DeleteRule")]
        [Authorize]
        public IActionResult DeleteRule(int id)
        {
            var Rule = db.Rule.Find(id);
            if (Rule == null)
            {
                return Ok(new { status = 0 });
            }
            int userid = int.Parse(HttpContext.User.FindFirst("userId").Value);
            if (Rule.add_by != userid)
                return Ok(new { status = 0, message = "Canot delete this rule" });

            Rule.isdeleted = true;
            db.SaveChanges();
            return Ok(new { status = 1 });
        }
    }
}

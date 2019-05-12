﻿using System.Linq;
using Core.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Core.Api.Controllers
{

    [Produces("application/json")]
    [Route("api/Add_ons")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class Add_ons : ControllerBase
    {
        AppDbContext db = new AppDbContext();

        [HttpPost]
        [Authorize]
        [Route("CreateAdd_Ons")]
        public IActionResult CreateAdd_Ons([FromBody]Add_Ons add_Ons)
        {
            if (add_Ons != null)
            {         
                if (string.IsNullOrEmpty(add_Ons.name) | string.IsNullOrWhiteSpace(add_Ons.name))
                    return Ok(new { status = 0 ,message= "Please, Insert addon name" });

                int userid = int.Parse(HttpContext.User.FindFirst("userId").Value);
                add_Ons.add_by = userid;
                add_Ons.isdeleted = false;
                db.Add_Ons.Add(add_Ons);
                db.SaveChanges();
                return Ok(new { Add_Ons_id = + add_Ons.id });
            }
            return BadRequest(new { message = "Check Data ,Please" });
        }
        
        [Authorize]
        [Route("GetAllAdd_Ons")]
        public IActionResult GetAllAdd_Ons()
        {
            int userid = int.Parse(this.HttpContext.User.FindFirst("userId").Value);
            var list1 = this.db.Add_Ons.Where(c => c.add_by.Value == userid && c.isdeleted == (bool?)false).Select(x => new
            {
                id = x.id,
                name = x.name,
                icon = x.icon,
                definedby = string.Format("{0}", (object)userid),
                intial = true
            }).ToList();
            var list2 = this.db.Add_Ons.Where(x => (x.add_by == (int?)userid | x.add_by == new int?()) & x.isdeleted == (bool?)false).Select(x => new
            {
                id = x.id,
                name = x.name,
                icon = x.icon,
                definedby = "others",
                intial = x.add_by == new int?() ? true : false
            }).ToList();
            list1.Concat(list2);
            if (list2 != null)
                return Ok(list2);
            return NoContent();
            //int userid = int.Parse(HttpContext.User.FindFirst("userId").Value);

            //var Add_Ons = db.Add_Ons.Where(x=>(x.add_by==userid|x.add_by==null)&x.isdeleted==false).Select(x => new
            //              { x.id,
            //                x.name,
            //                x.icon,
            //                intial =x.add_by==null?true:false}).ToList();
            //if (Add_Ons != null)
            //    return Ok(Add_Ons);
            //else
            //    return NoContent();
        }


        [Route("DeleteAddons")]
        [Authorize]
        public IActionResult DeleteAddons(int id)
        {
            var Addons = db.Add_Ons.Find(id);
            if (Addons == null)
            {
                return Ok(new { status = 0 });
            }
            int userid = int.Parse(HttpContext.User.FindFirst("userId").Value);
            if (Addons.add_by != userid)
                return Ok(new { status = 0 ,message="Canot delete this addons"});

            Addons.isdeleted = true;
            db.SaveChanges();
            return Ok(new { status = 1 });
        }
    }
}

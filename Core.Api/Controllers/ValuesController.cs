using System;
using System.Collections.Generic;
using System.Linq;
using Core.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        AppDbContext db = new AppDbContext();
        private readonly IHostingEnvironment _hostingEnvironment;
  

        public ValuesController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Route("GetActivityTypes")]
        public IActionResult GetActivityTypes()
        {
            try
            {
                var activityType = db.ActivityType.Select(x => new { x.id, x.Name, x.url }).ToList();
                string webRootPath = _hostingEnvironment.WebRootPath;
                string contentRootPath = _hostingEnvironment.ContentRootPath;
                if (activityType != null)
                    return Ok(new { activityType, webRootPath, contentRootPath });
                else
                    return NoContent();
            }
            catch (Exception e) {
                return BadRequest(e.Message);

            }
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            try
            {

               var activityType = db.ActivityType.Select(x => new { x.id, x.Name, x.url }).ToList();
              //  string webRootPath = _hostingEnvironment.WebRootPath;
              //  string contentRootPath = _hostingEnvironment.ContentRootPath;
              //  string ApplicationName = _hostingEnvironment.ApplicationName;
              //string  EnvironmentName = _hostingEnvironment.EnvironmentName;
                if (activityType != null)
                    return Ok(new { activityType});
                else
                    return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

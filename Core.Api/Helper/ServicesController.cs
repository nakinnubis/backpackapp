using System.Collections.Generic;
using System.Linq;
using Core.Api.Models;
using Core.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        AppDbContext db = new AppDbContext();

        [HttpGet]
        
        [Route("GetLookUp")]
        public IActionResult GetLookUp(string lookUp)  //***
        {
            // IBusinessLayer businessLayer = new BuinessLayer();
            //businessLayer.GetAllNationalities()
            if (lookUp != null)
            {
                switch (lookUp)
                {
                    case "Nationality":
                        return Ok(db.Nationalities.Select(x => new
                        {
                            x.nationality_id,
                            x.nationality_name_ar,
                            x.nationality_name_en
                        }));

                    case "Identification_types":
                        return Ok(db.Identification_types.Select(x => new
                        {
                            x.identification_type_id,
                            x.identification_type_ar,
                            x.identification_type_en
                        }));
                    case "Diseases":
                        return Ok(db.Diseases.Select(x => new
                        {
                            x.diseases_id,
                            x.diseases_name_ar,
                            x.diseases_name_en
                        }));
                    default:
                        return Ok(new { status = 0, message = "Plz, check lookUp name" });
                }
            }
            else
                return BadRequest();

        }


    }

    public interface IBusinessLayer
    {
        IList<Nationalities> GetAllNationalities();

    }



    public class BuinessLayer : IBusinessLayer
    {
        private readonly INationalityRepository _natiRepository;


        public BuinessLayer()
        {
            _natiRepository = new NationalityRepository();

        }

        public BuinessLayer(INationalityRepository natiRepository)
        {
            _natiRepository = natiRepository;

        }

        public IList<Nationalities> GetAllNationalities()
        {
            return _natiRepository.GetAll();
        }

    }


}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie_Web_Api.Models;
using Movie_Web_Api.Repository;
using System.Collections.Generic;
using Movie_Web_Api.Dto;
using AutoMapper;

namespace Movie_Web_Api.Controllers
{
    // [Authorize(Roles = UserRoles.Admin)]
    [ApiController]
    [Route("api/[controller]")]
    public class ProducersController : ControllerBase
    {
       private readonly IProducersRepository producersRepository;
        private readonly IMapper imapper;
        //  private AppDbContext dbContext;
        public ProducersController(IProducersRepository producersRepository, IMapper mapper)
        {
            //   this.dbContext = context;
            this.imapper = mapper;
            this.producersRepository = producersRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<Producer>> GetAllProducer()
        {
            var allproducers = producersRepository.GetAll();
            return Ok(allproducers);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<Producer> GetProducer(int id)
        {
            var ProducerDetails = producersRepository.GetByID(id);
            if (ProducerDetails == null)
            {
                return NotFound();
            }
            return Ok(ProducerDetails);
        }

        [HttpPost]

        public IActionResult Create([Bind("FullName,ProfilePictureURL,Bio")] ProducerDto producerdto)
        {
            var producer = imapper.Map<Producer>(producerdto);
            producersRepository.Insert(producer);
            return Ok("Created");


        }


        [HttpPut("update/{id}")]
        public IActionResult Update(int id, ProducerDto producerdto)
        {
            if (id != producerdto.Id)
                return BadRequest("Update not allowed");

            var Producerfromdb = producersRepository.GetByActId(id);
            if (Producerfromdb == null)
                return BadRequest("Update not allowed");

            var producer = imapper.Map(producerdto, Producerfromdb);


            producersRepository.Edit(id, producer);
            return StatusCode(200);
        }


        [HttpDelete("{id}")]
        public ActionResult DeleteProducer(int id)
        {
            var producerDetails = producersRepository.GetByID(id);
            if (producerDetails == null)
            {
                return NotFound();
            }

            producersRepository.Delete(id);

            return NoContent();
        }
    }
}

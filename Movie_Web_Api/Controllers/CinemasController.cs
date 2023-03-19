using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie_Web_Api.Models;
using Movie_Web_Api.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Movie_Web_Api.Dto;
using AutoMapper;
namespace Movie_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemasController : ControllerBase
    {
        ICinamasReprosatory cinemasReprosatory;
        IMapper mapper;
        public CinemasController(ICinamasReprosatory _cinemaReposatory, IMapper mapper) //(AppDbContext context)
        {
            //this.context = context;
            cinemasReprosatory = _cinemaReposatory;
            this.mapper = mapper;
        }
        //  [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            // var allActor = context.Actors.ToList();
            // var allActors = actorReposatory.GetAll();
            return Ok(cinemasReprosatory.GetAll());
            //  return Ok(context.Actors.ToList());
            // Actors.ToList());
            //View(allActors);
        }
        [HttpPost]

        public IActionResult Create([Bind("Name,Logo,Description")] CenimaDto cenimadto)
        {
            var cenima = mapper.Map<Cinema>(cenimadto);
            cinemasReprosatory.Insert(cenima);

            return Ok("Created");

        }

       

        [HttpPut("update/{id}")]
        public IActionResult Update(int id, CenimaDto cenimaDto)
        {
            if (id != cenimaDto.Id)
                return BadRequest("Update not allowed");

            //  var actorfromdb = dbContext.Actors.FirstOrDefault(a=>a.Id==id);
            var cenimafromdb = cinemasReprosatory.GetByCeID(id);
            if (cenimafromdb == null)
                return BadRequest("Update not allowed");

            var actor = mapper.Map(cenimaDto, cenimafromdb);


            cinemasReprosatory.Edit(id, actor);
            return StatusCode(200);
        }

      

        //Get: Actors/Delete/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            Cinema c = cinemasReprosatory.GetByCeID(id);
            if (c == null)
            {
                return NotFound("Data not Found");
            }
            else
            {
                try
                {
                    cinemasReprosatory.Delete(id);

                    return StatusCode(StatusCodes.Status204NoContent);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}

    

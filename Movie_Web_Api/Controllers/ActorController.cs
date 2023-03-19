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
    public class ActorsController : ControllerBase
    {
        private readonly IActorReposatory actorReposatory;
        private readonly IMapper imapper;
      //  private AppDbContext dbContext;
        public ActorsController(IActorReposatory actorReposatory,IMapper mapper)
        {
         //   this.dbContext = context;
            this.imapper = mapper;
            this.actorReposatory = actorReposatory;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<Actor>> GetAllActors()
        {
            var allActors = actorReposatory.GetAll();
            return Ok(allActors);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<Actor> GetActor(int id)
        {
            var actorDetails = actorReposatory.GetByID(id);
            if (actorDetails == null)
            {
                return NotFound();
            }
            return Ok(actorDetails);
        }

        [HttpPost]

        public IActionResult Create([Bind("FullName,ProfilePictureURL,Bio")] ActorDto actordto)
        {
            var actor = imapper.Map<Actor>(actordto);
           actorReposatory.Insert(actor);
         //  await dbContext.SaveChangesAsync();
           // var actorDtoResult = imapper.Map<ActorDto>(actor);
            // return CreatedAtAction(nameof(Get), new { id = actorDtoResult.Id }, actorDtoResult);
            return Ok("Created");

        }











        //public IActionResult Create([Bind("FullName,ProfilePictureURL,Bio")] Actor actor)
        //{
        //    if (actor.FullName == null & actor.ProfilePictureURL == null & actor.Bio == null) return BadRequest("Full Name and Bio are required fields.");
        //    //actor.Actors_Movies = null;

        //    //    var newActor = new Actor
        //    //    {
        //    //        ProfilePictureURL = "https://example.com/profile-picture.jpg",
        //    //        FullName = "John Doe",
        //    //        Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
        //    //        Actors_Movies = new Actor_Movie { " MovieId:2", "ActorId:3" }

        //    //};


        //    actorReposatory.Insert(actor);
        //    return Ok("created");
        //}



        //public ActionResult<Actor> CreateActor(Actor actor)
        //{
        //    if (actor.FullName == null || actor.Bio == null )
        //    {
        //        return BadRequest("Full Name and Bio are required fields.");
        //    }

        //if (image != null && image.Length > 0)
        //{
        //    var fileName = System.IO.Path.GetFileName(image.FileName);
        //    var filePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot\\Images\\", fileName);

        //    using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
        //    {
        //        image.CopyTo(fileStream);
        //    }

        //    actor.ProfilePictureURL = "/Images/" + fileName;
        //}

        //    actorReposatory.Insert(actor);

        //    return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, actor);
        //}


        [HttpPut("update/{id}")]
        public IActionResult Update(int id, ActorDto actorDto)
        {
            if (id != actorDto.Id)
                return BadRequest("Update not allowed");

          //  var actorfromdb = dbContext.Actors.FirstOrDefault(a=>a.Id==id);
            var actorfromdb =actorReposatory.GetByActId(id);
            if (actorfromdb == null)
                return BadRequest("Update not allowed");

            //  actorfromdb.LastUpdatedBy = 1;
            //  actorfromdb.LastUpdatedOn = DateTime.Now;
            var actor = imapper.Map(actorDto, actorfromdb);
            //imapper.Map<Actor>(actorfromdb);
            //  mapper.Map(ActorDto, actorfromdb);

            actorReposatory.Edit(id, actor);
            return StatusCode(200);
        }

        //public async Task<IActionResult> UpdateCity(int id, CityDto cityDto)
        //{
        //    if (id != cityDto.Id)
        //        return BadRequest("Update not allowed");

        //    var cityFromDb = await uow.CityRepository.FindCity(id);

        //    if (cityFromDb == null)
        //        return BadRequest("Update not allowed");

        //    cityFromDb.LastUpdatedBy = 1;
        //    cityFromDb.LastUpdatedOn = DateTime.Now;
        //    mapper.Map(cityDto, cityFromDb);

        //    await uow.SaveAsync();
        //    return StatusCode(200);
        //}



        //[HttpPut("{id}")]
        //public ActionResult UpdateActor(int id,  Actor actor,IFormFile image)
        //{
        //    if (actor.FullName == null || actor.Bio == null)
        //    {
        //        return BadRequest("Full Name and Bio are required fields.");
        //    }

        //    var existingActor = actorReposatory.GetByID(id);
        //    if (existingActor == null)
        //    {
        //        return NotFound();
        //    }

        //    if (image != null && image.Length > 0)
        //    {
        //        var fileName = System.IO.Path.GetFileName(image.FileName);
        //        var filePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot\\Images\\", fileName);

        //        using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
        //        {
        //            image.CopyTo(fileStream);
        //        }

        //        actor.ProfilePictureURL = "/Images/" + fileName;
        //    }
        //    else
        //    {
        //        actor.ProfilePictureURL = existingActor.ProfilePictureURL;
        //    }

        //    actorReposatory.Edit(id, actor);

        //    return NoContent();
        //}

        [HttpDelete("{id}")]
        public ActionResult DeleteActor(int id)
        {
            var actorDetails = actorReposatory.GetByID(id);
            if (actorDetails == null)
            {
                return NotFound();
            }

            actorReposatory.Delete(id);

            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie_Web_Api.Repository;
using Movie_Web_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using Movie_Web_Api.Dto;
using Microsoft.EntityFrameworkCore;

namespace Movie_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        IMovieRepository moviesRepository;
        IMapper imapper;
        AppDbContext _context;
        public MoviesController(IMovieRepository _movieRepo,IMapper mapper, AppDbContext context)
        {
            imapper = mapper;
            moviesRepository = _movieRepo;
            _context = context;
        }



        [HttpGet]
        public IActionResult GetAllMovies()
        {
            var allMovies = moviesRepository.GetAllAsync();
            return Ok(allMovies);
        }


        [HttpGet("filter")]
        public IActionResult Filter(string searchString)
        {
            var allMovies = moviesRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResultNew = allMovies.Where(n => string.Equals(n.Name, searchString, System.StringComparison.CurrentCultureIgnoreCase) || string.Equals(n.Description, searchString, System.StringComparison.CurrentCultureIgnoreCase)).ToList();

                return Ok(filteredResultNew);
            }

            return Ok(allMovies);
        }



        [HttpGet("{id}")]

        public IActionResult Details(int id)
        {
            var movieDetail = moviesRepository.GetMovieById(id);

            return Ok(movieDetail);
        }


        [HttpPost]
        public async Task<ActionResult> CreateMovie([FromBody] MoviesDto movieDto)
        {
            moviesRepository.AddNewMovieAsync(movieDto);
            return Ok("Created");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, MoviesDto movieDto)
        {

            moviesRepository.UpdateMovieAsync(id, movieDto);
            return Ok("Updated");
        }


        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {
            moviesRepository.DeleteMovie(id);
            return NoContent();
        }





    }
}

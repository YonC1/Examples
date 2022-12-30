using Cyon_WebAPI_Project_4.Data;
using Cyon_WebAPI_Project_4.Data_Transfer;
using Cyon_WebAPI_Project_4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cyon_WebAPI_Project_4.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        /// <summary>
        /// GET /api/v1/movies/<key>
        /// Returns all movies in the database
        /// </summary>
        /// <param name="movieID"></param>
        /// <returns>TBD</returns>
        [HttpGet("{key}")]
        public ActionResult<List<MovieDto>> GetAllMovies(string key)
        {
            List<Movie> movies = new List<Movie>();
            DataLayer dl = new DataLayer();
            var results = dl.GetAUserByIDAsync(key);
            if (results.Result != null)
            {
                Task<List<Movie>> appMovies = dl.GetAllMoviesAsync();

                List<MovieDto> moviesDto = new List<MovieDto>();
                foreach (Movie movie in appMovies.Result)
                {
                    MovieDto movieDto = new MovieDto();
                    movieDto.Title = movie.Title;
                    movieDto.PostDate = movie.PostDate;
                    movieDto.Summary = movie.Summary;
                    movieDto.Link = movie.Link;
                    movieDto.ValidMovieDto = true;
                    moviesDto.Add(movieDto);
                }

                movies = appMovies.Result;

                return Ok(moviesDto);
            }
            else
            {
                return BadRequest("The user key in not valid.");
            }

        } // end GetAllMovies

        /// <summary>
        /// Get all movies with a n-rating or greater
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("Ratings/{key}")]
        public ActionResult<List<MovieWithRatingDto>> GetAllMoviesWithRatings(string key)
        {
            List<MovieWithRatingDto> movies = new List<MovieWithRatingDto>();
            DataLayer dl = new DataLayer();
            var results = dl.GetAUserByIDAsync(key);
            if (results.Result != null)
            {
                Task<List<MovieWithRatingDto>> appMovies = dl.GetAllMoviesWithAveRating();
                movies = appMovies.Result;
                return Ok(movies);
            }
            else
            {
                return BadRequest("The user key in not valid.");
            }

        }
    }
}

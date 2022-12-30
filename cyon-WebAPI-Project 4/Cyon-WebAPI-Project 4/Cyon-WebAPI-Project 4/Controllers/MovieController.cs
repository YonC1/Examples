using Cyon_WebAPI_Project_4.Data;
using Cyon_WebAPI_Project_4.Data_Transfer;
using Cyon_WebAPI_Project_4.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cyon_WebAPI_Project_4.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {

        /// <summary>
        /// GET /api/v1/movie/<movieId>/<key>
        /// Gets a movie by movieID
        /// </summary>
        /// <param name="movieID"></param>
        /// <returns>TBD</returns>
        [HttpGet("{movieID}/{key}")]
        public ActionResult<MovieDto> Get(int movieID, string key)
        {
            DataLayer dl = new DataLayer();
            MovieDto movieDto = null;
            var results = dl.GetAUserByIDAsync(key);
            if (results.Result != null)
            {
                var movieResults = dl.GetMoviesByMovieIDAsync(movieID);

                if (movieResults.Result != null)
                {
                    movieDto = new MovieDto();
                    movieDto.Title = movieResults.Result.Title;
                    movieDto.PostDate = movieResults.Result.PostDate;
                    movieDto.Summary = movieResults.Result.Summary;
                    movieDto.Link = movieResults.Result.Link;
                    movieDto.ValidMovieDto = true;

                    return Ok(movieDto);
                }
                else
                {
                    return BadRequest("No movie by that ID in the database.");
                }
            }
            else
            {
                return BadRequest(key + " is not a valid user key.");
            }

        } // end Get

        /// <summary>
        /// Get a movie with all ratings for that movie
        /// </summary>
        /// <param name="movieID"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("{movieID}/Ratings/{key}")]
        public ActionResult<Movie> GetMovieWithRatings(int movieID, string key)
        {
            DataLayer dl = new DataLayer();
            var results = dl.GetAUserByIDAsync(key);
            if (results.Result != null)
            {
                var movieResults = dl.GetMovieWithRatingsByMovieIDAsync(movieID);
                if (movieResults.Result != null)
                {
                    return Ok(movieResults.Result);
                }
                else
                {
                    return BadRequest("No movie by that ID in the database.");
                }
            }
            else
            {
                return BadRequest(key + " is not a valid user key.");
            }

        } // end GetMovieWithRatings

        // POST api/<MovieController>
        /// <summary>
        /// /api/v1/movie/<key>
        /// [FromBody new Movie]
        /// Insert a new movie for a valid user. The movie must be passed in the body of the request
        /// </summary>
        /// <param name="value"></param>
        /// <returns>TBD</returns>
        [HttpPost("{key}")]
        public ActionResult<MovieDto> Post(string key, [FromBody] MovieDto movieDto)
        {
            DataLayer dl = new DataLayer();
            var userResults = dl.GetAUserByIDAsync(key);
            if (userResults.Result != null)
            {
                if (BusLogLayer.ValidateMovieDto(movieDto).ValidMovieDto)
                {
                    Movie movie = new Movie();
                    movie.Title = movieDto.Title;
                    movie.Summary = movieDto.Summary;
                    movie.PostDate = movieDto.PostDate;
                    movie.Link = movieDto.Link;
                    movie.OwnerID = key;

                    var results = dl.PostANewMovieAsync(movie);
                    if (results.Result > 0)
                    {
                        movieDto.ValidMovieDto = true;
                        return Ok(movieDto);
                    }
                    else
                    {
                        movieDto.ValidMovieDto = false;
                        return BadRequest(movieDto);
                    }
                }
                else
                {
                    movieDto.ValidMovieDto = false;
                    return BadRequest(movieDto);
                }
            }
            else
            {
                movieDto.ValidMovieDto = false;
                return BadRequest(movieDto);
            }

        } // end post

        /// <summary>
        /// /api/v1/movie/<movieId>/<key>
        /// User can delete a movie by movieId
        /// </summary>
        /// <param name="movieID"></param>
        /// <param name="key"></param>
        /// <returns>TBD</returns>
        [HttpDelete("{movieID}/{key}")]
        public ActionResult<string> Delete(int movieID, string key)
        {
            DataLayer dl = new DataLayer();
            var userResults = dl.GetAUserByIDAsync(key);
            if (userResults.Result != null)
            {
                var results = dl.DeleteAMovieAsync(movieID);
                if (results.Result != 0)
                {
                    return Ok("Successfully delete movie ID: " + movieID);
                }
                else
                {
                    return BadRequest("Not successful deleting movie ID: " + movieID);
                }
            }
            else
            {
                return BadRequest("User key not valid.");
            }
        } // end Delete
    }
}

/// File: RatingsController.cs
/// Name: Cora Yon
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Project 

using Cyon_WebAPI_Project_4.Data;
using Cyon_WebAPI_Project_4.Data_Transfer;
using Cyon_WebAPI_Project_4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cyon_WebAPI_Project_4.Controllers
{
    /// <summary>
    /// Controller for all ratings request
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        /// <summary>
        /// GET: api/v1/Rating/<movieID>/<key>
        /// Get all ratings for a movie by movie id
        /// </summary>
        /// <param name="movieID"></param>
        /// <param name="key"></param>
        /// <returns>TBD</returns>
        [HttpGet("{movieID}/{key}")]
        public ActionResult<string> Get(int movieID, string key)
        {
            DataLayer dl = new DataLayer();
            Task<List<RatingsByMovieIdDto>>? movieRatings = null;
            Task<FinalUser> userTask = dl.GetAUserByIDAsync(key);
            if (userTask.Result != null)
            {
                movieRatings = dl.GetAllRatingsByMovieIDAsync(movieID);
                return Ok(movieRatings.Result);
            }
            else
            {
                return BadRequest("User key is not valid.");
            }
        } // end Get
    }
}

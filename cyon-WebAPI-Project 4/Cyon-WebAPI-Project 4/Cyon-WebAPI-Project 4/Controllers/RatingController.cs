/// File: RatingController.cs
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
    /// Controller for all rating requests
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        /// <summary>
        /// POST api/v1/Rating/movieID/12345
        /// Post a new Movie Rating. A user can not post more than one rating for an movieid
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="key"></param>
        /// <param name="rating"></param>
        /// <returns>TBD</returns>
        [HttpPost]
        public ActionResult<string> Post([FromBody] Rating rating)
        {
            // is key valid
            DataLayer dl = new DataLayer();
            Task<FinalUser> user = dl.GetAUserByIDAsync(rating.UserID);

            if (user.Result != null)
            {
                // validate rating
                var result = BusLogLayer.ValidateRating(rating);
                if (result.ValidRating == true)
                {
                    // post rating
                    var rateResult = dl.PostANewRatingAsync(rating);
                    if (rateResult.Result != 0)
                    {
                        return Ok(rating);
                    }
                    else
                    {
                        return BadRequest("Rating not posted in database.");
                    }
                }
                else
                {
                    return BadRequest(rating);
                }
            }
            else
            {
                return BadRequest("User not valid.");
            }

        } // end Post

        /// <summary>
        /// PATCH api/v1/Rating/articleID/12345
        /// Update the rating for an article for a user
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>TBD</returns>
        [HttpPatch("{movieId}/{key}")]
        public ActionResult<Rating> Patch(int movieID, string key, [FromBody] double value)
        {
            // validate rating value
            if (value < 0 || value > 5)
            {
                return BadRequest("Value " + value + " is out of bounds. Value must be between 0.0 and 5.0");
            }
            else
            {
                if (value < 0 || value > 5)
                {
                    return BadRequest("Value must be between 0 and 5");
                }

                // is key valid
                DataLayer dl = new DataLayer();
                Task<FinalUser> user = dl.GetAUserByIDAsync(key);
                //if valid user, check for the same user as on the rating
                if (user.Result != null)
                {
                    var rating = dl.GetARatingByARatingAsync(movieID, key); //null if not rating found
                    if (rating.Result != null)
                    {
                        Rating tempRating = new()
                        {
                            MovieID = movieID,
                            UserID = key,
                            UserRating = (float)value,
                            ValidRating = true
                        };
                        dl.PutARatingAsync(tempRating);

                        return Ok(tempRating);
                    }
                    else
                    {
                        return BadRequest("You can only change the value on a rating that you have rated");
                    }
                }
                else
                {
                    return BadRequest("Not a valid user");
                }

            } // end Patch
        }

        /// <summary>
        /// DELETE api/v1/Rating/articleID/12345
        /// Delete a rating if an admin
        /// </summary>
        /// <param name="ratingId"></param>
        /// <param name="key"></param>
        /// <returns>TBD</returns>
        [HttpDelete("{key}")]
        public ActionResult<string> Delete(string key, [FromBody] RatingDTO rating)
        {
            // is valid admin key
            // is key valid
            DataLayer dl = new DataLayer();
            Task<FinalUser> user = dl.GetAUserByIDAsync(key);

            if (user.Result != null)
            {
                Rating rating1 = new Rating();
                rating1.MovieID = rating.MovieID;
                rating1.UserID = rating.UserID;

                // delete rating from database
                var result = dl.DeleteARatingAsync(rating1);
                if (result.Result == 1)
                {
                    return Ok(rating);
                }
                else
                {
                    return BadRequest("Rating not deleted");
                }

            }
            else
            {
                return BadRequest("Must be an active user to delete a rating");
            }
        } // end Delete
    }
}

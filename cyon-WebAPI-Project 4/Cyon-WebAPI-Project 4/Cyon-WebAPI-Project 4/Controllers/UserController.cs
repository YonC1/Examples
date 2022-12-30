/// File: UserController.cs
/// Name: Cora Yon
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Project 

using Cyon_WebAPI_Project_4.Data;
using Cyon_WebAPI_Project_4.Data_Transfer;
using Cyon_WebAPI_Project_4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Cyon_WebAPI_Project_4.Controllers
{
    /// <summary>
    /// Controller for all user request
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// GET /api/v1/user/status/<key>
        /// A user can check their status
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TBD</returns>
        [HttpGet("status/{key}")]
        public ActionResult<string> Get(string key)
        {
            bool active = false;

            DataLayer dl = new DataLayer();
            var results = dl.GetAUserByIDAsync(key);
            if (results.Result != null)
            {
                active = results.Result.IsActive;
                return Ok("User " + key + " is active? " + active);
            }
            else
            {
                return BadRequest("User " + key + " not is database.");
            }

            return Ok("Get Status key: " + key);
        }

        /// <summary>
        /// POST /api/v1/user/
        /// This will add a user to the database
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>TBD</returns>
        [HttpPost]
        public ActionResult<UserDto> PostAsUser([FromBody] UserDto userDto)
        {
            // check all data received in the UserDto object
            var validUserDto = BusLogLayer.ValidateUserDto(userDto);

            if (validUserDto.ValidUserDto)
            {
                var email = userDto.Email;
                var firstName = userDto.FirstName;
                var lastName = userDto.LastName;

                // create hashes

                SHA256 sha256Hash = SHA256.Create();
                var hashPass = BusLogLayer.GetHash(sha256Hash, userDto.UserPassword);
                var id = Guid.NewGuid().ToString();

                // create new User Model to submit to database
                FinalUser user = new FinalUser();
                user.Id = id;
                user.Email = email;
                user.Password = hashPass;
                user.FirstName = firstName;
                user.LastName = lastName;
                user.IsActive = true;

                // add to database
                DataLayer dl = new DataLayer();
                var results = dl.PostANewUserAsync(user);

                // return User object showing fields successful
                validUserDto.UserPassword = "NA";

                if (results.Result == 0)
                {
                    validUserDto.ValidUserDto = false;
                    return BadRequest(validUserDto);
                }
                else
                {
                    validUserDto.ValidUserDto = true;
                    return Ok(validUserDto);
                }

            }
            else
            {
                // failed validation
                return BadRequest("Failed validation " + validUserDto);
            }

        } // end PostAsUser


        /// <summary>
        /// PATCH /api/v1/user/<yourkey>/<userkey>?active=<bool>
        /// Admin can update user status to active or inactive
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TBD</returns>
        [HttpPatch("{yourkey}/{userkey}")]
        public ActionResult<string> PatchUpdateStatus(string yourkey, string userkey, [FromQuery] bool status)
        {
            // check for valid key 
            DataLayer dl = new DataLayer();
            Task<FinalUser> adminUser = dl.GetAUserByIDAsync(yourkey);
            if (adminUser.Result != null)
            {
                var results = dl.PatchAUsersStateAsync(userkey, status);
                if (adminUser.Result != null)
                {
                    // user updated, show 200 status code
                    if (results.Result == 0)
                    {
                        return BadRequest("User " + userkey + " not in database.");
                    }
                    else
                    {
                        return Ok("Key: " + userkey + " Status: " + status);
                    }

                }
                else
                {
                    // userkey now found, show bad request 400
                    return BadRequest("Key: " + userkey + " Status: " + status);
                }
            }
            else
            {
                // if not admin key, show that user status was not changed by sending back a bad request status 400
                return BadRequest("Key: " + userkey + " Status: " + status);
            }

        } // end PatchUpdateStatus

        /// <summary>
        /// DELETE /api/v1/user/<userkey>/<yourkey>
        /// User can delete a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TBD</returns>
        [HttpDelete("{yourkey}/{userkey}")]
        public ActionResult<string> Delete(string yourkey, string userkey)
        {

            // check for valid admin key 
            DataLayer dl = new DataLayer();
            Task<FinalUser> yourKeyUser = dl.GetAUserByIDAsync(yourkey);
            if (yourKeyUser.Result != null)
            {
                // check for valid userkey
                var results = dl.DeleteAUserAsync(userkey);
                if (results.Result != 0)
                {
                    // success, status 200
                    return Ok("User: " + userkey + " deleted.");
                }
                else
                {
                    // bad userkey, status 400
                    return BadRequest("User: " + userkey + " not deleted. User key not found OR has existing articles in the database.");
                }
            }
            else
            {
                // bad admin, status 400
                return Ok("User: " + userkey + " not deleted. Admin key not found.");
            }

        } // end Delete
    }
}

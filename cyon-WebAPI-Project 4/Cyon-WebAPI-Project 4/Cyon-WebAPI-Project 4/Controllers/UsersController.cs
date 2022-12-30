/// File: UsersController.cs
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
    /// Controller for all users request
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // POST api/<UsersController>
        /// <summary>
        /// Add many users to the database
        /// </summary>
        /// <param name="adminkey"></param>
        /// <param name="usersDto"></param>
        /// <returns></returns>
        [HttpPost("{adminkey}")]
        public ActionResult<UserDto[]> Post(string adminkey, [FromBody] UserDto[] usersDto)
        {

            DataLayer dl = new DataLayer();
            Task<FinalUser> adminUser = dl.GetAUserByIDAsync(adminkey);
            if (adminUser.Result != null)
            {
                foreach (UserDto temp in usersDto)
                {
                    var validUserDto = BusLogLayer.ValidateUserDto(temp);

                    if (validUserDto.ValidUserDto)
                    {

                        //add user
                        var email = temp.Email;
                        var firstName = temp.FirstName;
                        var lastName = temp.LastName;

                        // create hashes
                        SHA256 sha256Hash = SHA256.Create();
                        var hashPass = BusLogLayer.GetHash(sha256Hash, temp.UserPassword);
                        var guid = Guid.NewGuid().ToString();

                        // create new User Model to submit to database
                        FinalUser user = new FinalUser();
                        user.Id = guid;
                        user.Email = email;
                        user.Password = hashPass;
                        user.FirstName = firstName;
                        user.LastName = lastName;
                        user.IsActive = false;

                        // add to database                
                        var results = dl.PostANewUserAsync(user);
                        if (results.Result == 0)
                        {
                            temp.Email = "Duplicate User";
                            temp.FirstName = "Duplicate User";
                            temp.LastName = "Duplicate User";
                            temp.UserPassword = "Duplicate User";
                            temp.ValidUserDto = false;
                        }
                        else
                        {
                            temp.UserPassword = "Not Available";
                        }

                    }
                }

                return Ok(usersDto);
            }
            else
            {
                // bad request status, 400
                UserDto[] notAdmin = new UserDto[1];
                UserDto badUser = new UserDto();
                badUser.UserPassword = "Not a valid administrator";
                badUser.Email = "Not a valid administrator";
                badUser.FirstName = "Not a valid administrator";
                badUser.LastName = "Not a valid administrator";
                badUser.ValidUserDto = false;
                notAdmin[0] = badUser;
                return BadRequest(notAdmin);
            }

        } // end Post

    }
}

/// File: UserDto.cs
/// Name: Cora Yon
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Project 4
namespace Cyon_WebAPI_Project_4.Data_Transfer
{
    /// <summary>
    /// This class used to communicate with any client
    /// </summary>
    public class UserDto
    {
        public string? Email { get; set; }
        public string? UserPassword { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool ValidUserDto { get; set; }

        /// <summary>
        /// This method provides an easy to output the contents of the object
        /// Used mostly for testing
        /// </summary>
        /// <returns>Return all properties in UserDto</returns>
        public override string? ToString()
        {
            return "Email: " + Email + " User Password: " + UserPassword + " First Name: " + FirstName + " Last Name: " + LastName;
        }
    }
}

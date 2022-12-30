/// File: FinalUser.cs
/// Name: Cora Yon
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Article Jet

namespace Cyon_WebAPI_Project_4.Models
{
    /// <summary>
    /// This class matches the database table FinalUser for CRUD operations
    /// </summary>
    public class FinalUser
    {
        /// <summary>
        /// This is an entity class for the FinalUser table in the database
        /// </summary>
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Boolean IsActive { get; set; }

        /// <summary>
        /// This method provides an easy to output the contents of the object
        /// mostly used for testing
        /// </summary>
        /// <returns>String</returns>
        public override string? ToString()
        {
            return "Id: " + Id + " Email: " + Email + " Password: " + Password + " First Name: " + FirstName + " Last Name: " + LastName + " IsActive" + IsActive;
        }
    }
}

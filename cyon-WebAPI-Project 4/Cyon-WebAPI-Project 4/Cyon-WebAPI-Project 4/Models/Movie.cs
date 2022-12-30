/// File: Movie.cs
/// Name: Cora Yon
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Project 4

namespace Cyon_WebAPI_Project_4.Models
{
    /// <summary>
    /// This class matches the database table Movie for CRUD operations
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// This is an entity class for the Movie table in the database
        /// </summary>
        public int MovieID { get; set; }
        public string? Title { get; set; }
        public DateTime PostDate { get; set; }
        public string? Summary { get; set; }
        public string? Link { get; set; }
        public string? OwnerID { get; set; }

        /// <summary>
        /// This method provides an easy to output the contents of the object
        /// mostly used for testing
        /// </summary>
        /// <returns>String representing all properties in an Article</returns>
        public override string? ToString()
        {
            return "Movie ID: " + MovieID + " Title: " + "" + Title + " Post Date: " + PostDate + " Summary: " + Summary + " Link: " + Link + " OwnerID: " + OwnerID;
        }
    }
}

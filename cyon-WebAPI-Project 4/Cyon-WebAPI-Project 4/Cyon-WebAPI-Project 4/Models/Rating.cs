/// File: Rating.cs
/// Name: Cora Yon
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Project 

namespace Cyon_WebAPI_Project_4.Models
{
    /// <summary>
    /// Entity object to match database table Rating
    /// </summary>
    public class Rating
    {
        /// <summary>
        /// Class Properties, entity to match database table Rating
        /// </summary>
        public int MovieID { get; set; }
        public string? UserID { get; set; }
        public float UserRating { get; set; }
        public bool ValidRating { get; set; }

        /// <summary>
        /// Output properties for testing, mostly
        /// </summary>
        /// <returns></returns>
        public override string? ToString()
        {
            return "Movie ID: " + MovieID + " User ID: " + UserID + " User Rating: " + UserRating;
        }
    }
}

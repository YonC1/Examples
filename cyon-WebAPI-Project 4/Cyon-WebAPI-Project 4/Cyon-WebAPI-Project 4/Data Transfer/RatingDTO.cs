/// File: RatingDto.cs
/// Name: Cora Yon
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Project 4

namespace Cyon_WebAPI_Project_4.Data_Transfer
{
    /// <summary>
    /// This class used to communicate with any client
    /// </summary>
    public class RatingDTO
    {
        public int MovieID { get; set; }
        public string? UserID { get; set; }

        /// <summary>
        /// Output properties for testing, mostly
        /// </summary>
        /// <returns></returns>
        public override string? ToString()
        {
            return "Movie ID: " + MovieID + " User ID: " + UserID;
        }
    }
}

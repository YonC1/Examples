/// File: RatingWithName.cs
/// Name: Cora Yon
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Project 4
namespace Cyon_WebAPI_Project_4.Data_Transfer
{
    /// <summary>
    /// Class for communicating with client
    /// </summary>
    public class RatingWithNameDto
    {
        public double Rating { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

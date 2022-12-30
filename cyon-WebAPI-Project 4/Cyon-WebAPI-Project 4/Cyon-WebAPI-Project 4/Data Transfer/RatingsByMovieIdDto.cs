/// File: RatingsbyMovieIdDto.cs
/// Name: Cora Yon
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Project 4

namespace Cyon_WebAPI_Project_4.Data_Transfer
{
    /// <summary>
    /// This class used to communicate with any client
    /// </summary>
    public class RatingsByMovieIdDto
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public double Value { get; set; }
    }
}

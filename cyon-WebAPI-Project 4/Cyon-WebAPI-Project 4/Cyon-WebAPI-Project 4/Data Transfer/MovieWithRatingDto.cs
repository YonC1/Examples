/// File: ArticleWithRatingDto.cs
/// Name: Cora Yon
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Project 4

namespace Cyon_WebAPI_Project_4.Data_Transfer
{
    /// <summary>
    /// Class used for communicating with client
    /// </summary>
    public class MovieWithRatingDto
    {
        public string Title { get; set; }
        public double AvgRating { get; set; }
    }
}

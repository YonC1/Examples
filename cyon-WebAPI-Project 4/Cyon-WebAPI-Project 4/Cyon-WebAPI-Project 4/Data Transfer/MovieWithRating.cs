/// File: MovieWithRating.cs
/// Name: Cora Yon
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Article Jet

namespace Cyon_WebAPI_Project_4.Data_Transfer
{
    /// <summary>
    /// Class for communicating with client
    /// </summary>
    public class MovieWithRating
    {
        public string Title { get; set; }
        public DateTime PostDate { get; set; }
        public string Summary { get; set; }
        public string Link { get; set; }
        public List<RatingWithNameDto>? ratingWithNameDto { get; set; }
    }
}

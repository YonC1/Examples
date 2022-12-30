namespace Cyon_WebAPI_Project_4.Data_Transfer
{
    public class MovieDto
    {
        public string Title { get; set; }
        public DateTime PostDate { get; set; }
        public string Summary { get; set; }
        public string Link { get; set; }
        public bool ValidMovieDto { get; set; }

        //public string Key { get; set; }

        /// <summary>
        /// This method provides an easy to output the contents of the object
        /// mostly used for testing
        /// </summary>
        /// <returns>String representing all properties in an Article</returns>
        public override string? ToString()
        {
            return "Title: " + Title + " PostDate: " + PostDate.ToShortDateString + " Summary: " + Summary + " Link: " + Link; // + " Key: " + Key;
        }
    }
}

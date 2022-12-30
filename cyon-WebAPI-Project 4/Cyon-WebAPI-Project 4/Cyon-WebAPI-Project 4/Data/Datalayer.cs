using Cyon_WebAPI_Project_4.Data_Transfer;
using Cyon_WebAPI_Project_4.Models;
using MySql.Data.MySqlClient;
using System.Data;
/// File: DataLayer.cs
/// Name: Cora Yon
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Article Jet

namespace Cyon_WebAPI_Project_4.Data;
    /// <summary>
    /// The data DataLayer class is used to hide implementation details of
    /// connecting to the database doing standard CRUD operations.
    /// 
    /// IMPORTANT NOTES:
    /// On the serverside, any input-output operations should be done asynchronously. This includes
    /// file and database operations. In doing so, the thread is freed up for the entire time a request
    /// is in flight. When a request executes the await code, the request thread is returned back to the
    /// thread pool. When the request is satisfied, the thread is taken from the thread pool and continues.
    /// This is all built into the .NET Core Framework making it very easy to implement into our code.
    /// 
    /// When throwing an exception from an ASYNC function the exception is never thrown back to the calling entity. 
    /// This makes sense because the function could possibly block and cause strange and unexpected 
    /// behavior. Instead, we will LOG the exception.
    /// </summary>
    internal class DataLayer
    {

        #region "Properties"

        /// <summary>
        /// This variable holds the connection details
        /// such as name of database server, database name, username, and password.
        /// The ? makes the property nullable
        /// </summary>
        private readonly string? connectionString = null;

        #endregion

        #region "Constructors"

        /// <summary>
        /// This is the default constructor and has the default 
        /// connection string specified 
        /// </summary>
        public DataLayer()
        {
            //preprocessor directives can help by using a debug build database environment for testing
            // while using a production database environment for production build.
#if (DEBUG)
            connectionString = @"server=localhost;uid=root;pwd=NyxBart20;database=FinalProject";
#else
            connectionString = @"server=192.168.79.131;uid=root;pwd=NyxBart20;database=FinalProject";
#endif
        }

        /// <summary>
        /// Parameterized Constructor passing in a connection string
        /// </summary>
        /// <param name="connectionString"></param>
        public DataLayer(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #endregion

        #region "User Database Operations"

        /// <summary>
        /// Get a user by using the user ID (key)
        /// returns a single User object or a null User
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<FinalUser>? GetAUserByIDAsync(string? ID)
        {

            FinalUser? user = null;

            try
            {

                //test for guid to be null and throw and exception back to the caller
                if (ID == null)
                {
                    throw new ArgumentNullException("ID can not be null.");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open the database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetAUserById", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("Id", ID));

                // execute the command which returns a data reader object
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // if the reader contains a data set, load a local user object
                if (rdr.Read())
                {
                    user = new FinalUser();
                    user.Id = (string?)rdr.GetValue(0);
                    user.Email = (string?)rdr.GetValue(1);
                    user.Password = (string?)rdr.GetValue(2);
                    user.FirstName = (string?)rdr.GetValue(3);
                    user.LastName = (string?)rdr.GetValue(4);
                    UInt64 test = (UInt64)rdr.GetValue(5);
                    if (test == 1)
                        user.IsActive = true;
                    else
                        user.IsActive = false;
                }
            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return user;

        } // end GetAUserByIDAsync

        /// <summary>
        /// Get a user by Username and Password (key)
        /// returns a single User object or a null User
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<FinalUser>? GetAUserByUserPassAsync(string? username, string? password)
        {

            FinalUser? user = null; // new syntax can replace User? user = new User()

            try
            {
                if (username == null || password == null)
                {
                    throw new ArgumentNullException("Username or Password can not be null.");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open the database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetAUserByUserAndPass", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameters to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("userEmail", username));
                cmd.Parameters.Add(new MySqlParameter("password", password));

                // execute the command which returns a data reader object
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // if the reader contains a data set, load a local user object
                if (rdr.Read())
                {
                    user = new FinalUser();
                    user.Id = (string?)rdr.GetValue(0);
                    user.Email = (string?)rdr.GetValue(1);
                    user.Password = (string?)rdr.GetValue(2);
                    user.FirstName = (string?)rdr.GetValue(3);
                    user.LastName = (string?)rdr.GetValue(4);
                    UInt64 test = (UInt64)rdr.GetValue(5);
                    if (test == 1)
                        user.IsActive = true;
                    else
                        user.IsActive = false;
                }
            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return user;

        } // end GetAUserByUserPassAsync

        /// <summary>
        /// Get all the active users in the database and return as a List of Users.
        /// If none are found, then the List will have a Count of 0.
        /// </summary>
        /// <returns></returns>
        public async Task<List<FinalUser>>? GetAllUsersAsync()
        {

            // instantiate a new empty List of Users
            List<FinalUser> users = new List<FinalUser>();

            try
            {
                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open the database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetAllUsers", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // execute the command which returns a data reader object
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // iterate through results adding a new user to a generic List of users
                while (rdr.Read())
                {
                    FinalUser user = new FinalUser();

                    user.Id = (string?)rdr.GetValue(0);
                    user.Email = (string?)rdr.GetValue(1);
                    user.Password = (string?)rdr.GetValue(2);
                    user.FirstName = (string?)rdr.GetValue(3);
                    user.LastName = (string?)rdr.GetValue(4);
                    UInt64 test = (UInt64)rdr.GetValue(5);
                    if (test == 1)
                        user.IsActive = true;
                    else
                        user.IsActive = false;

                    users.Add(user);
                }
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return users;

        } // end GetAllUsersAsync

    public async Task<int> PatchAUsersStateAsync(string? guid, bool? state)
    {
        // integer value that shows if a row is updated in the database
        int results = 0;

        try
        {
            // check for null parameters being passed in
            if (guid == null || state == null)
            {
                throw new ArgumentNullException("ID and State can not be null.");
            }

            //using guarentees the release of resources at the end of scope 
            using MySqlConnection conn = new MySqlConnection(connectionString);

            // open the database connection
            conn.Open();

            // create a command object identifying the stored procedure
            using MySqlCommand cmd = new MySqlCommand("spPutUserActiveState", conn);

            // set the command object so it knows to execute a stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // add parameters to command, which will be passed to the stored procedure
            cmd.Parameters.Add(new MySqlParameter("GUID", guid));
            cmd.Parameters.Add(new MySqlParameter("state", state));

            // execute the none query command that returns an integer for number of rows changed
            results = await cmd.ExecuteNonQueryAsync();

        }
        catch (ArgumentNullException ex)
        {
            LoggerJet lj = new LoggerJet();
            lj.WriteLog(ex.Message);
        }
        catch (MySqlException ex)
        {
            LoggerJet lj = new LoggerJet();
            lj.WriteLog(ex.Message);
        }
        catch (Exception ex)
        {
            LoggerJet lj = new LoggerJet();
            lj.WriteLog(ex.Message);
        }
        finally
        {
            // no clean up because the 'using' statements guarantees closing resources
        }

        return results;

    } // end PatchAUsersStateAsync



    /// <summary>
    /// Delete a user using the users ID (key)
    /// </summary>
    /// <param name="userGuid"></param>
    /// <returns>int</returns>
    public async Task<int> DeleteAUserAsync(string? userID)
    {
        // integer value that shows if a row is updated in the database
        int results = 0;

        try
        {
            // check for null parameters being passed in
            if (userID == null)
            {
                throw new ArgumentNullException("ID can not be null.");
            }

            //using guarentees the release of resources at the end of scope 
            using MySqlConnection conn = new MySqlConnection(connectionString);

            // open the database connection
            conn.Open();

            // create a command object identifying the stored procedure
            using MySqlCommand cmd = new MySqlCommand("spDeleteAUserByGuid", conn);

            // set the command object so it knows to execute a stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // add parameters to command, which will be passed to the stored procedure
            cmd.Parameters.Add(new MySqlParameter("GUID", userID));

            // execute the none query command that returns an integer for number of rows changed
            results = await cmd.ExecuteNonQueryAsync();

        }
        catch (ArgumentNullException ex)
        {
            LoggerJet lj = new LoggerJet();
            lj.WriteLog(ex.Message);
        }
        catch (MySqlException ex)
        {
            LoggerJet lj = new LoggerJet();
            lj.WriteLog(ex.Message);
        }
        catch (Exception ex)
        {
            LoggerJet lj = new LoggerJet();
            lj.WriteLog(ex.Message);
        }
        finally
        {
            // no clean up because the 'using' statements guarantees closing resources
        }

        return results;

    } // end DeleteAUserAsync



    /// <summary>
    /// Insert a new User into the database.
    /// Return 0 if no row is modified.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<int>? PostANewUserAsync(FinalUser? user)
        {
            // local variable to return the row count altered
            int results = 0;

            try
            {
                // check for User to be null
                if (user == null)
                {
                    throw new ArgumentNullException("New user can not be null.");
                }

                // using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open the database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spPostNewFinalUser", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameters to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("Id", user.Id));
                cmd.Parameters.Add(new MySqlParameter("mail", user.Email));
                cmd.Parameters.Add(new MySqlParameter("pass", user.Password));
                cmd.Parameters.Add(new MySqlParameter("fname", user.FirstName));
                cmd.Parameters.Add(new MySqlParameter("lname", user.LastName));
                cmd.Parameters.Add(new MySqlParameter("isActive", user.IsActive));

                // execute the command
                results = await cmd.ExecuteNonQueryAsync();

            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return results;

        } // end PostANewUserAsync

        #endregion

        #region "Movie Database Operations"

        /// <summary>
        /// Get all the Movies in the database and return as a List of Movies.
        /// If none are found, then the List will have a Count of 0.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Movie>>? GetAllMoviesAsync()
        {

            // Instantiate new List of Movies
            List<Movie> movies = new(); // new way to instantiate, old way: List<Movie> movies = new List<Movie>()

            try
            {
                // using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open the database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetAllMovies", conn);
                //using SqlCommand cmd = new SqlCommand("CustOrderHist", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // execute the command
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // iterate through results adding a new Article to the List to return
                while (rdr.Read())
                {
                    Movie movie = new Movie();
                    movie.MovieID = (int)rdr.GetValue("MovieID");
                    movie.Title = (string?)rdr.GetValue("Title");
                    movie.PostDate = (DateTime)rdr.GetValue("Postdate");
                    movie.Summary = (string?)rdr.GetValue("Summary");
                    movie.Link = (string?)rdr.GetValue("Link");
                    movie.OwnerID = (string?)rdr.GetValue("OwnerID");

                    movies.Add(movie);
                }
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return movies;

        } // end GetAllMoviesAsync

        //spGetAllArticlesWithAveRating
        public async Task<List<MovieWithRatingDto>>? GetAllMoviesWithAveRating()
        {

            // Instantiate new List of Movies
            List<MovieWithRatingDto> movies = new(); // new way to instantiate, old way: List<Article> articles = new List<Article>()

            try
            {
                // using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open the database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetAllMoviesWithAveRating", conn);
                //using SqlCommand cmd = new SqlCommand("CustOrderHist", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // execute the command
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // iterate through results adding a new Article to the List to return
                while (rdr.Read())
                {
                    MovieWithRatingDto movie = new MovieWithRatingDto();
                    movie.Title = (string?)rdr.GetValue("Title");
                    movie.AvgRating = (double)rdr.GetValue("Avg_Rating");

                    movies.Add(movie);
                }
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return movies;

        } // end GetAllMoviessWithAveRating

        /// <summary>
        /// Get a single Article in the database by the ArticleID (auto increment int).
        /// If none are found, return a null object.
        /// </summary>
        /// <param name="MovieID"></param>
        /// <returns></returns>
        public async Task<Movie>? GetMoviesByMovieIDAsync(int? MovieID)
        {
            // instantiate new Movie object
            Movie? movie = null;

            try
            {
                if (MovieID == null)
                {
                    throw new ArgumentNullException("Movie ID can not be null.");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetMovieByMovieID", conn);
                //using SqlCommand cmd = new SqlCommand("CustOrderHist", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("ID", MovieID));

                // execute the command and return a data reader
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // Load a new article and return to caller
                if (rdr.Read())
                {
                    movie = new Movie();
                    movie.Title = (string?)rdr.GetValue("Title");
                    movie.PostDate = (DateTime)rdr.GetValue("Postdate");
                    movie.Summary = (string?)rdr.GetValue("Summary");
                    movie.Link = (string?)rdr.GetValue("Link");
                    movie.OwnerID = (string?)rdr.GetValue("OwnerID");
                }
            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return movie;

        } // end GetArticlesByArticleIDAsync

        // TODO - Finish

    /// <summary>
    /// Returns a movie with ratings by its id 
    /// </summary>
    /// <param name="MovieID"></param>
    /// <returns></returns>
        public async Task<MovieWithRating>? GetMovieWithRatingsByMovieIDAsync(int? MovieID)
        {
            bool once = false;

            // instantiate new Article object
            MovieWithRating? movie = null;

            try
            {
                if (MovieID == null)
                {
                    throw new ArgumentNullException("Movie ID can not be null.");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetMoviesAndRatingsByMovieID", conn);
                //using SqlCommand cmd = new SqlCommand("CustOrderHist", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("ID", MovieID));

                // execute the command and return a data reader
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // Load a new article and return to caller
                if (rdr.Read())
                {
                    movie = new();
                    movie.Title = (string)rdr.GetValue("Title");
                    movie.PostDate = (DateTime)rdr.GetValue("Postdate");
                    movie.Summary = (string)rdr.GetValue("Summary");
                    movie.Link = (string)rdr.GetValue("Link");
                }

                if (movie != null)
                {
                    //spGetAllUserRatingsForAnArticle
                    //using guarentees the release of resources at the end of scope 
                    using MySqlConnection conn2 = new MySqlConnection(connectionString);

                    // open database connection
                    conn2.Open();

                    // create a command object identifying the stored procedure
                    using MySqlCommand cmd2 = new MySqlCommand("spGetAllUserRatingsForAMovie", conn2);
                    //using SqlCommand cmd = new SqlCommand("CustOrderHist", conn);

                    // set the command object so it knows to execute a stored procedure
                    cmd2.CommandType = CommandType.StoredProcedure;

                    // add parameter to command, which will be passed to the stored procedure
                    cmd2.Parameters.Add(new MySqlParameter("movId", MovieID));

                    // execute the command and return a data reader
                    using MySqlDataReader rdr2 = (MySqlDataReader)await cmd2.ExecuteReaderAsync();

                    List<RatingWithNameDto> ratings = new();

                    while (rdr2.Read())
                    {
                        RatingWithNameDto rating = new();

                        rating.Rating = (double)rdr2.GetValue("Rating");
                        rating.FirstName = (string)rdr2.GetValue("FirstName");
                        rating.LastName = (string)rdr2.GetValue("LastName");
                        ratings.Add(rating);
                    }

                    movie.ratingWithNameDto = ratings;
                }

            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return movie;

        } // end GetArticleWithRatingsByArticleIDAsync

        /// <summary>
        /// Get all Movies that are equal to or greater than the date passed in.
        /// If none are found, return list with count 0.
        /// </summary>
        /// <param name="movieDate"></param>
        /// <returns></returns>
        public async Task<List<Movie>>? GetMoviesAfterDateAsync(DateTime? movieDate)
        {

            // Instantiate new List of Articles
            List<Movie> movies = new();

            try
            {
                // check for null date
                if (movieDate == null)
                {
                    throw new ArgumentNullException("The Movie data can not be null");
                }
                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetMoviesAfterDate", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("movieDate", movieDate));

                // execute the command that returns a data reader
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // iterate through results loading the Article List<>
                while (rdr.Read())
                {
                    Movie movie = new Movie();

                    movie.MovieID = (int)rdr.GetValue("MovieID");
                    movie.Title = (string?)rdr.GetValue("Title");
                    movie.Summary = (string?)rdr.GetValue("Summary");
                    movie.Link = (string?)rdr.GetValue("Link");
                    movie.OwnerID = (string?)rdr.GetValue("OwnerID");

                    movies.Add(movie);
                }
            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return movies;

        } // end GetArticlesAfterDateAsync

        /// <summary>
        /// Adds a new Movie to the database.
        /// Returns an integer for the number of rows affected.
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int>? PostANewMovieAsync(Movie? movie)
        {

            // local variable for returning row count
            int results = 0;

            try
            {
                // check for null article being passed in
                if (movie == null)
                {
                    throw new ArgumentNullException("New movie can not be null.");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // Open database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spPostANewMovie", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // load all parameters to pass to stored procedure
                cmd.Parameters.Add(new MySqlParameter("movieTitle", movie.Title));
                cmd.Parameters.Add(new MySqlParameter("movieDate", movie.PostDate));
                cmd.Parameters.Add(new MySqlParameter("movieSummary", movie.Summary));
                cmd.Parameters.Add(new MySqlParameter("movieLink", movie.Link));
                cmd.Parameters.Add(new MySqlParameter("movieOwner", movie.OwnerID));

                // execute command and get number of rows affected
                results = await cmd.ExecuteNonQueryAsync();

            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return results;

        } // end PostANewArticleAsync

        /// <summary>
        /// Update an existing Article changing everything except the ArticleID (key).
        /// Returns an integer showing how many rows in the database was affected.
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int>? PutAMovieAsync(Movie? movie)
        {

            // local variable to return the number of rows affected in the database
            int results = 0;

            try
            {

                // check for null parameter
                if (movie == null)
                {
                    throw new ArgumentNullException("Movie can not be null.");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spPutAMovie", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameter to command, which will be passed to the stored procedure                
                cmd.Parameters.Add(new MySqlParameter("ID", movie.MovieID));
                cmd.Parameters.Add(new MySqlParameter("movieTitle", movie.Title));
                cmd.Parameters.Add(new MySqlParameter("movieDate", movie.PostDate));
                cmd.Parameters.Add(new MySqlParameter("movieSummary", movie.Summary));
                cmd.Parameters.Add(new MySqlParameter("movieLink", movie.Link));
                cmd.Parameters.Add(new MySqlParameter("movieOwner", movie.OwnerID));

                // execute the command and get number of rows affected
                results = await cmd.ExecuteNonQueryAsync();

            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return results;

        } // end PutAnArticleAsync

        /// <summary>
        /// Delete a single Article.
        /// Returns an integer showing how many rows in the database was affected.
        /// </summary>
        /// <param name="movieID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int>? DeleteAMovieAsync(int? movieID)
        {
            // local variable for returning total rows affected in the database
            int results = 0;

            try
            {
                // check for null id being passed in
                if (movieID == null)
                {
                    throw new ArgumentNullException("New movie can not be null.");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spDeleteMovieByMovieID", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameter to command, which will be passed to the stored procedure                
                cmd.Parameters.Add(new MySqlParameter("ID", movieID));

                // execute the command that returns the number of rows affected                
                results = await cmd.ExecuteNonQueryAsync();

            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return results;
        } // end DeleteAnArticleAsync

        #endregion

        #region "Rating Database Operations"

        /// <summary>
        /// Get all the Ratings in the database and return as a List.
        /// If none are found, then the List will have a Count of 0.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Rating>>? GetAllRatingsAsync()
        {
            // Instantiate Ratings List
            List<Rating> ratings = new();

            try
            {
                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetAllRatings", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // execute the command
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // iterate through results loading Rating List
                while (rdr.Read())
                {
                    Rating rating = new();

                    //rating.RatingID = (int)rdr.GetValue("RatingID");
                    rating.MovieID = (int)rdr.GetValue("MovieID");
                    rating.UserID = (string?)rdr.GetValue("UserID");
                    rating.UserRating = (float)rdr.GetValue("Rating");

                    ratings.Add(rating);
                }
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return ratings;

        } // end GetAllRatingsAsync

        /// <summary>
        /// Get all the Ratings in the database by the User that submitted the rating and return as a List.
        /// If none are found, then the List will have a Count of 0.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<List<Rating>>? GetAllRatingsByAUserAsync(string? ID)
        {

            // Instantiate new List of Ratings
            List<Rating> ratings = new();

            try
            {
                // check parameter for null
                if (ID == null)
                {
                    throw new ArgumentNullException("ID can not be null");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open database connection 
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetAllRatingsByAUser", conn);
                //using SqlCommand cmd = new SqlCommand("CustOrderHist", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("ID", ID));

                // execute the command
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // iterate through results loading the Ratings List
                while (rdr.Read())
                {
                    Rating rating = new();

                    rating.MovieID = (int)rdr.GetValue("MovieID");
                    rating.UserID = (string?)rdr.GetValue("UserID");
                    rating.UserRating = (float)rdr.GetValue("Rating");

                    ratings.Add(rating);
                }
            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return ratings;

        } // end GetAllRatingsByAUserAsync

        /// <summary>
        /// Get all the Ratings in the database by the Rating (ex: 4) and return as a List.
        /// If none are found, then the List will have a Count of 0.
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        public async Task<Rating>? GetARatingByARatingAsync(int movieID, string userID)
        {

            // Instantiage ratings List
            Rating rating = null;

            try
            {
                // check parameter to be null
                if (movieID == 0 || userID == null)
                {
                    throw new ArgumentNullException("Rating number may not be zero or null");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // Open database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetARatingByARating", conn);
                //using SqlCommand cmd = new SqlCommand("CustOrderHist", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("mID", movieID));
                cmd.Parameters.Add(new MySqlParameter("uID", userID));

                // execute the command
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // iterate through results loading List of Ratings
                if (rdr.Read())
                {
                    rating = new();
                    rating.MovieID = (int)rdr.GetValue("MovieID");
                    rating.UserID = (string?)rdr.GetValue("UserID");
                    rating.UserRating = (float)rdr.GetValue("Rating");
                }
            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return rating;

        } // end GetArticlesAfterDateAsync

        /// <summary>
        /// Get all the Ratings in the database for a movie and return as a List.
        /// If none are found, then the List will have a Count of 0.
        /// </summary>
        /// <param name="MovieID"></param>
        /// <returns></returns>
        public async Task<List<RatingsByMovieIdDto>>? GetAllRatingsByMovieIDAsync(int? MovieID)
        {

            // Instantiate Ratings List
            List<RatingsByMovieIdDto> ratings = new();

            try
            {
                // check parameter to be null
                if (MovieID == null)
                {
                    throw new ArgumentNullException("MovieID can not be null");
                }

                // using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                // open database connection
                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spGetAllRatingsByID", conn);
                //using SqlCommand cmd = new SqlCommand("CustOrderHist", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("movID", MovieID));

                // execute the command
                using MySqlDataReader rdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                // iterate through results
                while (rdr.Read())
                {
                    RatingsByMovieIdDto rating = new();

                    rating.Title = (string)rdr.GetValue("Title");
                    rating.FirstName = (string)rdr.GetValue("FirstName");
                    rating.Value = (float)rdr.GetValue("Rating");

                    ratings.Add(rating);
                }
            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return ratings;

        } // end GetAllRatingsByMovieIDAsync


    /// <summary>
    /// Submit a new Rating for a movie.
    /// Return an integer for how many rows were affected in the database.
    /// </summary>
    /// <param name="rating"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<int>? PostANewRatingAsync(Rating? rating)
    {

        // returns the affected row count from the database
        int results = 0;

        try
        {

            // make sure the Rating parameter is not null
            if (rating == null)
            {
                throw new ArgumentNullException("Rating can not be null.");
            }

            // using guarentees the release of resources at the end of scope 
            using MySqlConnection conn = new MySqlConnection(connectionString);

            conn.Open();

            // create a command object identifying the stored procedure
            using MySqlCommand cmd = new MySqlCommand("spPostANewRating", conn);

            // set the command object so it knows to execute a stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // add parameter to command, which will be passed to the stored procedure                
            cmd.Parameters.Add(new MySqlParameter("movID", rating.MovieID));
            cmd.Parameters.Add(new MySqlParameter("uID", rating.UserID));
            cmd.Parameters.Add(new MySqlParameter("ratingValue", rating.UserRating));

            // execute the command
            results = await cmd.ExecuteNonQueryAsync();

        }
        catch (ArgumentNullException ex)
        {
            LoggerJet lj = new LoggerJet();
            lj.WriteLog(ex.Message);
        }
        catch (MySqlException ex)
        {
            LoggerJet lj = new LoggerJet();
            lj.WriteLog(ex.Message);
        }
        catch (Exception ex)
        {
            LoggerJet lj = new LoggerJet();
            lj.WriteLog(ex.Message);
        }
        finally
        {
            // no clean up because the 'using' statements guarantees closing resources
        }

        return results;

    } // end PostANewRatingAsync


    /// <summary>
    /// Update a new Rating for a movie.
    /// Return an integer for how many rows were affected in the database.
    /// </summary>
    /// <param name="rating"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<int>? PutARatingAsync(Rating? rating)
        {

            // returns the affected row count from the database
            int results = 0;

            try
            {
                // make sure the Rating parameter is not null
                if (rating == null)
                {
                    throw new ArgumentNullException("Rating can not be null.");
                }

                // using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spPutARating", conn);
                //using SqlCommand cmd = new SqlCommand("CustOrderHist", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameter to command, which will be passed to the stored procedure                
                cmd.Parameters.Add(new MySqlParameter("movID", rating.MovieID));
                cmd.Parameters.Add(new MySqlParameter("uID", rating.UserID));
                cmd.Parameters.Add(new MySqlParameter("movieRating", rating.UserRating));

                // execute the command                
                results = await cmd.ExecuteNonQueryAsync();

            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return results;

        } // end PutARatingAsync

        /// <summary>
        /// Delete a new Rating for an Article.
        /// Return an integer for how many rows were affected in the database.
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int>? DeleteARatingAsync(Rating? rating)
        {

            // returns the affected row count from the database
            int results = 0;

            try
            {
                // make sure the Rating parameter is not null
                if (rating == null)
                {
                    throw new ArgumentNullException("Rating can not be null.");
                }

                //using guarentees the release of resources at the end of scope 
                using MySqlConnection conn = new MySqlConnection(connectionString);

                conn.Open();

                // create a command object identifying the stored procedure
                using MySqlCommand cmd = new MySqlCommand("spDeleteRatingByUserIDMovieID", conn);
                //using SqlCommand cmd = new SqlCommand("CustOrderHist", conn);

                // set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // add parameter to command, which will be passed to the stored procedure                
                cmd.Parameters.Add(new MySqlParameter("movID", rating.MovieID));
                cmd.Parameters.Add(new MySqlParameter("uID", rating.UserID));

                // execute the command                
                results = await cmd.ExecuteNonQueryAsync();

            }
            catch (ArgumentNullException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (MySqlException ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            catch (Exception ex)
            {
                LoggerJet lj = new LoggerJet();
                lj.WriteLog(ex.Message);
            }
            finally
            {
                // no clean up because the 'using' statements guarantees closing resources
            }

            return results;
        } // end DeleteARatingAsync

        #endregion
    }
